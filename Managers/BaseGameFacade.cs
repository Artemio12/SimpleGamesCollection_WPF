using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MatchGame1
{
    internal abstract class BaseGameFacade
    {
        protected IEndable endable;
        protected abstract void SetUp();
        protected abstract void Restart();

        public abstract void StartGame(Button button);
        public abstract void GameOver(string finalInscription); 
    }
}
