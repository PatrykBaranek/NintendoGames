namespace NintendoGames.Entities
{
    public class UserRequest
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public string UserRole { get; set; }
        public string Request { get; set; }
        public string RequestUrl { get; set; }
        public DateTime RequestDate { get; set; }

    }
}
