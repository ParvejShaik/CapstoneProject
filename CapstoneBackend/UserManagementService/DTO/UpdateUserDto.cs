using System.ComponentModel.DataAnnotations;

namespace UserManagementService.DTO
{
    public class UpdateUserDto
    {
        public string UserName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
