namespace GameManagerAPI.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Engine { get; set; } 
        public DateTime ReleaseDate { get; set; }
    }
}
