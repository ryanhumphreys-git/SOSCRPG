using System.Text.Json.Serialization;

namespace SOSCSRPG.Models
{
    public class Quest
    {
        #region Properties
        public int ID { get; }
        [JsonIgnore]
        public string Name { get; }
        [JsonIgnore]
        public string Description { get; }
        [JsonIgnore]
        public List<ItemQuantity> ItemsToComplete { get; }
        [JsonIgnore]
        public int RewardExperiencePoints   { get; }
        [JsonIgnore]
        public int RewardGold {  get; }
        [JsonIgnore]
        public List<ItemQuantity> RewardItems { get; }
        #endregion
        [JsonIgnore]
        public string ToolTipContents =>
            Description + Environment.NewLine + Environment.NewLine +
            "Items to complete the quest" + Environment.NewLine +
            "===========================" + Environment.NewLine +
            string.Join(Environment.NewLine, ItemsToComplete.Select(i => i.QuantityItemDescription)) +
            Environment.NewLine + Environment.NewLine +
            "Rewards\r\n" +
            "===========================" + Environment.NewLine +
            $"{RewardExperiencePoints} experience " + Environment.NewLine +
            $"{RewardGold} gold" + Environment.NewLine +
            string.Join(Environment.NewLine, RewardItems.Select(i => i.QuantityItemDescription));

        public Quest(int id, string name, string description, List<ItemQuantity> itemsToComplete,
                     int rewardExperiencePoints, int rewardGold, List<ItemQuantity> rewardItems)
        {
            ID = id;
            Name = name;
            Description = description;
            ItemsToComplete = itemsToComplete;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
            RewardItems = rewardItems;
        }
    }
}
