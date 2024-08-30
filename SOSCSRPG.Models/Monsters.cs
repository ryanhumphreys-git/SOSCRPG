namespace SOSCSRPG.Models
{
    public class Monster : LivingEntity
    {
        #region Public Variables
        public List<ItemPercentage> LootTable = new List<ItemPercentage>();
        public int ID { get; }
        public string ImageName {  get; }
        public int RewardExperiencePoints { get; }
        #endregion

        #region Constructor
        public Monster(int id, string name, string imageName,
                       int maximumHitPoints, IEnumerable<PlayerAttribute> attributes,
                       GameItem currentWeapon,
                       int rewardExperiencePoints, int gold) :
            base(name, maximumHitPoints, maximumHitPoints, attributes, gold)
        {
            ID = id;
            ImageName = imageName;
            CurrentWeapon = currentWeapon;
            RewardExperiencePoints = rewardExperiencePoints;
        }
        #endregion
        #region Public Functions
        public void AddItemToLootTable(int id, int percentage)
        {
            LootTable.RemoveAll(ip => ip.ID == id);

            LootTable.Add(new ItemPercentage(id, percentage));
        }
        public Monster Clone()
        {
            Monster newMonster = new Monster(ID, Name, ImageName, MaximumHitPoints, Attributes,
                                             CurrentWeapon, RewardExperiencePoints, Gold);

            newMonster.LootTable.AddRange(LootTable);

            return newMonster;
        }
        #endregion
    }
}
