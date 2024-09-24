using System.ComponentModel.DataAnnotations;

namespace AgentMicroservice.Models
{
    public class Agent
    {
        [Key]
       
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Mail { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Locality { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}
