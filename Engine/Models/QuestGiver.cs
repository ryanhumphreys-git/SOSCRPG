using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class QuestGiver : BaseNotificationClass
    {

        public string Name { get; }
        public int ID { get; }
        public string ImageName { get; }
        public List<Quest> QuestAvailableHere { get; set; } = new List<Quest>();

        public QuestGiver(int id, string name, string imageName)
        {
            ID = id;
            Name = name;
            ImageName = imageName;
        }

        public void AddQuest(Quest quest)
        {
            QuestAvailableHere.Add(quest);
        }
    }
}
