using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Weapon : GameItem
    {
        public int MinimumDamage { get; }
        public int MaximumDamage { get; }

        public Weapon(int itemTypeID, string name, int price, int minDamage, int maxDamage)
            : base(itemTypeID, name, price, true)
        {
            MinimumDamage = minDamage;
            MaximumDamage = maxDamage;
        }

        public new Weapon Clone()
        {
            return new Weapon(ItemTypeID, Name, Price, MinimumDamage, MaximumDamage);
        }
    }
}
