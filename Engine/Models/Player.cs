using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

namespace Engine.Models
{
    public class Player : LivingEntity
    {
        #region Backing Variables
        
        private int _experiencePoints;
        #endregion
        #region Public Variables
        public int ExperiencePoints
        {
            get => _experiencePoints;
            private set
            {
                _experiencePoints = value;
                OnPropertyChanged();

                SetLevelAndMaximumHitpoints();
            }
        }
        #endregion
        #region Collections
        public ObservableCollection<QuestStatus> Quests { get; } = new ObservableCollection<QuestStatus>();
        public ObservableCollection<Recipe>  Recipes { get; } = new ObservableCollection<Recipe>();
        #endregion
        public event EventHandler OnLeveledUp;
        #region Constructor
        public Player(string name, int experiencePoints,
                      int maximumHitPoints, int currentHitPoints, IEnumerable<PlayerAttribute> attributes, int gold) :
            base(name, maximumHitPoints, currentHitPoints, attributes, gold)
        {
            ExperiencePoints = experiencePoints;
        }
        #endregion
        #region Public Functions
        public void AddExperience(int experiencePoints)
        {
            ExperiencePoints += experiencePoints;
        }
        public void LearnRecipe(Recipe recipe)
        {
            if(!Recipes.Any(r => r.ID == recipe.ID))
            {
                Recipes.Add(recipe);
            }
        }
        private void SetLevelAndMaximumHitpoints()
        {
            int originalLevel = Level;

            Level = (ExperiencePoints / 100) + 1;

            if(Level != originalLevel)
            {
                MaximumHitPoints = Level * 10;

                OnLeveledUp?.Invoke(this, System.EventArgs.Empty);
            }
        }
        #endregion
    }
}
