﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Services;

namespace Engine.Models
{
    public class PlayerAttribute : BaseNotificationClass
    {
        private int _modifiedValue;
        public string Key { get; }
        public string DisplayName { get; }
        public string DiceNotation { get; }
        public int BaseValue { get; set; }
        public int ModifiedValue 
        {
            get => _modifiedValue;
            set
            {
                _modifiedValue = value;
                OnPropertyChanged();
            }
        }

        // Constructor that will use diceservice to create a base value
        // Will put the same value into basevalue and modifiedvalue
        public PlayerAttribute(string key, string displayName, string diceNotation)
        }

        // Constructor that takes a base value and also uses it for modified value
        // for when we're creating a new attribute
        public PlayerAttribute(string key, string displayName, string diceNotation, int baseValue)
            : this(key, displayName, diceNotation, baseValue, baseValue)
        {
        }

        public PlayerAttribute(string key, string displayName, string diceNotation, int baseValue, int modifiedValue)
        {
            Key = key;
            DisplayName = displayName;
            DiceNotation = diceNotation;
            BaseValue = baseValue;
            ModifiedValue = modifiedValue;
        }

        public void ReRoll()
        {
            BaseValue = DiceService.Instance.Roll(DiceNotation).Value;
            ModifiedValue = BaseValue;
        }
    }
}