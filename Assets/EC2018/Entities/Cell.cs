using Newtonsoft.Json;
using EC2018.Enums;

// This originates from the Entelect Challenge 2018 Repo
// https://github.com/EntelectChallenge/2018-TowerDefence

namespace EC2018.Entities
{
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public PlayerType PlayerType { get; set; }
    }
}