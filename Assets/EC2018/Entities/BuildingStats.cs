using System;
namespace EC2018.Entities 
{
    public class BuildingStats 
    {
        public int Health { get; set; }
        public int ConstructionTime { get; set; }
        public int Price { get; set; }
        public int WeaponDamage { get; set; }
        public int WeaponSpeed { get; set; }
        public int WeaponCooldownPeriod { get; set; }
        public int EnergyGeneratedPerTurn { get; set; }
        public int DestroyMultiplier { get; set; }
        public int ConstructionScore { get; set; }
    }
}
