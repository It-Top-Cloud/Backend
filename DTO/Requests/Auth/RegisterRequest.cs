using System.ComponentModel.DataAnnotations;

namespace cloud.DTO.Requests.Auth {
    public class RegisterRequest {
        [Required(ErrorMessage = "Поле username обязательно")]
        [StringLength(16, MinimumLength = 3, ErrorMessage = "Имя пользователя должно быть от 3 до 16 символов")]
        public string username { get; set; }

        [Required(ErrorMessage = "Поле password обязательно")]
        public string password { get; set; }

        [Required(ErrorMessage = "Поле email обязательно")]
        [EmailAddress(ErrorMessage = "Ошибка валидации электронной почты")]
        public string email { get; set; }
    }
}
