using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSCSRPG.Services.Factories;
using Engine.Models;
using Engine.Services;
using NUnit.Framework;

namespace TestEngine.Models
{
    public class TestInventory
    {
       //[Test]
       // public void Test_Instantiate()
       // {
       //     Inventory inventory = new Inventory();

       //     Assert.That(inventory.Items.Count, Is.EqualTo(0));
       // }

       // [Test]
       // public void Test_AddItem()
       // {
       //     Inventory inventory = new Inventory();
       //     Inventory inventory1 = inventory.AddItemFromFactory(3001);
       //     Assert.That(inventory1.Items.Count, Is.EqualTo(1));
       // }
       // [Test]
       // public void Test_AddItems()
       // {
       //     Inventory inventory = new Inventory();
       //     List<GameItem> itemsToAdd = new List<GameItem>();
       //     itemsToAdd.Add(ItemFactory.CreateGameItem(3001));
       //     itemsToAdd.Add(ItemFactory.CreateGameItem(1001));
       //     Inventory inventory1 = inventory.AddItems(itemsToAdd);
       //     Assert.That(inventory1.Items.Count, Is.EqualTo(2));

       //     Inventory inventory2 = inventory1.AddItemFromFactory(3001).AddItemFromFactory(1001);
       //     Assert.That(inventory2.Items.Count, Is.EqualTo(4));
       // }
       // [Test]
       // public void Test_AddItemQuantities()
       // {
       //     Inventory inventory = new Inventory();
       //     Inventory inventory1 = inventory.AddItems(new List<ItemQuantity> { new ItemQuantity(1001, 3) });
       //     Assert.That(inventory1.Items.Count(i => i.ItemTypeID == 1001), Is.EqualTo(3));

       //     Inventory inventory2 = inventory1.AddItemFromFactory(1001);
       //     Assert.That(inventory2.Items.Count(i => i.ItemTypeID == 1001), Is.EqualTo(4));

       //     Inventory inventory3 = inventory2.AddItems(new List<ItemQuantity> { new ItemQuantity(1002, 1) });
       //     Assert.That(inventory3.Items.Count(i => i.ItemTypeID == 1001), Is.EqualTo(4));
       //     Assert.That(inventory3.Items.Count(i => i.ItemTypeID == 1002), Is.EqualTo(1));
       // }
       // [Test]
       // public void Test_RemoveItem()
       // {
       //     Inventory inventory = new Inventory();
       //     GameItem item1 = ItemFactory.CreateGameItem(9001);
       //     GameItem item2 = ItemFactory.CreateGameItem(9002);
       //     Inventory inventory1 = inventory.AddItems(new List<GameItem> { item1, item2 });
       //     Inventory inventory2 = inventory1.RemoveItem(item1);
       //     Assert.That(inventory2.Items.Count, Is.EqualTo(1));
       // }
       // [Test]
       // public void Test_RemoveItems()
       // {
       //     Inventory inventory = new Inventory();
       //     GameItem item1 = ItemFactory.CreateGameItem(9001);
       //     GameItem item2 = ItemFactory.CreateGameItem(9002);
       //     GameItem item3 = ItemFactory.CreateGameItem(9002);
       //     Inventory inventory1 = inventory.AddItems(new List<GameItem> { item1, item2, item3 });
       //     Inventory inventory2 = inventory1.RemoveItems(new List<GameItem> { item1, item3 });
       //     Assert.That(inventory2.Items.Count(), Is.EqualTo(1));
       // }
       // [Test]
       // public void Test_CategorizedItemProperties()
       // {
       //     Inventory inventory = new Inventory();
       //     Assert.That(inventory.Weapons.Count, Is.EqualTo(0));
       //     Assert.That(inventory.Consumables.Count, Is.EqualTo(0));

       //     Inventory inventory1 = inventory.AddItemFromFactory(1001);
       //     Assert.That(inventory1.Weapons.Count, Is.EqualTo(1));
       //     Assert.That(inventory1.Consumables.Count, Is.EqualTo(0));

