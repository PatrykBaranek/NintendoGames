using NintendoGames.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesList.Entities
{
    public class PricesEntity
    {
        public int Id { get; set; }
        public string Price { get; set; }


        public int GameId { get; set; }
        public GamesEntity Games { get; set; }

    }
}
