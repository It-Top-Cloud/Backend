using System.ComponentModel.DataAnnotations;

namespace cloud.DTO.Requests.Auth.Verify {
    public class PhoneVerificationRequest {
        [Required(ErrorMessage = "Поле phone обязательно")]
        [RegularExpression(@"^7\d{10}$", ErrorMessage = "Не верный формат номера, пример: 74999999999")]
        public string phone { get; set; }
    }
}
