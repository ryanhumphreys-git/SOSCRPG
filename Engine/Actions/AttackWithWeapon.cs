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
        private readonly string _damageDice;

        public AttackWithWeapon(GameItem itemInUse, string damageDice) : base(itemInUse)
        {
            if(itemInUse.Category != GameItem.ItemCategory.Weapon)
            {
                throw new ArgumentException($"{itemInUse.Name} is not a weapon");
            }

            if(string.IsNullOrWhiteSpace(damageDice))
            {
                throw new ArgumentException("damageDice must be valid dice notation");
            }

            _damageDice = damageDice;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            

            string actorName = (actor is Player) ? "You" : $"The {actor.Name}";
            string targetName = (target is Player) ? "you" : $"the {target.Name}";

            if(BattleService.AttackSucceeded(actor, target))
            {
                int damage = DiceService.Instance.Roll(_damageDice).Value;
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
