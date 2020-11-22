using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroDiffusion.Entities
{
    class Town
    {
        public Town(Point coord, int[] coins)
        {
            Coord = coord;
            Coins = coins;
        }

        public int[] Coins { get; set; }
        public Point Coord { get; set; }
    }
}
