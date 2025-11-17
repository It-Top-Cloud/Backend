using AutoMapper;
using cloud.DTO.Requests.Files;
using cloud.DTO.Responses.Files;
using cloud.Exceptions;
using cloud.Repositories.Files;
using cloud.Services.Files.FileWorkers.Browser;
using cloud.Services.Files.FileWorkers.Uploader;

namespace cloud.Services.Files {
    public class FileService : IFileService {
        private readonly IFileRepository repository;
        private readonly IFileUploaderService uploader;
        private readonly IFileBrowserService browser;
        private readonly IMapper mapper;

        public FileService(IFileRepository repository, IFileUploaderService uploader, IFileBrowserService browser, IMapper mapper) {
            this.repository = repository;
            this.uploader = uploader;
            this.browser = browser;
            this.mapper = mapper;
        }

        public async Task<List<FileResponse>> GetUserFilesAsync(string userId) {
            var files = await repository.GetUserFilesAsync(userId);
            return mapper.Map<List<FileResponse>>(files);
        }

        public async Task<List<FileResponse>> UploadFilesAsync(string userId, IFormFileCollection files) {
            if (repository.HasStorageLimit(userId, out long available)) {
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
                    extension = Path.GetExtension(file.FileName),
                    path = Path.GetDirectoryName(file.FileName),
                    bytes = file.Length,
                };
                await repository.CreateFileAsync(fileRecord);
                await uploader.StoreFileAsync(userId, file);
                result.Add(mapper.Map<FileResponse>(fileRecord));
            }

            return result;
        }

        public async Task<FileStream> GetFileStream(string userId, DownloadFileRequest request) {
            var file = await repository.GetFileByIdAsync(request.id);
            if (file == null) {
                throw new NotFoundException("Файл не найден");
            }

            if (file.user_id != Guid.Parse(userId)) {
                throw new AccessDeniedException("Доступ запрещен");
            }
            /*
             * изменить проверку в будущем
             * у файлов будет возможность их разшаривать
             * или сделать файл публичным
             * 
             * TODO: установить значение status у объекта File
             * на Enum - FileAccessibilityEnum
            */

            return browser.GetFileStream(userId, file);
        }

        public async Task RemoveFileAsync(string userId, RemoveFileRequest request) {
            var file = await repository.GetFileByIdAsync(request.id);
            if (file == null) {
                throw new NotFoundException("Файл не найден");
            }

            if (file.user_id != Guid.Parse(userId)) {
                throw new AccessDeniedException("Доступ запрещен");
            }

            await repository.RemoveFileAsync(file);
            string path = string.IsNullOrWhiteSpace(file.path) ? file.name : Path.Combine(file.path, file.name);
            await uploader.RemoveFileAsync(userId, path);
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
