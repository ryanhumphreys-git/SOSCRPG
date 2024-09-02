using System.Text.Json.Nodes;
using System.Xml;

namespace SOSCSRPG.Models.Shared
{
    public static class ExtensionMethods
    {
        public static int AttributeAsInt(this XmlNode node, string attributeName)
        {
            return Convert.ToInt32(node.AttributeAsString(attributeName));
        }

        public static string AttributeAsString(this XmlNode node, string attributeName)
        {
            XmlAttribute attribute = node.Attributes?[attributeName];
            if (attribute == null)
            {
                throw new ArgumentException($"The attribute '{attributeName}' does not exist");
            }
            return attribute.Value;
        }
        public static string StringValueOf(this JsonObject jsonObject, string key)
        {
            if(jsonObject.TryGetPropertyValue(key, out JsonNode value))
            {
                return value.ToString();
            }
            return null;
        }
        public static int IntValueOf(this JsonObject jsonElement, string key)
        {
            if(jsonElement.TryGetPropertyValue(key, out JsonNode value))
            {
                string valueString = value.ToString();
                return Convert.ToInt32(valueString);
            }
            throw new Exception("value cannot be converted to int");
        }
        public static PlayerAttribute GetAttribute(this LivingEntity entity, string attributeKey)
        {
            return entity.Attributes.First(pa => pa.Key.Equals(attributeKey, StringComparison.CurrentCultureIgnoreCase));
        }
        public static List<GameItem> ItemsThatAre(this IEnumerable<GameItem> inventory, GameItem.ItemCategory category)
        {
            return inventory.Where(i => i.Category == category).ToList();
        }
        //public static string QuestNameOf(this Quest quest)
        //{

        //}
    }
}
