using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Models
{
    public class World
    {
        private readonly List<Location> _locations = new List<Location>();

        public void AddLocation(Location location)
        {
            _locations.Add(location);
        }

        public Location LocationAt(int xCoordinate, int yCoordinate)
        {
            foreach(Location loc in _locations)
            {
                if(loc.XCoordinate == xCoordinate && loc.YCoordinate == yCoordinate)
                {
                    return loc;
                }
            }

            return null;
        }

    }
}
