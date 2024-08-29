using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSCSRPG.Models.Actions;
using System.Text.Json.Serialization;
namespace SOSCSRPG.Models
{
    public class GameItem
    {

        public enum ItemCategory
        {
            Miscellaneous,
            Weapon,
            Consumable
        }
        [JsonIgnore]
        public ItemCategory Category { get; }
        public int ItemTypeID { get; }
        [JsonIgnore]
        public string Name { get; }
        [JsonIgnore]
        public int Price { get; }
        [JsonIgnore]
        public bool IsUnique { get;}
        [JsonIgnore]
        public IAction Action { get; set; }

        public GameItem(ItemCategory category, int itemTypeID, string name, int price,
                        bool isUnique = false, IAction action = null)
        {
            Category = category;
            ItemTypeID = itemTypeID;
            Name = name;
            Price = price;
            IsUnique = isUnique; 
            Action = action;
        }

        public void PerformAction(LivingEntity actor, LivingEntity target)
        {
            Action?.Execute(actor, target);
        }

        public GameItem Clone()
        {
            return new GameItem(Category, ItemTypeID, Name, Price, IsUnique, Action);   
        }
    }
}
