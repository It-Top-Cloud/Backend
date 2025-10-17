using System.ComponentModel.DataAnnotations;

namespace cloud.DTO.Requests.Auth {
    public class PhoneLoginRequest {
        [Required(ErrorMessage = "Поле phone обязательно")]
        [RegularExpression(@"^7\d{10}$", ErrorMessage = "Не верный формат номера, пример: 74999999999")]
        public string phone { get; set; }

        [Required(ErrorMessage = "Поле password обязательно")]
        public string password { get; set; }
    }
}
