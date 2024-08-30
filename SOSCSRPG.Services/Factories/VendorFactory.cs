using System.Xml;
using SOSCSRPG.Models;
using SOSCSRPG.Models.Shared;

namespace SOSCSRPG.Services.Factories
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

                string rootImagePath = data.SelectSingleNode("/Vendors").AttributeAsString("RootImagePath");
                LoadVendorsFromNodes(data.SelectNodes("/Vendors/Vendor"), rootImagePath);
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }
        private static void LoadVendorsFromNodes(XmlNodeList nodes, string rootImagePath)
        {
            foreach (XmlNode node in nodes)
            {
                Vendor vendor =
                    new Vendor(node.AttributeAsInt("ID"),
                               node.SelectSingleNode("./Name")?.InnerText ?? "",
                               $".{rootImagePath}{node.AttributeAsString("ImageName")}");
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