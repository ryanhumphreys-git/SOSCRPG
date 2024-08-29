using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Models
{
    public class QuestStatus : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Quest PlayerQuest { get; }
        public bool IsCompleted { get; set; }

        public QuestStatus(Quest quest)
        {
            PlayerQuest = quest;
            IsCompleted = false;
        }
    }
}
