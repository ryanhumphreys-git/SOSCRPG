using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using SOSCSRPG.Services.Factories;
using System.Text.Json.Nodes;
using SOSCSRPG.Models.Shared;
using SOSCSRPG.Models;

namespace SOSCSRPG.Services
{
    public static class SaveGameService
    {
        public static void Save(GameState gameState, string fileName)
        {
            File.WriteAllText(fileName,
                              JsonSerializer.Serialize(gameState, new JsonSerializerOptions {  WriteIndented = true }));
        }
        public static GameState LoadLastSaveOrCreateNew(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"Filename: {fileName}");
            }
            try
            {
                JsonObject data = (JsonObject)JsonObject.Parse(File.ReadAllText(fileName));

                // Populate Player object
                Player player = CreatePlayer(data);
                int x = (int)data[nameof(GameState.XCoordinate)];
                int y = (int)data[nameof(GameState.YCoordinate)];

                // Create GameState object with saved game data
                return new GameState(player, x, y);
            }
            catch (Exception ex)
            {
                throw new FormatException($"Error reading: {fileName}");
            }
        }
        private static Player CreatePlayer(JsonObject data)
        {
            Player player =
                new Player((string)data[nameof(GameState.Player)][nameof(Player.Name)],
                            (int)data[nameof(GameState.Player)][nameof(Player.ExperiencePoints)],
                            (int)data[nameof(GameState.Player)][nameof(Player.MaximumHitPoints)],
                            (int)data[nameof(GameState.Player)][nameof(Player.CurrentHitPoints)],
                            GetPlayerAttributes(data),
                            (int)data[nameof(GameState.Player)][nameof(Player.Gold)]);

            PopulatePlayerInventory(data, player);
            PopulatePlayerQuests(data, player);
            PopulatePlayerRecipes(data, player);
            return player;
        }
        private static IEnumerable<PlayerAttribute> GetPlayerAttributes(JsonObject data)
        {
            List<PlayerAttribute> attributes = new List<PlayerAttribute>();
            foreach(JsonObject itemToken in (JsonArray)data[nameof(GameState.Player)][nameof(Player.Attributes)])
            {
                attributes.Add(new PlayerAttribute(
                                   (string)itemToken[nameof(PlayerAttribute.Key)],
                                   (string)itemToken[nameof(PlayerAttribute.DisplayName)],
                                   (string)itemToken[nameof(PlayerAttribute.DiceNotation)],
                                   (int)itemToken[nameof(PlayerAttribute.BaseValue)],
                                   (int)itemToken[nameof(PlayerAttribute.ModifiedValue)]));
            }
            return attributes;
        }
        private static void PopulatePlayerInventory(JsonObject data, Player player)
        {
            foreach (JsonObject itemToken in (JsonArray)data[nameof(GameState.Player)][nameof(Player.Inventory)][nameof(Inventory.Items)])
            {
                int itemId = (int)itemToken[nameof(GameItem.ItemTypeID)];
                player.AddItemToInventory(ItemFactory.CreateGameItem(itemId));
            }
        }
        private static void PopulatePlayerQuests(JsonObject data, Player player)
        {
            foreach (JsonObject questToken in (JsonArray)data[nameof(GameState.Player)][nameof(Player.Quests)])
            {
                int questId =
                    (int)questToken[nameof(QuestStatus.PlayerQuest)][nameof(QuestStatus.PlayerQuest.ID)];
                Quest quest = QuestFactory.GetQuestByID(questId);
                QuestStatus questStatus = new QuestStatus(quest);
                questStatus.IsCompleted = (bool)questToken[nameof(QuestStatus.IsCompleted)];
                player.Quests.Add(questStatus);
            }
        }
        private static void PopulatePlayerRecipes(JsonObject data, Player player)
        {
            foreach (JsonObject recipeToken in
                (JsonArray)data[nameof(GameState.Player)][nameof(Player.Recipes)])
            {
                int recipeId = (int)recipeToken[nameof(Recipe.ID)];
                Recipe recipe = RecipeFactory.RecipeByID(recipeId);
                player.Recipes.Add(recipe);
            }
        }
    }
}
