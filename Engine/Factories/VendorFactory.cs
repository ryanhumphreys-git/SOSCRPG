using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    public static class VendorFactory
    {
        private static readonly List<Vendor> _vendors = new List<Vendor>();

        static VendorFactory()
        {
            Vendor susVendor = new Vendor("Suspicious Trader");
            susVendor.AddItemToInventory(ItemFactory.CreateGameItem(1001));

            Vendor birdHermit = new Vendor("Bird Hermit");
            birdHermit.AddItemToInventory(ItemFactory.CreateGameItem(1004));

            AddVendorToList(susVendor);
            AddVendorToList(birdHermit);

        }

        public static Vendor GetVendorByName(string name)
        {
            return _vendors.FirstOrDefault(t => t.Name == name);
        }

        private static void AddVendorToList(Vendor vendor)
        {
            if(_vendors.Any(t => t.Name == vendor.Name))
            {
                throw new ArgumentException($"There is already a vendor named {vendor.Name}");
            }

            _vendors.Add(vendor);
        }
    }
}
