namespace HungApple.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int Rating { get; set; }
        public DateTime DateCreated { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string UserName { get; set; }
    }
}
