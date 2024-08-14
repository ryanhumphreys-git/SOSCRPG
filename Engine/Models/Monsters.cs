using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Monster : BaseNotificationClass
    {
        private int _hitPoints;

        public string Name { get; private set; }
        public string ImageName {  get; set; }
        public int MaximumHitPoints { get; private set; }
        public int MinimumDamage { get; set; }
        public int MaximumDamage { get; set; }
        public int HitPoints
        {
            get { return _hitPoints; }
            set
            {
                _hitPoints = value;
                OnPropertyChanged(nameof(HitPoints));
            }
        }

        public int RewardExperiencePoints { get; private set; }
        public int RewardGold { get; private set; }

        public ObservableCollection<ItemQuantity> Inventory { get; set; }

        //string imageName,
        public Monster(string name, int maxHitPoints, int hitPoints, int mimimunDamage, int maximumDamage, int rewardExperience, int rewardGold)
        {
            Name = name;
            //ImageName = string.Format($"/Engine;component/Images/Monsters/{0}", imageName);
            MaximumHitPoints = maxHitPoints;
            HitPoints = hitPoints;
            MinimumDamage = mimimunDamage;
            MaximumDamage = maximumDamage;
            RewardExperiencePoints = rewardExperience;
            RewardGold = rewardGold;

            Inventory = new ObservableCollection<ItemQuantity>();
        }
    }
}
