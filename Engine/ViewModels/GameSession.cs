using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Engine.Factories;
using System.ComponentModel;
using Engine.EventArgs;

namespace Engine.ViewModels
{
    public class GameSession : BaseNotificationClass
    {
        public event EventHandler<GameMessageEventArgs> OnMessageRaised;
        public event EventHandler<GameMessageEventArgs> OnWindowMessageRaised;

        private Location _currentLocation;
        private Monster _currentMonster;
        private Player _currentPlayer;
        private Vendor _currentVendor;
        private QuestGiver _currentQuestGiver;
        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                if (_currentPlayer != null)
                {
                    _currentPlayer.OnLeveledUp -= OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled -= OnCurrentPlayerKilled;
                }

                _currentPlayer = value;

                if (_currentPlayer != null)
                {
                    _currentPlayer.OnLeveledUp += OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled += OnCurrentPlayerKilled;
                }
            }
        }
            
            
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToSouth));
                OnPropertyChanged(nameof(HasLocationToWest));

                GetMonsterAtLocation();

                CurrentVendor = CurrentLocation.VendorHere;
                CurrentQuestGiver = CurrentLocation.QuestGiverHere;
            }
        }

        public Monster CurrentMonster
        {
            get { return _currentMonster; }
            set
            {
                if(_currentMonster != null)
                {
                    _currentMonster.OnKilled -= OnCurrentMonsterKilled;
                }

                _currentMonster = value;

                if(_currentMonster != null)
                {
                    _currentMonster.OnKilled += OnCurrentMonsterKilled;

                    RaiseMessage("");
                    RaiseMessage($"You see a {CurrentMonster.Name} here.");
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasMonster));
            }
        }

        public Vendor CurrentVendor
        {
            get { return _currentVendor; }
            set
            {
                _currentVendor = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasVendor));
            }
        }

        public QuestGiver CurrentQuestGiver
        {
            get { return _currentQuestGiver; }
            set
            {
                _currentQuestGiver = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasQuestGiver));
            }
        }
        public Weapon CurrentWeapon {  get; set; }
        public bool HasLocationToNorth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;
        public bool HasLocationToEast => CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;
        public bool HasLocationToWest => CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;
        public bool HasLocationToSouth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;
        public World CurrentWorld { get; }

        public bool HasMonster => CurrentMonster != null;
        public bool HasVendor => CurrentVendor != null;
        public bool HasQuestGiver => CurrentQuestGiver != null;
        public bool HasTwoQuests => !(CurrentLocation.QuestGiverHere.QuestAvailableHere.Count > 0);

        public Quest SelectedQuest { get; set; }
        public GameSession()
        {
            CurrentPlayer = new Player("Starxo", "Fighter", 0, 100, 10, 100);
            
            if(!CurrentPlayer.Weapons.Any())
            {
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            }

            CurrentWorld = WorldFactory.CreateWorld();

            CurrentLocation = CurrentWorld.LocationAt(0, -1);
        }

        public void MoveNorth()
        {
            if(HasLocationToNorth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);
            }
            
        }
        public void MoveEast()
        {
            if(HasLocationToEast)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);
            }
            
        }
        public void MoveWest()
        {
            if(HasLocationToWest)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
            }
            
        }
        public void MoveSouth()
        {
            if(HasLocationToSouth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1);
            }
            
        }

        public void CompleteQuest(Quest currentQuest)
        {
            if(CurrentQuestGiver == null)
            {
                return;

            }
            QuestStatus questToComplete =
                CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.ID == currentQuest.ID && !q.IsCompleted);

            if(questToComplete != null)
            {
                if(CurrentPlayer.HasAllTheseItems(currentQuest.ItemsToComplete))
                {
                    foreach(ItemQuantity itemQuantity in currentQuest.ItemsToComplete)
                    {
                        for(int i = 0; i < itemQuantity.Quantity; i++)
                        {
                            CurrentPlayer.RemoveItemFromInventory(CurrentPlayer.Inventory.First(item => item.ItemTypeID == itemQuantity.ItemID));
                        }
                    }
                    RaiseMessage("");
                    RaiseMessage($"You completed the '{currentQuest.Name}' quest");

                    RaiseMessage($"You receive {currentQuest.RewardExperiencePoints} experience points");
                    CurrentPlayer.AddExperience(currentQuest.RewardExperiencePoints);

                    RaiseMessage($"You receive {currentQuest.RewardGold} gold");
                    CurrentPlayer.ReceiveGold(currentQuest.RewardGold);
                    
                    foreach (ItemQuantity itemQuantity in currentQuest.RewardItems)
                    {
                        GameItem rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                        RaiseMessage($"You receive a {rewardItem.Name}");
                        CurrentPlayer.AddItemToInventory(rewardItem);
                    }
                        
                    questToComplete.IsCompleted = true;
                }
            }
        }

        public void AcceptQuest(Quest currentQuest)
        {
            if(CurrentPlayer.Quests.Any(q => q.PlayerQuest.ID == currentQuest.ID))
            {
                return;
            }
            CurrentPlayer.Quests.Add(new QuestStatus(currentQuest));

            RaiseMessage($"You have accepted {currentQuest.Name}");
        }
        private void GetMonsterAtLocation()
        {
            CurrentMonster = CurrentLocation.GetMonster();
        }

        public void RaiseMessage(string message)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
        }

        public void RaiseQuestMessage(string message)
        {
            OnWindowMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
        }
        public void AttackCurrentMonster()
        {
            int damageToMonster;
            if(CurrentWeapon == null)
            {
                damageToMonster = RandomNumberGenerator.NumberBetween(0, 4);
            }
            else
            {
                damageToMonster = RandomNumberGenerator.NumberBetween(CurrentWeapon.MinimumDamage, CurrentWeapon.MaximumDamage);
            }

            if (damageToMonster == 0)
            {
                RaiseMessage($"You missed the {CurrentMonster.Name}.");
            }
            else
            {
                if (CurrentWeapon == null)
                {
                    RaiseMessage($"You punch the {CurrentMonster.Name} for {damageToMonster}.");
                    CurrentMonster.TakeDamage(damageToMonster);
                }
                else
                {
                    RaiseMessage($"You hit the {CurrentMonster.Name} for {damageToMonster}.");
                    CurrentMonster.TakeDamage(damageToMonster);
                }
            }

            if(CurrentMonster.IsDead)
            {
                GetMonsterAtLocation();
            }
            else
            {
                int damageToPlayer = RandomNumberGenerator.NumberBetween(CurrentMonster.MinimumDamage, CurrentMonster.MaximumDamage);

                if(damageToPlayer == 0)
                {
                    RaiseMessage($"The {CurrentMonster.Name} attacks, but misses.");
                }    
                else
                {
                    RaiseMessage($"The {CurrentMonster.Name} hits you for {damageToPlayer} damage.");
                    CurrentPlayer.TakeDamage(damageToPlayer);
                }
            }
        }

        private void OnCurrentPlayerKilled(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage("");
            RaiseMessage($"You have been slain by {CurrentMonster.Name}.");

            CurrentLocation = CurrentWorld.LocationAt(0, -1);
            CurrentPlayer.CompletelyHeal();
        }

        private void OnCurrentMonsterKilled(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage("");
            RaiseMessage($"You defeated the {CurrentMonster.Name}.");

            CurrentPlayer.AddExperience(CurrentMonster.RewardExperiencePoints);
            RaiseMessage($"You receive {CurrentMonster.RewardExperiencePoints} experience points.");

            CurrentPlayer.ReceiveGold(CurrentMonster.Gold);
            RaiseMessage($"You receive {CurrentMonster.Gold} gold.");

            foreach (GameItem gameItem in CurrentMonster.Inventory)
            {
                RaiseMessage($"You receive one {gameItem.Name}.");
                CurrentPlayer.AddItemToInventory(gameItem);
            }
        }

        private void OnCurrentPlayerLeveledUp(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage("");
            RaiseMessage($"You are now level {CurrentPlayer.Level}!");
        }
    }
}
