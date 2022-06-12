using NintendoGames.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesList.Entities
{
    public class RatingsEntity
    {
        public int Id { get; set; }
        public string MetacriticRating { get; set; }

        public int GameId { get; set; }
        public GamesEntity Games { get; set; }
    }
}
