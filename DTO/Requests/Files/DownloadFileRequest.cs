using System.ComponentModel.DataAnnotations;

namespace cloud.DTO.Requests.Files {
    public class DownloadFileRequest {
        [Required(ErrorMessage = "Поле id обязательно")]
        public string id { get; set; }
    }
}
