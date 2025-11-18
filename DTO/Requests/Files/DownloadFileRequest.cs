using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace cloud.DTO.Requests.Files {
    public class DownloadFileRequest {
        [Required(ErrorMessage = "Поле id обязательно")]
        [FromRoute]
        public string id { get; set; }
    }
}
