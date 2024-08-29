using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Models
{
    public class Race
    {
        public string Key { get; set; }
        public string DisplayName { get; set; }
        public List<PlayerAttributeModifier> PlayerAttributeModifiers { get; } = new List<PlayerAttributeModifier>();
    }
}
