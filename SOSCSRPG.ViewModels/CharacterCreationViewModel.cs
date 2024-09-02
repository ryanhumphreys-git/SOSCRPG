﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using SOSCSRPG.Models;
using SOSCSRPG.Services;
using SOSCSRPG.Services.Factories;

namespace SOSCSRPG.ViewModels
{
    public class CharacterCreationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public GameDetails GameDetails { get; }
        public Race SelectedRace { get; init; }
        public string Name { get; init; }
        public ObservableCollection<PlayerAttribute> PlayerAttributes { get; } = new ObservableCollection<PlayerAttribute>();
        public bool HasRaces => GameDetails.Races.Any();
        public bool HasRaceAttributeModifiers => HasRaces && GameDetails.Races.Any(r => r.PlayerAttributeModifiers.Any());

        public CharacterCreationViewModel()
        {
            GameDetails = GameDetailsService.ReadGameDetails();

            if(HasRaces)
            {
                SelectedRace = GameDetails.Races.First();
            }

            RollNewCharacter();
        }
        public void RollNewCharacter()
        {
            PlayerAttributes.Clear();

            foreach(PlayerAttribute playerAttribute in GameDetails.PlayerAttributes)
            {
                playerAttribute.ReRoll();
                PlayerAttributes.Add(playerAttribute);
            }

            ApplyAttributeModifiers();
        }
        public void ApplyAttributeModifiers()
        {
            foreach(PlayerAttribute playerAttribute in PlayerAttributes)
            {
                var attributeRaceModifier = SelectedRace.PlayerAttributeModifiers.FirstOrDefault(pam => pam.AttributeKey.Equals(playerAttribute.Key));

                playerAttribute.ModifiedValue = playerAttribute.BaseValue + (attributeRaceModifier?.Modifier ?? 0);
            }
        }
        public Player GetPlayer()
        {
            Player player = new Player(Name, 0, 100, 100, PlayerAttributes, 10);
            player.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            player.AddItemToInventory(ItemFactory.CreateGameItem(2001));
            player.LearnRecipe(RecipeFactory.RecipeByID(1));
            player.AddItemToInventory(ItemFactory.CreateGameItem(3001));
            player.AddItemToInventory(ItemFactory.CreateGameItem(3001));

            return player;
        }
    }
}
