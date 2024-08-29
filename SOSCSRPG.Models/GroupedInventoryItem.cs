using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Models
{
    public class GroupedInventoryItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public GameItem Item { get; set; }
        public int Quantity { get; set; }
        public GroupedInventoryItem(GameItem item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }
}
