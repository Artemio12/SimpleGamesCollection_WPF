
namespace MatchGame1
{
    public abstract class BaseDamager 
    {
        public IEndable End { protected get; set; }
      
        protected int currentHP;
        public int CurrentHP { get =>currentHP; set => currentHP = value; }

        protected int maxHP;
        public int MaxHP => maxHP;
       
        public abstract void TakeDamage();
    }
}
