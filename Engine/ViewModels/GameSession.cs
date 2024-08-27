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
using Engine.Services;
using static System.Collections.Specialized.BitVector32;

namespace Engine.ViewModels
{
    public class GameSession : BaseNotificationClass
    {
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();

        #region Backing Variables
        private Battle _currentBattle;
        private Location _currentLocation;
        private Monster _currentMonster;
        private Player _currentPlayer;
        private Vendor _currentVendor;
        private QuestGiver _currentQuestGiver;
        #endregion
        #region Properties
        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                if (_currentPlayer != null)
                {
                    _currentPlayer.OnLeveledUp -= OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled -= OnPlayerKilled;
                }

                _currentPlayer = value;

                if (_currentPlayer != null)
                {
                    _currentPlayer.OnLeveledUp += OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled += OnPlayerKilled;
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

                CurrentMonster = CurrentLocation.GetMonster();

                CurrentVendor = CurrentLocation.VendorHere;
                CurrentQuestGiver = CurrentLocation.QuestGiverHere;
            }
        }
        public Monster CurrentMonster
        {
            get { return _currentMonster; }
            set
            {
                if(_currentBattle != null)
                {
                    _currentBattle.OnCombatVictory -= OnCurrentMonsterKilled;
                    _currentBattle.Dispose();
                }

                _currentMonster = value;

                if(_currentMonster != null)
                {
                    _currentBattle = new Battle(CurrentPlayer, CurrentMonster);

                    _currentBattle.OnCombatVictory += OnCurrentMonsterKilled;
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
        #endregion
        #region Constructor
        public GameSession()
        {
            int dexterity = RandomNumberGenerator.NumberBetween(8, 18);

            CurrentPlayer = new Player("Starxo", "Fighter", 0, 10, 10, dexterity, 100);
            
            if(!CurrentPlayer.Inventory.Weapons.Any())
            {
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            }

            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(2001));
            CurrentPlayer.LearnRecipe(RecipeFactory.RecipeByID(1));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3001));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3001));

            CurrentWorld = WorldFactory.CreateWorld();

            CurrentLocation = CurrentWorld.LocationAt(0, -1);
        }
        #endregion
        #region Public Functions
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
                if(CurrentPlayer.Inventory.HasAllTheseItems(currentQuest.ItemsToComplete))
                {
                    CurrentPlayer.RemoveItemsFromInventory(currentQuest.ItemsToComplete);

                    _messageBroker.RaiseMessage("");
                    _messageBroker.RaiseMessage($"You completed the '{currentQuest.Name}' quest");

                    _messageBroker.RaiseMessage($"You receive {currentQuest.RewardExperiencePoints} experience points");
                    CurrentPlayer.AddExperience(currentQuest.RewardExperiencePoints);

                    _messageBroker.RaiseMessage($"You receive {currentQuest.RewardGold} gold");
                    CurrentPlayer.ReceiveGold(currentQuest.RewardGold);
                    
                    foreach (ItemQuantity itemQuantity in currentQuest.RewardItems)
                    {
                        GameItem rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                        _messageBroker.RaiseMessage($"You receive a {rewardItem.Name}");
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

            _messageBroker.RaiseMessage($"You have accepted {currentQuest.Name}");
        }
        public void SelectQuest(Quest selectedQuest)
        {
            if (CurrentPlayer.Quests.FirstOrDefault(pq => pq.PlayerQuest.ID == selectedQuest.ID) == null)
            {
                _messageBroker.RaiseMessage("");
                _messageBroker.RaiseMessage($"Would you like to accept {selectedQuest.Name}?");
                _messageBroker.RaiseMessage($"You must {selectedQuest.Description}");
                _messageBroker.RaiseMessage($"And will be rewarded with {selectedQuest.RewardExperiencePoints} experience, ");
                _messageBroker.RaiseMessage($"{selectedQuest.RewardGold} gold.");
                if (selectedQuest.RewardItems != null)
                {
                    _messageBroker.RaiseMessage($"You will also receive " +
                        $"{ItemFactory.ItemName(CurrentLocation.QuestGiverHere.QuestAvailableHere[0].RewardItems[0].ItemID)}.");
                }
            }
            else
            {

                _messageBroker.RaiseMessage("");
                _messageBroker.RaiseMessage($"Would you like to complete {selectedQuest.Name}?");
                _messageBroker.RaiseMessage($"You must {selectedQuest.Description}");
            }
        }
        public void AllQuestsAtQuestGiverAreCompleted(string questGiver)
        {
            _messageBroker.RaiseMessage($"You have completed all the {questGiver}'s quests.");
        }
        public void AttackCurrentMonster()
        {
            _currentBattle.AttackOpponent();
        }
        public void UseCurrentConsumable()
        {
            if(CurrentPlayer.CurrentConsumable != null)
            {
                CurrentPlayer.UseCurrentConsumable();
            }
        }
        public void CraftItemUsing(Recipe recipe)
        {
            if(CurrentPlayer.Inventory.HasAllTheseItems(recipe.Ingredients))
            {
                CurrentPlayer.RemoveItemsFromInventory(recipe.Ingredients);

                foreach(ItemQuantity itemQuantity in recipe.OutputItems)
                {
                    for(int i = 0; i < itemQuantity.Quantity; i++)
                    {
                        GameItem outputItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                        CurrentPlayer.AddItemToInventory(outputItem);
                        _messageBroker.RaiseMessage($"You craft 1 {outputItem.Name}");
                    }
                }
            }
            else
            {
                _messageBroker.RaiseMessage("You do not have the required ingredients");
                foreach(ItemQuantity itemQuantity in recipe.Ingredients)
                {
                    _messageBroker.RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.ItemName(itemQuantity.ItemID)}");
                }
            }
        }
        #endregion
        #region Private Functions
        private void OnPlayerKilled(object sender, System.EventArgs eventArgs)
        {
            _messageBroker.RaiseMessage("");
            _messageBroker.RaiseMessage($"You have been slain.");

            CurrentLocation = CurrentWorld.LocationAt(0, -1);
            CurrentPlayer.CompletelyHeal();
        }
        private void OnCurrentMonsterKilled(object sender, System.EventArgs eventArgs)
        {
            CurrentMonster = CurrentLocation.GetMonster();
        }
        private void OnCurrentPlayerLeveledUp(object sender, System.EventArgs eventArgs)
        {
            _messageBroker.RaiseMessage("");
            _messageBroker.RaiseMessage($"You are now level {CurrentPlayer.Level}!");
        }
        #endregion
    }
}
