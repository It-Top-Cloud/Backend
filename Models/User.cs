using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using cloud.Enums;

namespace cloud.Models {
    public class User {
        [Key]
        [Column(TypeName = "uniqueidentifier")]
        public Guid id { get; set; }


        [Column(TypeName = "nvarchar(16)")]
        public string username { get; set; }


        [Column(TypeName = "nvarchar(255)")]
        public string email { get; set; }


        [Column(TypeName = "nvarchar(255)")]
        public string password { get; set; }


        [Column(TypeName = "int")]
        public int role { get; set; } = (int)RolesEnum.User;


        [Column(TypeName = "datetime")]
        public DateTime сreated_at { get; set; } = DateTime.UtcNow;


        [Column(TypeName = "datetime")]
        public DateTime updated_at { get; set; } = DateTime.UtcNow;
    }
}
