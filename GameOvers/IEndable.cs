using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MatchGame1
{
    public interface IEndable
    {
        void GameOver(string finalInscription);
    }
}
