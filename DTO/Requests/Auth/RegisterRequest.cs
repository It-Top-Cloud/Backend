using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using cloud.Config;

namespace cloud.DTO.Requests.Auth {
    public class RegisterRequest {
        [Required(ErrorMessage = "Поле fname обязательно")]
        public string fname { get; set; }

        public string? sname { get; set; }

        public string? lname { get; set; }

        [Required(ErrorMessage = "Поле phone обязательно")]
        [RegularExpression(RegExp.Phone, ErrorMessage = "Не верный формат номера, пример: 74999999999")]
        public string phone { get; set; }

        [Required(ErrorMessage = "Поле email обязательно")]
        [EmailAddress(ErrorMessage = "Ошибка валидации электронной почты")]
        public string email { get; set; }

        [Required(ErrorMessage = "Поле password обязательно")]
        public string password { get; set; }
    }
}
