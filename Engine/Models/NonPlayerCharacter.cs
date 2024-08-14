using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class NonPlayerCharacter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Quest> QuestAvailableHere { get; set; } = new List<Quest>();

        public NonPlayerCharacter(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
