using AutoMapper;
using cloud.DTO.Requests.Files;
using cloud.DTO.Responses.Files;
using cloud.Enums;
using cloud.Exceptions;
using cloud.Models;
using cloud.Repositories.Files;
using cloud.Repositories.Users;
using cloud.Services.Files.FileWorkers.Browser;
using cloud.Services.Files.FileWorkers.Uploader;

namespace cloud.Services.Files {
    public class FileService : IFileService {
        private readonly IFileRepository repository;
        private readonly IUserRepository userRepository;
        private readonly IFileUploaderService uploader;
        private readonly IFileBrowserService browser;
        private readonly IMapper mapper;

        public FileService(IFileRepository repository, IUserRepository userRepository, IFileUploaderService uploader, IFileBrowserService browser, IMapper mapper) {
            this.repository = repository;
            this.userRepository = userRepository;
            this.uploader = uploader;
            this.browser = browser;
            this.mapper = mapper;
        }

        public async Task<List<FileResponse>> GetUserFilesAsync(string userId) {
            var files = await repository.GetUserFilesAsync(userId);
            return mapper.Map<List<FileResponse>>(files);
        }

        public async Task<FileResponse> GetFileByIdAsync(string requesterId, string id) {
            var file = await ValidateSharedAccessAsync(requesterId, id);
            return mapper.Map<FileResponse>(file);
        }

        public async Task<List<SharedFileResponse>> GetFromSharedFilesAsync(string userId) {
            var sharedFiles = await repository.GetFromSharedFilesAsync(userId);
            return await PopulateSharedFileResponsesAsync(sharedFiles);
        }

        public async Task<List<SharedFileResponse>> GetWithSharedFilesAsync(string userId) {
            var sharedFiles = await repository.GetWithSharedFilesAsync(userId);
            return await PopulateSharedFileResponsesAsync(sharedFiles);
        }

        public async Task<List<FileResponse>> UploadFilesAsync(string userId, IFormFileCollection files) {
            var user = await userRepository.GetUserByIdAsync(userId);
            if (userRepository.HasStorageLimit(user!, out long available)) {
                long uploadSize = 0;
                foreach (var file in files) {
                    uploadSize += file.Length;
                }

                if (uploadSize > available) {
                    throw new InvalidActionException("Загруженные файлы превышают лимит");
                }
            }

            foreach (var file in files) {
                if (!ValidateFileName(file.FileName)) {
                    throw new InvalidActionException($"Имя файла {file.FileName} содержит запрещенные символы");
                } else if (uploader.FileExists(userId, file.FileName)) {
                    throw new InvalidActionException($"Файл {file.FileName} уже существует");
                }
            }

            var result = new List<FileResponse>();
            foreach (var file in files) {
                var fileRecord = new Models.File {
                    user_id = Guid.Parse(userId),
                    name = Path.GetFileName(file.FileName),
                    extention = Path.GetExtension(file.FileName),
                    path = Path.GetDirectoryName(file.FileName),
                    bytes = file.Length,
                };
                await repository.CreateFileAsync(fileRecord);
                await uploader.StoreFileAsync(userId, file);
                result.Add(mapper.Map<FileResponse>(fileRecord));
            }

            return result;
        }

        public async Task<FileStream> GetFileStream(string userId, UriFileRequest request) {
            var file = await ValidateSharedAccessAsync(userId, request.id);
            return browser.GetFileStream(userId, file);
        }

        public async Task<SharedFileResponse> ShareFileAsync(string userId, ShareFileRequest request) {
            var file = await repository.GetFileByIdAsync(request.id);
            if (file == null) {
                throw new NotFoundException("Файл не найден");
            }

            if (file.user_id != Guid.Parse(userId)) {
                throw new AccessDeniedException("Доступ запрещен");
            }

            var existing = await repository.GetExistingShareAsync(request.user_id, request.id);
            if (existing != null) {
                throw new InvalidActionException("Этот пользователь уже имеет доступ к этому файлу");
            }

            var shared = await repository.ShareFileAsync(new SharedFile {
                file_id = Guid.Parse(request.id),
                user_id = Guid.Parse(request.user_id)
            });

            var response = mapper.Map<SharedFileResponse>(file);
            response.receiver_id = shared.user_id;
            return response;
        }

        public async Task RemoveShareAsync(string userId, ShareFileRequest request) {

        }

        public async Task RemoveFileAsync(string userId, UriFileRequest request) {
            var file = await repository.GetFileByIdAsync(request.id);
            if (file == null) {
                throw new NotFoundException("Файл не найден");
            }

            if (file.user_id != Guid.Parse(userId)) {
                throw new AccessDeniedException("Доступ запрещен");
            }

            await repository.RemoveFileAsync(file);
            string path = browser.GetFilePath(file);
            await uploader.RemoveFileAsync(userId, path);
        }

        private async Task<Models.File> ValidateSharedAccessAsync(string requesterId, string fileId) {
            var userId = Guid.Parse(requesterId);
            var file = await repository.GetFileByIdAsync(fileId);
            if (file == null) {
                throw new NotFoundException("Файл не найден");
            }

            var user = await userRepository.GetUserByIdAsync(requesterId);
            if (userRepository.HasPermissions(user!, RolesEnum.Admin)) return file;

            if (file.user_id == userId) return file;

            /*
             * TODO: установить значение status у объекта File
             * на Enum - FileAccessibilityEnum
             * и делать проверку на доступ по ссылке
            */

            var sharedFiles = await repository.GetWithSharedFilesAsync(requesterId);
            if (sharedFiles.FirstOrDefault(f =>
                f.file_id == file.id &&
                f.user_id == userId) == null) {
                throw new AccessDeniedException("Этот файл вам недоступен");
            }

            return file;
        }

        private async Task<List<SharedFileResponse>> PopulateSharedFileResponsesAsync(List<SharedFile> sharedFiles) {
            var fileIds = sharedFiles.Select(sf => sf.file_id.ToString()).ToList();

            var fileInfoTasks = fileIds.Select(id => repository.GetFileByIdAsync(id));
            var fileInfos = await Task.WhenAll(fileInfoTasks);

            return fileInfos.Zip(sharedFiles, (fileInfo, sharedFile) => {
                var response = mapper.Map<SharedFileResponse>(fileInfo);
                response.receiver_id = sharedFile.user_id;
                return response;
            }).ToList();
        }

        private bool ValidateFileName(string fileName) {
            if (string.IsNullOrWhiteSpace(fileName)) {
                return false;
            }

            if (Path.IsPathRooted(fileName) || fileName.Contains("..") || fileName.Contains("\\")) {
                return false;
            }

            var invalidChars = Path.GetInvalidFileNameChars().Except(['/']);
            if (fileName.Any(c => invalidChars.Contains(c))) {
                return false;
            }

            if (fileName.Length > 255) {
                return false;
            }
            return true;
        }
    }
}
