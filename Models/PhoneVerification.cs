using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using cloud.Enums;

namespace cloud.Models {
    public class PhoneVerification {
        [Key]
        [Column(TypeName = "uniqueidentifier")]
        public Guid id { get; set; } = Guid.NewGuid();


        [Column(TypeName = "varchar(20)")]
        public string check_id { get; set; }


        [Column(TypeName = "varchar(11)")]
        public string phone { get; set; }


        [Column(TypeName = "int")]
        public int status { get; set; } = (int)VerificationEnum.Pending;


        [Column(TypeName = "datetime")]
        public DateTime сreated_at { get; set; } = DateTime.UtcNow;


        [Column(TypeName = "datetime")]
        public DateTime updated_at { get; set; } = DateTime.UtcNow;
    }
}
