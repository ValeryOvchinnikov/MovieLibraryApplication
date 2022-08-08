namespace MovieLibrary.BusinessLogic.Models
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public int MovieId { get; set; }
        public string UserNickname { get; set; }
        public string Text { get; set; }
    }
}
