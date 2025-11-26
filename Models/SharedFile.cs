using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cloud.Models {
    public class SharedFile {
        [Key]
        [Column(TypeName = "uniqueidentifier")]
        public Guid id { get; set; } = Guid.NewGuid();


        [Column(TypeName = "uniqueidentifier")]
        public Guid file_id { get; set; }


        [Column(TypeName = "uniqueidentifier")]
        public Guid user_id { get; set; }


        [Column(TypeName = "datetime")]
        public DateTime сreated_at { get; set; } = DateTime.UtcNow;


        [Column(TypeName = "datetime")]
        public DateTime updated_at { get; set; } = DateTime.UtcNow;
    }
}
