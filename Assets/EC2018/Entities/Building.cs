using EC2018.Enums;

// This originates from the Entelect Challenge 2018 Repo
// https://github.com/EntelectChallenge/2018-TowerDefence

namespace EC2018.Entities
{
    public class Building
    {
        public int Health { get; set; }
        public int ConstructionTimeLeft { get; set; }
        public int Price { get; set; }
        public int WeaponDamage { get; set; }
        public int WeaponSpeed { get; set; }
        public int WeaponCooldownTimeLeft { get; set; }
        public int WeaponCooldownPeriod { get; set; }
        public int DestroyScore { get; set; }
        public int EnergyGeneratedPerTurn { get; set; }
        public BuildingType BuildingType { get; set; }
		public PlayerType PlayerType { get; set; }
    }
}