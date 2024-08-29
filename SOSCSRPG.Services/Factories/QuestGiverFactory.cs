using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SOSCSRPG.Models.Shared;
using SOSCSRPG.Models;

namespace SOSCSRPG.Services.Factories
{
    public static class QuestGiverFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\QuestGivers.xml";
        private static readonly List<QuestGiver> _questGivers = new List<QuestGiver>();
        static QuestGiverFactory()
        {
            if(File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                string rootImagePath = data.SelectSingleNode("/QuestGivers").AttributeAsString("RootImagePath");
                LoadQuestGiversFromNodes(data.SelectNodes("/QuestGivers/QuestGiver"), rootImagePath);
            }
        }
        private static void LoadQuestGiversFromNodes(XmlNodeList nodes, string rootImagePath)
        {
            foreach (XmlNode node in nodes)
            {
                QuestGiver questGiver =
                    new QuestGiver(node.AttributeAsInt("ID"),
                                   node.SelectSingleNode("./Name")?.InnerText ?? "",
                                   $".{rootImagePath}{node.AttributeAsString("ImageName")}");
                AddQuests(questGiver, node.SelectNodes("./Quests"));
                //foreach (XmlNode childNode in node.SelectNodes("./QuestGiver/Quests"))
                //{
                //    questGiver.AddQuest(QuestFactory.GetQuestByID(childNode.AttributeAsInt("ID")));
                //}
                _questGivers.Add(questGiver);
            }
        }
        private static void AddQuests(QuestGiver questGiver, XmlNodeList quest)
        {
            if (quest == null) { return; }
            foreach(XmlNode node in quest)
            {
                questGiver.QuestAvailableHere.
                    Add(QuestFactory.GetQuestByID(node.AttributeAsInt("ID")));
            }
        }

        public static QuestGiver GetQuestGiverByID(int id)
        {
            return _questGivers.FirstOrDefault(qg => qg.ID == id);
        }
    }
}
