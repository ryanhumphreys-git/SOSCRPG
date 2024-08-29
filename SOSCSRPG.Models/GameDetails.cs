using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Models
{
    public class GameDetails
    {
        public string Title { get; }
        public string SubTitle { get; }
        public string Version { get; }

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
