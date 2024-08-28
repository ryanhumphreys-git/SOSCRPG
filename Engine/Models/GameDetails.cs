using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class GameDetails
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Version { get; set; }

        public List<PlayerAttribute> PlayerAttributes { get; set; } = new List<PlayerAttribute>();
        public List<Race> Races { get; } = new List<Race>();
        public GameDetails(string title, string subTitle, string version)
        {
            Title = title;
            SubTitle = subTitle;
            Version = version;
        }
    }
}
