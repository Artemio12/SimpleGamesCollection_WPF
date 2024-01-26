using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MatchGame1
{
    internal abstract class BaseDamager
    {
        protected int currentHP;
        public int CurrentHP { get =>currentHP; set => currentHP = value; }
        protected int maxHP;
        public abstract void TakeDamage();
    }
}
