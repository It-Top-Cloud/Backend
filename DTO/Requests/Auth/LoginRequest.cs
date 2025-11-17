using System.ComponentModel.DataAnnotations;
using cloud.Config;

namespace cloud.DTO.Requests.Auth {
    public class LoginRequest {
        [Required(ErrorMessage = "Поле phone обязательно")]
        [RegularExpression(Constants.Phone, ErrorMessage = "Не верный формат номера, пример: 74999999999")]
        public string phone { get; set; }

        [Required(ErrorMessage = "Поле password обязательно")]
        public string password { get; set; }
    }
}
