namespace NintendoGames.Models.UserRequestModels
{
    public class UserRequestDto
    {
        public string UserEmail { get; set; }
        public string UserRole { get; set; }
        public string Request { get; set; }
        public string RequestUrl { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
