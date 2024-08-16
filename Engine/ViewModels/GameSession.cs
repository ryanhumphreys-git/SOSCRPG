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
        private QuestGiver _currentNonPlayerCharacter;
        private Vendor _currentVendor;
        private QuestGiver _currentQuestGiver;
        public Player CurrentPlayer {  get; set; }
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                OnPropertyChanged(nameof(CurrentLocation));
                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToSouth));
                OnPropertyChanged(nameof(HasLocationToWest));

                //CompleteQuestsAtLocation();
                //GivePlayerQuestAtLocation();
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
                _currentMonster = value;

                OnPropertyChanged(nameof(CurrentMonster));
                OnPropertyChanged(nameof(HasMonster));

                if (CurrentMonster != null)
                {
                    RaiseMessage("");
                    RaiseMessage($"You see a {CurrentMonster.Name} here!");
                }
            }
        }

        public Vendor CurrentVendor
        {
            get { return _currentVendor; }
            set
            {
                _currentVendor = value;

                OnPropertyChanged(nameof(CurrentVendor));
                OnPropertyChanged(nameof(HasVendor));
            }
        }

        public QuestGiver CurrentQuestGiver
        {
            get { return _currentQuestGiver; }
            set
            {
                _currentQuestGiver = value;

                OnPropertyChanged(nameof(CurrentQuestGiver));
                OnPropertyChanged(nameof(HasQuestGiver));
            }
        }
        public Weapon CurrentWeapon {  get; set; }
        public bool HasLocationToNorth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;
        public bool HasLocationToEast => CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;
        public bool HasLocationToWest => CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;
        public bool HasLocationToSouth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;
        public World CurrentWorld { get; set; }

        public bool HasMonster => CurrentMonster != null;
        public bool HasVendor => CurrentVendor != null;
        public bool HasQuestGiver => CurrentQuestGiver != null;
        public bool HasTwoQuests => !(CurrentLocation.QuestGiverHere.QuestAvailableHere.Count > 0);
        public Quest SelectedQuest { get; set; }
        public GameSession()
        {
            CurrentPlayer = new Player
            {
                Name = "Starxo",
                CharacterClass = "Fighter",
                CurrentHitPoints = 10,
                MaximumHitPoints = 10,
                Gold = 100,
                ExperiencePoints = 0,
                Level = 1
            };

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

        private void CompleteQuests()
        {
            if(CurrentQuestGiver == null)
            {
                return;

            }
            foreach(Quest quest in CurrentQuestGiver.QuestAvailableHere)
            {
                QuestStatus questToComplete =
                    CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.ID == quest.ID && !q.IsCompleted);

                if(questToComplete != null)
                {
                    if(CurrentPlayer.HasAllTheseItems(quest.ItemsToComplete))
                    {
                        foreach(ItemQuantity itemQuantity in quest.ItemsToComplete)
                        {
                            for(int i = 0; i < itemQuantity.Quantity; i++)
                            {
                                CurrentPlayer.RemoveItemFromInventory(CurrentPlayer.Inventory.First(item => item.ItemTypeID == itemQuantity.ItemID));
                            }
                        }
                        RaiseMessage("");
                        RaiseMessage($"You completed the '{quest.Name}' quest");
                        
                        CurrentPlayer.ExperiencePoints += quest.RewardExperiencePoints;
                        RaiseMessage($"You receive {quest.RewardExperiencePoints} experience points");
                        CurrentPlayer.Gold += quest.RewardGold;
                        RaiseMessage($"You receive {quest.RewardGold} gold");
                        foreach (ItemQuantity itemQuantity in quest.RewardItems)
                        {
                            GameItem rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                            CurrentPlayer.AddItemToInventory(rewardItem);
                            RaiseMessage($"You receive a {rewardItem.Name}");
                        }
                        
                        questToComplete.IsCompleted = true;
                    }
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
        //public void AcceptQuest(Quest currentQuest)
        //{
        //    if(CurrentQuestGiver == null)
        //    {
        //        return; 
        //    }
        //    foreach(Quest quest in CurrentQuestGiver.QuestAvailableHere)
        //    {
        //        if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.ID == quest.ID));
        //        {
        //            CurrentPlayer.Quests.Add(new QuestStatus(quest));

        //            RaiseMessage("");
        //            RaiseMessage($"You receive the '{quest.Name}' quest.");
        //            RaiseMessage(quest.Description);

        //            RaiseMessage("Return with:");
        //            foreach(ItemQuantity itemQuantity in quest.ItemsToComplete)
        //            {
        //                RaiseMessage($"   {itemQuantity.Quantity}  {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
        //            }
        //            RaiseMessage("And you will recieve:");
        //            RaiseMessage($"   {quest.RewardExperiencePoints} experience points");
        //            RaiseMessage($"   {quest.RewardGold} gold");
        //            foreach(ItemQuantity itemQuantity in quest.RewardItems)
        //            {
        //                RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
        //            }
        //        }
        //    }
        //}
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
                    CurrentMonster.CurrentHitPoints -= damageToMonster;
                    RaiseMessage($"You punch the {CurrentMonster.Name} for {damageToMonster} damage.");
                }
                else
                {
                    CurrentMonster.CurrentHitPoints -= damageToMonster;
                    RaiseMessage($"You hit the {CurrentMonster.Name} for {damageToMonster} damage.");
                }
            }

            if(CurrentMonster.CurrentHitPoints <= 0)
            {
                RaiseMessage("");
                RaiseMessage($"You defeated the {CurrentMonster.Name}!");

                CurrentPlayer.ExperiencePoints += CurrentMonster.RewardExperiencePoints;
                RaiseMessage($"You receive {CurrentMonster.RewardExperiencePoints} experience points.");

                CurrentPlayer.Gold += CurrentMonster.Gold;
                RaiseMessage($"You receive {CurrentMonster.Gold} gold.");

                foreach(GameItem gameItem in CurrentMonster.Inventory)
                {
                    CurrentPlayer.AddItemToInventory(gameItem);
                    RaiseMessage($"You receive one {gameItem.Name}.");
                }

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
                    CurrentPlayer.CurrentHitPoints -= damageToPlayer;
                    RaiseMessage($"The {CurrentMonster.Name} hits you for {damageToPlayer} damage.");
                }

                if(CurrentPlayer.CurrentHitPoints <= 0)
                {
                    RaiseMessage("");
                    RaiseMessage($"You have been slain by {CurrentMonster.Name}!");

                    CurrentLocation = CurrentWorld.LocationAt(0, -1);
                    CurrentPlayer.CurrentHitPoints = CurrentPlayer.Level * 10;
                }
            }
        }
    }
}
