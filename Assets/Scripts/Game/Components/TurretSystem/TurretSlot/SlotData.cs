using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Scripts.Game.UI.TurretPurchaseSystem;

namespace Scripts.Game.Components.TurretSystem.TurretSlot
{
    //[JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    public sealed class SlotData
    {
        [JsonProperty("slot_data")] public List<SlotInfo> SlotInfos = new();
    }
    //[JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    public sealed class SlotInfo
    {
        [JsonProperty("has_turret")] public bool HasTurret;
        [JsonProperty("id")] public int ID;
        [JsonProperty("turret_id")] public int TurretID;
    }
}