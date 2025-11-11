namespace cloud.DTO.Responses.Auth {
    public class LoginResponse {
        public string token { get; set; }
        public Guid id { get; set; }
        public string phone { get; set; }
    }
}
