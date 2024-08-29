﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSCSRPG.Models.EventArgs;
using SOSCSRPG.Core;
using SOSCSRPG.Models.Shared;

namespace SOSCSRPG.Models
{
    public class Battle : IDisposable
    {
        #region Private Variables
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();
        private readonly Player _player;
        private readonly Monster _opponent;
        #endregion

        public event EventHandler<CombatVictoryEventArgs> OnCombatVictory;
        public enum Combatant
        {
            Player,
            Opponent
        }
        #region Constructor
        public Battle(Player player, Monster opponent)
        {
            _player = player;
            _opponent = opponent;

            _player.OnActionPerformed += OnCombatantActionPerformed;
            _opponent.OnActionPerformed += OnCombatantActionPerformed;
            _opponent.OnKilled += OnOpponentKilled;

            _messageBroker.RaiseMessage("");
            _messageBroker.RaiseMessage($"You see a {_opponent.Name} here!");

            if(FirstAttacker(_player, _opponent) == Combatant.Opponent)
            {
                AttackPlayer();
            }
        }
        #endregion

        #region Public Functions
        public void AttackOpponent()
        {
            if(_player.CurrentWeapon == null)
            {
                _messageBroker.RaiseMessage("You must select a weapon to attack");
                return;
            }
            _player.UseCurrentWeaponOn(_opponent);
            if(_opponent.IsAlive)
            {
                AttackPlayer();
            }
        }
        public void Dispose()
        {
            _player.OnActionPerformed -= OnCombatantActionPerformed;
            _opponent.OnActionPerformed -= OnCombatantActionPerformed;
            _opponent.OnKilled -= OnOpponentKilled;
        }
        private void OnOpponentKilled(object sender, System.EventArgs e)
        {
            _messageBroker.RaiseMessage("");
            _messageBroker.RaiseMessage($"You defeated the {_opponent.Name}.");

            _player.AddExperience(_opponent.RewardExperiencePoints);
            _messageBroker.RaiseMessage($"You receive {_opponent.RewardExperiencePoints} experience points.");

            _player.ReceiveGold(_opponent.Gold);
            _messageBroker.RaiseMessage($"You receive {_opponent.Gold} gold.");

            foreach (GameItem gameItem in _opponent.Inventory.Items)
            {
                _messageBroker.RaiseMessage($"You receive one {gameItem.Name}.");
                _player.AddItemToInventory(gameItem);
            }
            OnCombatVictory?.Invoke(this, new CombatVictoryEventArgs());
        }
        private void AttackPlayer()
        {
            _opponent.UseCurrentWeaponOn(_player);
        }
        private void OnCombatantActionPerformed(object sender, string result)
        {
            _messageBroker.RaiseMessage(result);
        }
        private static Combatant FirstAttacker(Player player, Monster opponent)
        {
            // Formula is: ((Dex(player)^2) - Dex(monster)^2)/10) + random(-10/10)
            // Results in +- 41.5
            int playerDexterity = player.GetAttribute("DEX").ModifiedValue * player.GetAttribute("DEX").ModifiedValue;
            int opponentDexterity = opponent.GetAttribute("DEX").ModifiedValue * opponent.GetAttribute("DEX").ModifiedValue;
            decimal dexterityOffset = (playerDexterity - opponentDexterity) / 10m;
            int randomOffset = DiceService.Instance.Roll(20).Value - 10;
            decimal totalOffset = dexterityOffset + randomOffset;

            return DiceService.Instance.Roll(100).Value <= 50 + totalOffset
                            ? Combatant.Player
                            : Combatant.Opponent;
        }
        #endregion
    }
}
