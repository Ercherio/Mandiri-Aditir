namespace MerchantService.ViewModels
{
    public class UserLoginVM
    {
        public string Password { get; set; }
        public string FullName { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int PasswordChange { get; set; }
        public int Status { get; set; }
        public DateOnly? FirstLoginAt { get; set; }

        public int AttemptLoginFailed { get; set; }
        public string Salt { get; set; }
        public string TokenWeb { get; set; }
        public DateOnly? LastLoginFailed { get; set; }
        public string[] Authorize { get; set; }

    }
}
