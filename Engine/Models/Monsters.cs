using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Monster : LivingEntity
    {
        public string ImageName {  get; set; }
        public int MinimumDamage { get; set; }
        public int MaximumDamage { get; set; }   

        public int RewardExperiencePoints { get; private set; }

        //string imageName,
        public Monster(string name, int maxHitPoints, int hitPoints, int mimimunDamage, int maximumDamage, int rewardExperience, int rewardGold)
        {
            Name = name;
            //ImageName = $"/Engine;component/Images/Monsters/{imageName}");
            MaximumHitPoints = maxHitPoints;
            CurrentHitPoints = hitPoints;
            MinimumDamage = mimimunDamage;
            MaximumDamage = maximumDamage;
            RewardExperiencePoints = rewardExperience;
            Gold = rewardGold;
        }
    }
}
