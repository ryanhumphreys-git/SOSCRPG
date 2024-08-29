using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SOSCSRPG.Models;
using SOSCSRPG.Models.Shared;

namespace SOSCSRPG.Services.Factories
{
    public static class WorldFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Locations.xml";

        public static World CreateWorld()
        {
            World world = new World();

            if(File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                string rootImagePath =
                    data.SelectSingleNode("/Locations").AttributeAsString("RootImagePath");

                LoadLocationsFromNodes(world,
                                       rootImagePath,
                                       data.SelectNodes("/Locations/Location"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
            return world;
        }
        private static void LoadLocationsFromNodes(World world, string rootImagePath, XmlNodeList nodes)
        {
            if(nodes == null)
            {
                return;
            }
            foreach(XmlNode node in nodes)
            {
                Location location =
                    new Location(node.AttributeAsInt("X"),
                                 node.AttributeAsInt("Y"),
                                 node.AttributeAsString("Name"),
                                 node.SelectSingleNode("./Description")?.InnerText ?? "",
                                 $".{rootImagePath}{node.AttributeAsString("ImageName")}");
                AddMonsters(location, node.SelectNodes("./Monsters/Monster"));
                AddQuestGiver(location, node.SelectSingleNode("./QuestGiver"));
                AddVendor(location, node.SelectSingleNode("./Vendor"));
                world.AddLocation(location);
            }
        }
        private static void AddMonsters(Location location, XmlNodeList monsters)
        {
            if (monsters == null) { return; }
            foreach (XmlNode monsterNode in monsters)
            {
                location.AddMonster(monsterNode.AttributeAsInt("ID"),
                                    monsterNode.AttributeAsInt("Percent"));
            }
        }
        private static void AddQuestGiver(Location location, XmlNode questGiver)
        {
            if (questGiver == null) { return; }

            location.QuestGiverHere =
                QuestGiverFactory.GetQuestGiverByID(questGiver.AttributeAsInt("ID"));
            
        }
        private static void AddVendor(Location location, XmlNode trader)
        {
            if (trader == null) { return; }

            location.VendorHere =
                VendorFactory.GetVendorByID(trader.AttributeAsInt("ID"));
        }
    }
}
