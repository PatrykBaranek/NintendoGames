using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NintendoGames.Models
{
    public class GameDto
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public List<string> Companies { get; set; }
    }
}
