using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Engine.Shared;
using Engine.Models;

namespace Engine.Factories
{
    public static class VendorFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Vendors.xml";
        private static readonly List<Vendor> _vendors = new List<Vendor>();
        static VendorFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));
                LoadVendorsFromNodes(data.SelectNodes("/Vendors/Vendor"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }
        private static void LoadVendorsFromNodes(XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                Vendor vendor =
                    new Vendor(node.AttributeAsInt("ID"),
                               node.SelectSingleNode("./Name")?.InnerText ?? "");
                foreach (XmlNode childNode in node.SelectNodes("./InventoryItems/Item"))
                {
                    int quantity = childNode.AttributeAsInt("Quantity");

                    for (int i = 0; i < quantity; i++)
                    {
                        vendor.AddItemToInventory(ItemFactory.CreateGameItem(childNode.AttributeAsInt("ID")));
                    }
                }
                _vendors.Add(vendor);
            }
        }
        public static Vendor GetVendorByID(int id)
        {
            return _vendors.FirstOrDefault(t => t.ID == id);
        }
    }
}