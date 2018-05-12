using System.Collections.Generic;
using EC2018.Enums;

namespace EC2018.Entities
{
    public class GameDetails
    {
        public int Round { get; set; }
        public int MapWidth  { get; set; }
        public int MapHeight  { get; set; }
        public Dictionary<BuildingType, int> BuildingPrices  { get; set; }  
    }
}