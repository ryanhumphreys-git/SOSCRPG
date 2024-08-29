using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using SOSCSRPG.Services.Factories;
using System.ComponentModel;
using static System.Collections.Specialized.BitVector32;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using System.Text.Json;
using SOSCSRPG.Models.EventArgs;
using SOSCSRPG.Core;
using SOSCSRPG.Models;
using SOSCSRPG.Services;

namespace SOSCSRPG.ViewModels
{
    public class GameSession : INotifyPropertyChanged
    {
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
        private Battle _currentBattle;
        private Location _currentLocation;
        private Monster _currentMonster;
        private Player _currentPlayer;
        [JsonIgnore]
        public GameDetails GameDetails { get; private set; }
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

                CurrentMonster = MonsterFactory.GetMonsterFromLocation(CurrentLocation);

                CurrentVendor = CurrentLocation.VendorHere;
                CurrentQuestGiver = CurrentLocation.QuestGiverHere;
            }
        }
        [JsonIgnore]
        public Monster CurrentMonster
        {
            get => _currentMonster; 
            set
            {
                if(_currentBattle != null)
                {
                    _currentBattle.OnCombatVictory -= OnCurrentMonsterKilled;
                    _currentBattle.Dispose();
                    _currentBattle = null;
                }

                _currentMonster = value;

                if(_currentMonster != null)
                {
                    _currentBattle = new Battle(CurrentPlayer, CurrentMonster);
                    _currentBattle.OnCombatVictory += OnCurrentMonsterKilled;
                }
            }
        }
        [JsonIgnore]
        public Vendor CurrentVendor { get; private set; }
        [JsonIgnore]
        public QuestGiver CurrentQuestGiver { get; private set; }
        [JsonIgnore]
        public bool HasLocationToNorth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;
        [JsonIgnore]
        public bool HasLocationToEast => CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;
        [JsonIgnore]
        public bool HasLocationToWest => CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;
        [JsonIgnore]
        public bool HasLocationToSouth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;
        [JsonIgnore]
        public World CurrentWorld { get; }
        [JsonIgnore]
        public bool HasMonster => CurrentMonster != null;
        [JsonIgnore]
        public bool HasVendor => CurrentVendor != null;
        [JsonIgnore]
        public bool HasQuestGiver => CurrentQuestGiver != null;
        [JsonIgnore]
        public bool HasTwoQuests => (CurrentLocation.QuestGiverHere?.QuestAvailableHere.Count > 1);
        [JsonIgnore]
        public Quest SelectedQuest { get; set; }
        #endregion
        #region Constructor
        public GameSession(Player player, int xCoordinate, int yCoordinate)
        {
            PopulateGameDetails();

            CurrentWorld = WorldFactory.CreateWorld();
            CurrentPlayer = player;
            CurrentLocation = CurrentWorld.LocationAt(xCoordinate, yCoordinate);
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
        public void PopulateGameDetails()
        {
            GameDetails = GameDetailsService.ReadGameDetails();
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
                    _messageBroker.RaiseMessage($"You will also receive: ");
                    foreach(ItemQuantity itemQuantity in selectedQuest.RewardItems)
                    {
                        GameItem rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                        _messageBroker.RaiseMessage($"{rewardItem.Name}");
                    }
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
            _currentBattle?.AttackOpponent();
        }
        public void UseCurrentConsumable()
        {
            if(CurrentPlayer.CurrentConsumable != null)
            {
                if(_currentBattle == null)
                {
                    CurrentPlayer.OnActionPerformed += OnConsumableActionPerformed;
                }

                CurrentPlayer.UseCurrentConsumable();

                if(_currentBattle == null)
                {
                    CurrentPlayer.OnActionPerformed -= OnConsumableActionPerformed;
                }
            }
        }
        private void OnConsumableActionPerformed(object sender, string result)
        {
            _messageBroker.RaiseMessage(result);
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
                    _messageBroker.RaiseMessage($"   {itemQuantity.QuantityItemDescription}");
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
            CurrentMonster = MonsterFactory.GetMonsterFromLocation(CurrentLocation);
        }
        private void OnCurrentPlayerLeveledUp(object sender, System.EventArgs eventArgs)
        {
            _messageBroker.RaiseMessage("");
            _messageBroker.RaiseMessage($"You are now level {CurrentPlayer.Level}!");
        }
        #endregion
    }
}
