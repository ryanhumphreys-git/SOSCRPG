using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;

namespace SOSCSRPG.Models
{
    public abstract class LivingEntity :INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
        private GameItem _currentWeapon;
        private GameItem _currentConsumable;

        public ObservableCollection<PlayerAttribute> Attributes { get; } = new ObservableCollection<PlayerAttribute>();
        public string Name { get; }
        public int CurrentHitPoints { get; private set; }
        public int MaximumHitPoints { get; protected set; }
        public int Gold { get; private set; }
        public int Level { get; protected set; }
        public Inventory Inventory { get; private set; }
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
            }
        }
        [JsonIgnore]
        public bool IsAlive => CurrentHitPoints > 0;
        [JsonIgnore]
        public bool IsDead => !IsAlive;
        public event EventHandler OnKilled;
        public event EventHandler<string> OnActionPerformed;
        #endregion
        #region Constructors
        protected LivingEntity(string name, int maximumHitPoints, int currentHitPoints, IEnumerable<PlayerAttribute> attributes, int gold, int level = 1)
        {
            Name= name;
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = currentHitPoints;
            Gold = gold;
            Level = level;

            foreach(PlayerAttribute attribute in attributes)
            {
                Attributes.Add(attribute);
            }

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
