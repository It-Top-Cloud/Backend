using System.ComponentModel.DataAnnotations;

namespace cloud.DTO.Requests.Auth {
    public class UsernameLoginRequest {
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }
    }
}
