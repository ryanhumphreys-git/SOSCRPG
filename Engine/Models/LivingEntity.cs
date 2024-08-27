using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Engine.Services;

namespace Engine.Models
{
    public abstract class LivingEntity :BaseNotificationClass
    {
        #region Backing Variables
        private string _name;
        private int _dexterity;
        private int _currentHitPoints;
        private int _maximumHitPoints;
        private int _gold;
        private int _level;
        private GameItem _currentWeapon;
        private GameItem _currentConsumable;
        private Inventory _inventory;
        #endregion
        #region Properties
        public string Name
        {
            get { return _name; }
            private set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public int Dexterity
        {
            get => _dexterity;
            private set
            {
                _dexterity = value;
                OnPropertyChanged();
            }
        }
        public int CurrentHitPoints
        {
            get { return _currentHitPoints; }
            private set
            {
                _currentHitPoints = value;
                OnPropertyChanged();
            }
        }
        public int MaximumHitPoints
        {
            get { return _maximumHitPoints; }
            protected set
            {
                _maximumHitPoints = value;
                OnPropertyChanged();
            }
        }
        public int Gold
        {
            get { return _gold; }
            private set
            {
                _gold = value;
                OnPropertyChanged();
            }
        }
        public int Level
        {
            get { return _level; }
            protected set
            {
                _level = value;
                OnPropertyChanged();
            }
        }
        public Inventory Inventory
        {
            get => _inventory;
            private set
            {
                _inventory = value;
                OnPropertyChanged();
            }
        }
        public GameItem CurrentWeapon
        {
            get { return _currentWeapon; }
            set
            {
                if(_currentWeapon != null )
                {
                    _currentWeapon.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }

                _currentWeapon = value;

                if(_currentWeapon != null )
                {
                    _currentWeapon.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }

                OnPropertyChanged();
            }
        }
        public GameItem CurrentConsumable
        {
            get => _currentConsumable;
            set
            {
                if(_currentConsumable != null )
                {
                    _currentConsumable.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }

                _currentConsumable = value;

                if( _currentConsumable != null )
                {
                    _currentConsumable.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }

                OnPropertyChanged();
            }
        }
        public bool IsAlive => CurrentHitPoints > 0;
        public bool IsDead => !IsAlive;
        public event EventHandler OnKilled;
        public event EventHandler<string> OnActionPerformed;
        #endregion
        #region Constructors
        protected LivingEntity(string name, int maximumHitPoints, int currentHitPoints, int dexterity, int gold, int level = 1)
        {
            Name= name;
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = currentHitPoints;
            Dexterity = dexterity;
            Gold = gold;
            Level = level;

            Inventory = new Inventory();
        }
        #endregion
        #region Public Functions
        public void UseCurrentWeaponOn(LivingEntity target)
        {
            CurrentWeapon.PerformAction(this, target);
        }
        public void UseCurrentConsumable()
        {
            CurrentConsumable.PerformAction(this, this);
            RemoveItemFromInventory(CurrentConsumable);
        }
        public void TakeDamage(int hitPointsOfDamage)
        {
            CurrentHitPoints -= hitPointsOfDamage;

            if(IsDead)
            {
                CurrentHitPoints = 0;
                RaiseOnKilledEvent();
            }
        }
        public void Heal(int hitPointsToHeal)
        {
            CurrentHitPoints += hitPointsToHeal;

            if(CurrentHitPoints > MaximumHitPoints)
            {
                CurrentHitPoints = MaximumHitPoints;
            }    
        }
        public void CompletelyHeal()
        {
            CurrentHitPoints = MaximumHitPoints;
        }
        public void ReceiveGold(int amountOfGold)
        {
            Gold += amountOfGold;
        }
        public void SpendGold(int amountOfGold)
        {
            if(amountOfGold > Gold)
            {
                throw new ArgumentOutOfRangeException($"{Name} only has {Gold} gold, and afford this.");
            }

            Gold -= amountOfGold;
        }
        public void AddItemToInventory(GameItem item)
        {
            Inventory = Inventory.AddItem(item);
        }
        public void RemoveItemFromInventory(GameItem item)
        {
            Inventory = Inventory.RemoveItem(item);
        }
        public void RemoveItemsFromInventory(List<ItemQuantity> itemQuantities)
        {
            Inventory = Inventory.RemoveItems(itemQuantities);
        }
        #endregion
        #region Private Functions
        private void RaiseOnKilledEvent()
        {
            OnKilled?.Invoke(this, new System.EventArgs());
        }
        private void RaiseActionPerformedEvent(object sender, string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
        #endregion
    }
}
