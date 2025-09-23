using System.ComponentModel.DataAnnotations;

namespace Automotive_Project.Models
{
    public class AppRole
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<UserAccount> Users { get; set; } = new();
    }
}