       //     Inventory inventory2 = inventory1.AddItemFromFactory(9001);
       //     Assert.That(inventory2.Weapons.Count, Is.EqualTo(1));
       //     Assert.That(inventory2.Consumables.Count, Is.EqualTo(0));

       //     Inventory inventory3 = inventory2.AddItemFromFactory(1002);
       //     Assert.That(inventory3.Weapons.Count, Is.EqualTo(2));
       //     Assert.That(inventory3.Consumables.Count, Is.EqualTo(0));

       //     Inventory inventory4 = inventory3.AddItemFromFactory(2001);
       //     Assert.That(inventory4.Weapons.Count, Is.EqualTo(2));
       //     Assert.That(inventory4.Consumables.Count, Is.EqualTo(1));
       // }
       // [Test]
       // public void Test_RemoveItemQuantities()
       // {
       //     Inventory inventory = new Inventory();
       //     Assert.That(inventory.Weapons.Count, Is.EqualTo(0));
       //     Assert.That(inventory.Consumables.Count, Is.EqualTo(0));

       //     Inventory inventory1 = inventory
       //                            .AddItemFromFactory(1001)
       //                            .AddItemFromFactory(1002)
       //                            .AddItemFromFactory(1002)
       //                            .AddItemFromFactory(1002)
       //                            .AddItemFromFactory(1002)
       //                            .AddItemFromFactory(3001)
       //                            .AddItemFromFactory(3001);
       //     Assert.That(inventory1.Items.Count(i => i.ItemTypeID == 1001), Is.EqualTo(1));
       //     Assert.That(inventory1.Items.Count(i => i.ItemTypeID == 1002), Is.EqualTo(4));
       //     Assert.That(inventory1.Items.Count(i => i.ItemTypeID == 3001), Is.EqualTo(2));

       //     Inventory inventory2 = inventory1
       //                            .RemoveItems(new List<ItemQuantity> { new ItemQuantity(1002, 2) });
       //     Assert.That(inventory2.Items.Count(i => i.ItemTypeID == 1001), Is.EqualTo(1));
       //     Assert.That(inventory2.Items.Count(i => i.ItemTypeID == 1002), Is.EqualTo(2));
       //     Assert.That(inventory2.Items.Count(i => i.ItemTypeID == 3001), Is.EqualTo(2));

       //     Inventory inventory3 = inventory2
       //                            .RemoveItems(new List<ItemQuantity> { new ItemQuantity(1002, 1) });
       //     Assert.That(inventory3.Items.Count(i => i.ItemTypeID == 1001), Is.EqualTo(1));
       //     Assert.That(inventory3.Items.Count(i => i.ItemTypeID == 1002), Is.EqualTo(1));
       //     Assert.That(inventory3.Items.Count(i => i.ItemTypeID == 3001), Is.EqualTo(2));
       // }
       // [Test]
       //public void Test_RemoveItemQuantities_RemoveTooMany()
       // {
       //     Assert.Throws<InvalidOperationException>(() =>
       //     {
       //         Inventory inventory = new Inventory();
       //         Assert.That(inventory.Weapons.Count, Is.EqualTo(0));
       //         Assert.That(inventory.Consumables.Count, Is.EqualTo(0));

       //         Inventory inventory2 =
       //             inventory
       //                 .AddItemFromFactory(1001)
       //                 .AddItemFromFactory(1002)
       //                 .AddItemFromFactory(1002)
       //                 .AddItemFromFactory(1002)
       //                 .AddItemFromFactory(1002)
       //                 .AddItemFromFactory(3001)
       //                 .AddItemFromFactory(3001);
       //         Assert.That(inventory2.Items.Count(i => i.ItemTypeID == 1001), Is.EqualTo(1));
       //         Assert.That(inventory2.Items.Count(i => i.ItemTypeID == 1002), Is.EqualTo(4));
       //         Assert.That(inventory2.Items.Count(i => i.ItemTypeID == 3001), Is.EqualTo(2));

       //         Inventory inventory3 =
       //         inventory2
       //             .RemoveItems(new List<ItemQuantity> { new ItemQuantity(1002, 999) });
       //     });
       // }
    }
}
