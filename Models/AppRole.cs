using System.ComponentModel.DataAnnotations;

namespace Automotive_Project.Models
{

    public class AppRole
    {

        public AppRole()
        {
            
        }

        public AppRole(string roleName)
        {
            Name = roleName;
            NormalizedName = roleName.ToUpper();
            CreatedAt = DateTime.UtcNow;
        }


        [Key]
        public int Id { get; set; }                
        public string Name { get; set; } = null!;  
        public string? NormalizedName { get; set; } 
        public string? Description { get; set; }   
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<UserAccount> Users { get; set; } = new List<UserAccount>();
    }
}
