using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Vendor : LivingEntity
    {
        public int ID { get; }
        public Vendor(int id, string name) : base(name, 9999, 9999, 9999)
        {
            ID = id;
        }
    }
}
