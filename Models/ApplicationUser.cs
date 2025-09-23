namespace Automotive_Project.Models
{
    public class ApplicationUser
    {
        public ApplicationUser()
        {
            Orders = new HashSet<Order>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = "";
        public DateTime CreatedAt { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
