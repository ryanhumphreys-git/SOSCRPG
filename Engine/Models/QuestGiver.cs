using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class QuestGiver : BaseNotificationClass
    {

        public string Name { get; set; }
        public string ImageName { get; set; }
        public List<Quest> QuestAvailableHere { get; set; } = new List<Quest>();

        public QuestGiver(string name)
        {
            Name = name;
        }

        public void AddQuest(Quest quest)
        {
            QuestAvailableHere.Add(quest);
        }
    }
}
