using System.Collections.Generic;

/// This originates from the Entelect Challenge 2018 Repo
/// https://github.com/EntelectChallenge/2018-TowerDefence

namespace EC2018.Entities
{
    public class GameState
    {
        public List<Player> Players { get; set; }
        public CellStateContainer[][] GameMap { get; set; }
        public GameDetails GameDetails { get; set; }
		public List<List<HitList>> TeslaHitList {get; set; }
		public List<HitList> IroncurtainHitList { get; set; }
    }
}