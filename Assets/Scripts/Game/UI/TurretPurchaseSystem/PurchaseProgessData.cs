using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Scripts.Game.UI.TurretPurchaseSystem
{
    //[JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    public sealed class PurchaseProgressData
    {
        [JsonProperty("purchase_progresses")] public List<PurchaseProgress> PurchaseProgresses = new();
    }
    //[JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    public sealed class PurchaseProgress
    {
        [JsonProperty("id")] public int ID;
        [JsonProperty("purchase_step")] public int PurchaseStep;
    }
}