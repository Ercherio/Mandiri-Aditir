using System.ComponentModel.DataAnnotations;

namespace MerchantService.ViewModels
{
    public class RegisterVM
    {


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;

        public string Fullname { get; set; } = null!;

        [Required(ErrorMessage = "Mobile Phone is required")]
        [StringLength(15, MinimumLength = 11, ErrorMessage = "Mobile Phone must be between 11 and 15 characters")]
        public string MobilePhone { get; set; } = null!;

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = null!;

        [Required(ErrorMessage = "Merchant ID is required")]
        public string mid { get; set; } = null!;

        [Required(ErrorMessage = "Merchant Name is required")]
        public string NamaMerchant { get; set; } = null!;


    }
}
