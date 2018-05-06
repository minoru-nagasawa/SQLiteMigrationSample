using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQLiteMigrationSample.Models
{
    [Table("Sample")]
    public class SamplePoco
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public SamplePoco()
        {
        }

        public SamplePoco(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
