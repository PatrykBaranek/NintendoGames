using GamesList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NintendoGames.Models
{
    public class GameDto
    {
        public string GameTitle { get; set; }
        public string ImageUrl { get; set; }
        public RatingDto Ratings { get; set; }
    }
}
