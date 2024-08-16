﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    internal static class QuestFactory
    {
        private static readonly List<Quest> _quests = new List<Quest>();
        
        static QuestFactory()
        {
            List<ItemQuantity> itemsToComplete = new List<ItemQuantity>();
            List<ItemQuantity> rewardItems = new List<ItemQuantity>();

            itemsToComplete.Add(new ItemQuantity(9003, 5));
            rewardItems.Add(new ItemQuantity(1002, 1));

            _quests.Add(new Quest(1,
                                  "Clear the Alchemist's garden",
                                  "Defeat the rats in the Alchemist's garden. Bring back 5 rat tails as proof of your deed.",
                                  itemsToComplete,
                                  25, 10,
                                  rewardItems));

        }

        internal static Quest GetQuestByID(int id)
        {
            return _quests.FirstOrDefault(quest => quest.ID == id);
        }
    }
}
