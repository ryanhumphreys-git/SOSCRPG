using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    public static class QuestGiverFactory
    {
        private static readonly List<QuestGiver> _questGivers = new List<QuestGiver>();


        static QuestGiverFactory()
        {
            QuestGiver alchemist = new QuestGiver("Alchemist");
            alchemist.AddQuest(QuestFactory.GetQuestByID(1));

            QuestGiver farmer = new QuestGiver("Farmer");

            QuestGiver guardAtPost = new QuestGiver("Guard");

            QuestGiver dryad = new QuestGiver("Dryad");

            QuestGiver birdGod = new QuestGiver("Bird God");

            AddQuestGiver(alchemist);
            AddQuestGiver(farmer);
            AddQuestGiver(guardAtPost);
            AddQuestGiver(dryad);
            AddQuestGiver(birdGod);
        }

        public static QuestGiver GetQuestGiverByName(string name)
        {
            return _questGivers.FirstOrDefault(qg => qg.Name == name);
        }

        private static void AddQuestGiver(QuestGiver questGiver)
        {
            if (_questGivers.Any(qg => qg.Name == questGiver.Name))
            {
                throw new ArgumentException($"There is already a vendor named {questGiver.Name}");
            }

            _questGivers.Add(questGiver);
        }

    }
}
