using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace cloud.DTO.Requests.Files {
    public class ShareFileRequest {
        [Required(ErrorMessage = "Поле id обязательно")]
        [FromRoute]
        public string id { get; set; }

        [Required(ErrorMessage = "Поле user_id обязательно")]
        [FromBody]
        public string user_id { get; set; }
    }
}
