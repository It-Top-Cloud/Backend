namespace cloud.DTO.Responses.Files {
    public class SharedFileResponse {
        public Guid id { get; set; }
        public string name { get; set; }
        public string extention { get; set; }
        public string? path { get; set; }
        public Guid user_id { get; set; }
        public Guid receiver_id { get; set; }
    }
}
