namespace GameManagerAPI.Models
{
    public class Developer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public List<int> AssignedProjectIds { get; set; } = new();
    }
}
