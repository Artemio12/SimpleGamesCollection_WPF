using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

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
