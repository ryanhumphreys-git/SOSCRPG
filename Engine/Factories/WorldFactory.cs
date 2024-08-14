using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    internal static class WorldFactory
    {
        internal static World CreateWorld()
        {
            World newWorld = new World();

            newWorld.AddLocation(0, -1, "Home", "This is your home");//, "/Engine;component/Images/Locations/Home.png");
            newWorld.AddLocation(0, 0, "Town square", "You see a fountain.");

            newWorld.AddLocation(0, 1, "Alchemist's hut", "There are many strange plants on the shelves.");
            newWorld.LocationAt(0, 1).QuestsAvailableHere.Add(QuestFactory.GetQuestByID(1));

            newWorld.AddLocation(0, 2, "Alchemist's garden", "Many plants are growing here.");
            newWorld.LocationAt(0, 2).AddMonster(2, 75);
            newWorld.LocationAt(0, 2).AddMonster(4, 25);

            newWorld.AddLocation(-1, 0, "Farmhouse", "There is a small farmhouse, with a farmer in front.");

            newWorld.AddLocation(-2, 0, "Farmer's field", "You see rows of vegetables growing here.");
            newWorld.LocationAt(-2, 0).AddMonster(1, 75);
            newWorld.LocationAt(-2, 0).AddMonster(5, 25);

            newWorld.AddLocation(1, 0, "Guard post", "There is a large, tough-looking guard here.");
            newWorld.AddLocation(2, 0, "Bridge", "A stone bridge crosses a wide river");
            newWorld.AddLocation(3, 0, "Forest", "You see spider webs covering the trees in this forest.");
            newWorld.LocationAt(3, 0).AddMonster(3, 100);

            newWorld.AddLocation(4, 0, "Lair of the Spider Queen", "You see webs covering all areas of the room and skeletons riddled across the floor.");
            newWorld.LocationAt(4, 0).AddMonster(13, 100);
            
            newWorld.AddLocation(0, -2, "Winding Path", "A calm winding path through the forest. " +
                "You are at peace for the moment.");
            newWorld.AddLocation(0, -3, "Forest Clearing", "You see animals scurry away.");
            newWorld.AddLocation(0, -4, "Dryad Shack", "You see a small abode made out of living trees. A dryad sits calmly inside.");
            newWorld.AddLocation(-1, -4, "Abandoned Campground", "You see empty tents and a fire with a small pot over it");   
            newWorld.AddLocation(0, -5, "Corrupted Forest", "You see mangey animals wandering around. Some stare at you " +
                "with red eyes");
            newWorld.LocationAt(0, -5).AddMonster(8, 33);
            newWorld.LocationAt(0, -5).AddMonster(9, 33);
            newWorld.LocationAt(0, -5).AddMonster(10, 33);

            newWorld.AddLocation(1, -4, "Elemental Clearing", "You see living beings made of rock and water. The air" +
                " tingles with elemental energy");
            newWorld.LocationAt(1, -4).AddMonster(6, 33);
            newWorld.LocationAt(1, -4).AddMonster(7, 33);


            newWorld.AddLocation(1, -2, "Mountain Path", "A winding path on the mountain. You feel hints" +
                " of corruption around you. As you approach the summit the air becomes thick and unbearable.");
            newWorld.LocationAt(1, -2).AddMonster(11, 33);

            newWorld.AddLocation(2, -2, "Mountain Peak", "You are so high you see white clouds as far " +
                "as the eye can see");
            newWorld.AddLocation(3, -2, "Perch of the Bird God", "You see a large bird in a throne-like nest surrounded " +
                "by other smaller birds. He offers you a quest to assist his flock.");
            newWorld.AddLocation(4, -2, "Overlook", "You see a beautiful view over the side of the mountains. A river flowing gently " +
                "and birds and animals roaming the land. It is serene.");
            newWorld.AddLocation(3, -3, "Mountain Path", "A winding path on the mountain. " +
                "You see a ravine at the end of the path.");
            newWorld.LocationAt(3, -3).AddMonster(12, 33);

            newWorld.AddLocation(3, -4, "Den of The Alpha", "The den is dark and has no way to escape. You see " +
                "two glowing red eyes staring at you from the darkness. Is this the end?");
            newWorld.LocationAt(3, -4).AddMonster(14, 100);
            return newWorld;
        }
    }
}
