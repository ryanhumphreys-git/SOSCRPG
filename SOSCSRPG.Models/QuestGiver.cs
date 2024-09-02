using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SOSCSRPG.Models
{
    public class QuestGiver : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get; }
        public int ID { get; }
        public string ImageName { get; }
        public List<Quest> QuestAvailableHere { get; set; } = new List<Quest>();
        public List<Quest> QuestUnavailableHere { get; set; } = new List<Quest>();

        public QuestGiver(int id, string name, string imageName)
        {
            ID = id;
            Name = name;
            ImageName = imageName;
        }
    }
}
