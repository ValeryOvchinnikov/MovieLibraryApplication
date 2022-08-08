namespace MovieLibrary.WebApp.Models
{
    public class RatingDTO
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string UserNickname { get; set; }
        public int Value { get; set; }
    }
}
