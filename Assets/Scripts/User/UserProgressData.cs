using System.Collections.Generic;
using Newtonsoft.Json;
using Scripts.Game.Components.TurretSystem.TurretSlot;
using Scripts.Game.UI.TurretPurchaseSystem;

namespace Scripts.User
{
    public class UserProgressData
    {
        [JsonProperty("has_value")] public bool HasValue;
        [JsonProperty("coin_amount")] public int CoinAmount;
        [JsonProperty("score")] public int Score;
        [JsonProperty("enemy_difficulty")] public int EnemyDifficulty;
        [JsonProperty("spawn_interval")] public float SpawnInterval;
        [JsonProperty("purchase_progress_data")] public PurchaseProgressData PurchaseProgressData = new();
        [JsonProperty("slots_data")] public SlotData SlotData = new();
    }
}