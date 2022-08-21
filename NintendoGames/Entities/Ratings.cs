namespace NintendoGames.Entities;

public class Ratings
{
    public Guid Id { get; set; }
    public int CriticRating { get; set; }
    public double UserScore { get; set; }
    public bool IsMustPlay { get; set; }

    public Guid GameId { get; set; }
    public Games Games { get; set; }
}