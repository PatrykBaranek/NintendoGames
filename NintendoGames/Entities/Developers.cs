namespace NintendoGames.Entities
{
    public class Developers
    {
        public Guid Id{ get; set; }
        public string Name { get; set; }

        public Guid GameId { get; set; }
        public Game Game { get; set; }

    }
}
