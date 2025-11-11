using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using cloud.Enums;

namespace cloud.Models {
    public class User {
        [Key]
        [Column(TypeName = "uniqueidentifier")]
        public Guid id { get; set; } = Guid.NewGuid();


        [Column(TypeName = "nvarchar(30)")]
        public string fname { get; set; }


        [Column(TypeName = "nvarchar(30)")]
        public string? sname { get; set; }


        [Column(TypeName = "nvarchar(30)")]
        public string? lname { get; set; }


        [Column(TypeName = "varchar(11)")]
        public string phone { get; set; }


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
