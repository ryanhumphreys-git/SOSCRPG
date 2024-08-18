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
        public string ImageName {  get; }
        public int MinimumDamage { get; }
        public int MaximumDamage { get; }   

        public int RewardExperiencePoints { get; }

        //string imageName,
        public Monster(string name,
                       int maximumHitPoints, int currentHitPoints,
                       int mimimunDamage, int maximumDamage, 
                       int rewardExperience, int gold) :
            base(name, maximumHitPoints, currentHitPoints, gold)
        { 
            //ImageName = $"/Engine;component/Images/Monsters/{imageName}");
            MinimumDamage = mimimunDamage;
            MaximumDamage = maximumDamage;
            RewardExperiencePoints = rewardExperience;
        }
    }
}
