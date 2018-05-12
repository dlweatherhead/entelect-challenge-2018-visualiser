﻿using System.Runtime.Serialization;
using Newtonsoft.Json;

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
        Energy = 2
    }
}