using System.Collections.Generic;
using EC2018.Enums;

/// This originates from the Entelect Challenge 2018 Repo
/// https://github.com/EntelectChallenge/2018-TowerDefence

namespace EC2018.Entities
{
    public class GameDetails
    {
        public int Round { get; set; }
        public int MaxRounds { get; set; }
        public int MapWidth  { get; set; }
        public int MapHeight  { get; set; }
        public int RoundIncomeEnergy { get; set; }
        public Dictionary<BuildingType, int> BuildingPrices  { get; set; }
        // TODO - Building Stats
        // TODO - Iron Curtain Stats
    }
}