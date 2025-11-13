using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cloud.Models {
    public class File {
        [Key]
        [Column(TypeName = "uniqueidentifier")]
        public Guid id { get; set; } = Guid.NewGuid();


        [Column(TypeName = "uniqueidentifier")]
        public Guid user_id { get; set; }


        //[Column(TypeName = "json")]
        //public string info { get; set; }


        [Column(TypeName = "nvarchar(255)")]
        public string name { get; set; }


        [Column(TypeName = "nvarchar(50)")]
        public string? extension { get; set; }


        [Column(TypeName = "nvarchar(255)")]
        public string? path { get; set; }


        [Column(TypeName = "bigint")]
        public long bytes { get; set; }


        [Column(TypeName = "int")]
        public int status { get; set; } = 1;


        [Column(TypeName = "datetime")]
        public DateTime? binned_at { get; set; }


        [Column(TypeName = "datetime")]
        public DateTime сreated_at { get; set; } = DateTime.UtcNow;


        [Column(TypeName = "datetime")]
        public DateTime updated_at { get; set; } = DateTime.UtcNow;
    }
}
