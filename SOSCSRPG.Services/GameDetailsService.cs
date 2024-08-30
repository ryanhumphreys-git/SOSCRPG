using System.Text.Json.Nodes;
using SOSCSRPG.Models;
using SOSCSRPG.Models.Shared;

namespace SOSCSRPG.Services
{
    public class GameDetailsService
    {
        public static GameDetails ReadGameDetails()
        {
            JsonObject gameDetailsJson = (JsonObject)JsonObject.Parse(File.ReadAllText(".\\GameData\\GameDetails.json"));
            GameDetails gameDetails = new GameDetails(gameDetailsJson.StringValueOf("Title"),
                                                      gameDetailsJson.StringValueOf("SubTitle"),
                                                      gameDetailsJson.StringValueOf("Version"));
            foreach(JsonObject token in (JsonArray)gameDetailsJson["PlayerAttributes"])
            {
                gameDetails.PlayerAttributes.Add(new PlayerAttribute(token.StringValueOf("Key"),
                                                                     token.StringValueOf("DisplayName"),
                                                                     token.StringValueOf("DiceNotation")));
            }
            if (gameDetailsJson["Races"] != null)
            {
                foreach(JsonObject token in (JsonArray)gameDetailsJson["Races"])
                {
                    Race race = new Race
                    {
                        Key = token.StringValueOf("Key"),
                        DisplayName = token.StringValueOf("DisplayName")
                    };
                    if (token["PlayerAttributeModifiers"] != null)
                    {
                        foreach(JsonObject childToken in (JsonArray)token["PlayerAttributeModifiers"])
                        {
                            race.PlayerAttributeModifiers.Add(new PlayerAttributeModifier
                            {
                                AttributeKey = childToken.StringValueOf("Key"),
                                Modifier = childToken.IntValueOf("Modifier")
                            });
                        }
                    }
                    gameDetails.Races.Add(race);
                }
            }
            return gameDetails;
        }
    }
}
