namespace SOSCSRPG.Models
{
    public class Vendor : LivingEntity
    {
        public int ID { get; }
        public string Imagename { get; }
        public string Name { get; }
        public Vendor(int id, string name, string imageName) : base(name, 9999, 9999, new List<PlayerAttribute>(), 9999)
        {
            ID = id;
            Name = name;
            Imagename = imageName;
        }
    }
}
