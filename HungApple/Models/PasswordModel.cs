namespace HungApple.Models
{
    public class PasswordModel
    {
        public int Id { get; set; }
        public string PasswordOld { get; set; }
        public string PasswordNew { get; set; }
        public string ConfirmPassword { get; set; }

    }
}
