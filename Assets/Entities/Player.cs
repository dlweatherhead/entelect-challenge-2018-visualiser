using EC2018.Enums;

namespace EC2018.Entities
{
    public class Player
    {
        public PlayerType PlayerType { get; set; }
        public int Energy { get; set; }
        public int Health { get; set; }
        public int HitsTaken { get; set; }
        public int Score { get; set; }
    }
}