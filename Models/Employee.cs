using System.ComponentModel.DataAnnotations;

namespace CodingAB.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public virtual ICollection<TimeOffRequest> TimeOffRequests { get; set; }

        // Constructor to initialize the TimeOffRequests collection
        public Employee()
        {
            TimeOffRequests = new List<TimeOffRequest>();
        }
    }
}
