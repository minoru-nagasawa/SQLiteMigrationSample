using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQLiteMigrationSample.Models
{
    [Table("Test")]
    public class TestPoco
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public TestPoco()
        {
        }

        public TestPoco(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
