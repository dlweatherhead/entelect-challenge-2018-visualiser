using Newtonsoft.Json;
using EC2018.Enums;

namespace EC2018.Entities
{
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public PlayerType PlayerType { get; set; }
    }
}