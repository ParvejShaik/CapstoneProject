using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactAgentMicroservice.Models
{
    public class ContactAgent
    {
        
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string PropertyName { get; set; }

        [Required]
        public string Locality { get; set; }

        public AgentDetails Agent { get; set; }
    }
    [ComplexType]
    public class AgentDetails
    {

        [Required]
        public string Name { get; set; }
       

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Contact { get; set; }
    }


}
