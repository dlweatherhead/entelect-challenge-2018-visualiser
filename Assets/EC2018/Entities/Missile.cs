
/// This originates from the Entelect Challenge 2018 Repo
/// https://github.com/EntelectChallenge/2018-TowerDefence
using EC2018.Enums;

namespace EC2018.Entities
{
    public class Missile
    {
        public int Damage { get; set; }
        public int Speed { get; set; }
		public PlayerType PlayerType { get; set; }
    }
}