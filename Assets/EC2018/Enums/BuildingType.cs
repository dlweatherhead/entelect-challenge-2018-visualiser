using System.Runtime.Serialization;
using Newtonsoft.Json;

/// This originates from the Entelect Challenge 2018 Repo
/// https://github.com/EntelectChallenge/2018-TowerDefence

namespace EC2018.Enums
{
    public enum BuildingType
    {
        [JsonProperty("DEFENSE")]
        [EnumMember(Value = "DEFENSE")]
        Defense = 0,
        [JsonProperty("ATTACK")]
        [EnumMember(Value = "ATTACK")]
        Attack = 1,
        [JsonProperty("ENERGY")]
        [EnumMember(Value = "ENERGY")]
        Energy = 2,
		[JsonProperty("TESLA")]
		[EnumMember(Value = "TESLA")]
		Tesla = 3
    }
}