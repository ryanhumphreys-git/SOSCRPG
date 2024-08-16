using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    public static class ItemFactory
    {
        private static readonly List<GameItem> _standardGameItems = new List<GameItem>();

        static ItemFactory()
        {
            _standardGameItems.Add(new Weapon(1001, "Pointy Stick", 1, 1, 2));
            _standardGameItems.Add(new Weapon(1002, "Rusty Sword", 5, 2, 4));
            _standardGameItems.Add(new Weapon(1003, "Club", 10, 3, 6));
            _standardGameItems.Add(new Weapon(1004, "Bird Talon Dagger", 20, 6, 10));
            _standardGameItems.Add(new Weapon(1005, "Sword of The Spider Queen", 30, 12, 20));
            _standardGameItems.Add(new Weapon(1006, "Claw of The First Wolf", 50, 25, 50));

            _standardGameItems.Add(new GameItem(9001, "Snake fang", 1));
            _standardGameItems.Add(new GameItem(9002, "Snake skin", 2));
            _standardGameItems.Add(new GameItem(9003, "Rat tail", 1));
            _standardGameItems.Add(new GameItem(9004, "Piece of fur", 1));
            _standardGameItems.Add(new GameItem(9005, "Spider fang", 1));
            _standardGameItems.Add(new GameItem(9006, "Spider silk", 1));
            _standardGameItems.Add(new GameItem(9007, "Adventurer pass", 5));
            _standardGameItems.Add(new GameItem(9008, "Fang of the Alpha Wolf", 5));
            _standardGameItems.Add(new GameItem(9009, "Fang of the Spider Queen", 5));
            _standardGameItems.Add(new GameItem(9010, "Elemental Dust", 3));
            _standardGameItems.Add(new GameItem(9011, "Corrupted Fur", 3));
            _standardGameItems.Add(new GameItem(9012, "Corrupted Meat", 3));
            _standardGameItems.Add(new GameItem(9013, "Leftover Rock", 3));
            _standardGameItems.Add(new GameItem(9014, "Leftover Water", 3));
            _standardGameItems.Add(new GameItem(9015, "Dryad's Blessing", 5));
            _standardGameItems.Add(new GameItem(9016, "Feather", 5));
        }

        public static GameItem CreateGameItem(int itemTypeID)
        {
            GameItem standardItem = _standardGameItems.FirstOrDefault(item => item.ItemTypeID == itemTypeID);

            if (standardItem != null)
            {
                if (standardItem is Weapon)
                {
                    return (standardItem as Weapon).Clone();
                }
                return standardItem.Clone();
            }

            return null;
        }

        public static string GetGameItemName(int itemTypeID)
        {
            GameItem standardItem = _standardGameItems.FirstOrDefault(item => item.ItemTypeID == itemTypeID);

            return standardItem.Name;
        }
    }
}
