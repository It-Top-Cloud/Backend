using System.ComponentModel.DataAnnotations;
using cloud.Config;

namespace cloud.DTO.Requests.Auth.Verify {
    public class PhoneVerificationRequest {
        [Required(ErrorMessage = "Поле phone обязательно")]
        [RegularExpression(Constants.Phone, ErrorMessage = "Не верный формат номера, пример: 74999999999")]
        public string phone { get; set; }
    }
}
