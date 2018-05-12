using System.Collections.Generic;

namespace EC2018.Entities
{
    public class GameState
    {
        public List<Player> Players { get; set; }
        public CellStateContainer[][] GameMap { get; set; }
        public GameDetails GameDetails { get; set; }
    }
}