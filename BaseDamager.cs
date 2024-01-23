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
        protected private IClickable clickable;
        public BaseDamager(IClickable clickable, ref StackPanel HPBar, TextBlock outputTextBlock)
        { 
            this.clickable = clickable;
        }

        protected List<Border> clickableBorderList;
        public List<Border> ClickableBorderList { get => clickableBorderList; set => clickableBorderList = value; }


        protected TextBlock outputTextBlock;
        public TextBlock OutputTextBlock => outputTextBlock;

        public abstract void TakeDamage();
        public abstract void GameOver(string finalInscription);
    }
}
