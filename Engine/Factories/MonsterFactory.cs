using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    public static class MonsterFactory
    {
        public static Monster GetMonster(int monsterID)
        {
            switch(monsterID)
            {
                case 1:
                    Monster snake =
                        new Monster("Snake", 5, 5, 3, 4, 5, 1);
                    AddLootItem(snake, 9001, 25);
                    AddLootItem(snake, 9002, 75);

                    return snake;
                case 2:
                    Monster rat =
                        new Monster("Rat", 3, 3, 1, 2, 5, 1);
                    AddLootItem(rat, 9003, 25);
                    AddLootItem(rat, 9004, 75);

                    return rat;
                case 3:
                    Monster giantSpider =
                        new Monster("Giant Spider", 10, 10, 6, 7, 10, 3);
                    AddLootItem(giantSpider, 9005, 25);
                    AddLootItem(giantSpider, 9006, 75);

                    return giantSpider;
                case 4:
                    Monster giantRat =
                        new Monster("Giant Rat", 6, 6, 2, 3, 10, 3);
                    AddLootItem(giantRat, 9003, 25);
                    AddLootItem(giantRat, 9004, 75);
                    AddLootItem(giantRat, 1003, 20);

                    return giantRat;
                case 5:
                    Monster giantSnake =
                        new Monster("Giant Snake", 10, 10, 5, 6, 10, 3);
                    AddLootItem(giantSnake, 9001, 25);
                    AddLootItem(giantSnake, 9002, 75);
                    AddLootItem(giantSnake, 1003, 20);

                    return giantSnake;
                case 6:
                    Monster angryEarthEle =
                        new Monster("Angry Earth Elemental", 30, 30, 8, 12, 20, 5);
                    AddLootItem(angryEarthEle, 9010, 75);
                    AddLootItem(angryEarthEle, 9013, 25);

                    return angryEarthEle;
                case 7:
                    Monster angryWaterEle =
                        new Monster("Angry Water Elemental", 30, 30, 8, 12, 20, 5);
                    AddLootItem(angryWaterEle, 9010, 75);
                    AddLootItem(angryWaterEle, 9014, 25);

                    return angryWaterEle;
                case 8:
                    Monster corruptedWolf =
                        new Monster("Corrupted Wolf", 30, 30, 8, 12, 20, 5);
                    AddLootItem(corruptedWolf, 9011, 75);
                    AddLootItem(corruptedWolf, 9012, 25);

                    return corruptedWolf;
                case 9:
                    Monster corruptedDeer =
                        new Monster("Corrupted Deer", 30, 30, 8, 12, 20, 5);
                    AddLootItem(corruptedDeer, 9011, 75);
                    AddLootItem(corruptedDeer, 9012, 25);

                    return corruptedDeer;
                case 10:
                    Monster corruptedBear =
                        new Monster("Corrupted Bear", 30, 30, 8, 12, 20, 5);
                    AddLootItem(corruptedBear, 9011, 75);
                    AddLootItem(corruptedBear, 9012, 25);

                    return corruptedBear;
                case 11:
                    Monster corruptedMotherBear =
                           new Monster("Corrupted Mother Bear", 30, 30, 8, 12, 20, 5);
                    AddLootItem(corruptedMotherBear, 9012, 50);

                    return corruptedMotherBear;
                case 12:
                    Monster guardingWolf =
                           new Monster("Guarding Wolf", 75, 75, 13, 20, 50, 15);
                    AddLootItem(guardingWolf, 9016, 50);

                    return guardingWolf;
                case 13:
                    Monster spiderQueen =
                        new Monster("Spider Queen", 110, 110, 15, 25, 150, 30);
                    AddLootItem(spiderQueen, 9009, 100);

                    return spiderQueen;
                case 14:
                    Monster alphaWolf =
                        new Monster("Alpha Wolf", 200, 200, 25, 40, 300, 200);
                    AddLootItem(alphaWolf, 9008, 100);

                    return alphaWolf;
                default:
                    throw new ArgumentException(string.Format("MonsterType '{0}' does not exist", monsterID));

            }
        }

        private static void AddLootItem(Monster monster, int itemID, int percentage)
        {
            if(RandomNumberGenerator.NumberBetween(1, 100) <= percentage)
            {
                monster.AddItemToInventory(ItemFactory.CreateGameItem(itemID));
            }
        }
    }
}
