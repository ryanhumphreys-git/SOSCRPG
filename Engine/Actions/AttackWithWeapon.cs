using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Engine.Services;


namespace Engine.Actions
{
    public class AttackWithWeapon : BaseActions, IAction
    {
        private readonly GameItem _weapon;
        private readonly int _maximumDamage;
        private readonly int _minimumDamage;

        public AttackWithWeapon(GameItem itemInUse, int minimumDamage, int maximumDamage) : base(itemInUse)
        {
            if(itemInUse.Category != GameItem.ItemCategory.Weapon)
            {
                throw new ArgumentException($"{itemInUse.Name} is not a weapon");
            }

            if(minimumDamage < 0)
            {
                throw new ArgumentException("minimumDamage must be 0 or larger");
            }

            if(maximumDamage < minimumDamage)
            {
                throw new ArgumentException("maximumDamage must be >= minimumDamage");
            }

            _weapon = itemInUse;
            _minimumDamage = minimumDamage;
            _maximumDamage = maximumDamage;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            

            string actorName = (actor is Player) ? "You" : $"The {actor.Name}";
            string targetName = (target is Player) ? "you" : $"the {target.Name}";

            if(BattleService.AttackSucceeded(actor, target))
            {
                int damage = RandomNumberGenerator.NumberBetween(_minimumDamage, _maximumDamage);
                ReportResult($"{actorName} hit {target.Name} for {damage} damage{(damage > 1 ? "s" : "")}" +
                    $" with {actor.CurrentWeapon.Name}.");
                target.TakeDamage(damage);
            }
            else
            {
                ReportResult($"{actorName} missed {target.Name}.");
            }
        }
    }
}
