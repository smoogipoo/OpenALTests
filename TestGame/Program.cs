using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            GameBase game = new GameBase(new GameWindow(800, 600));
            game.Game.Run(60);
        }
    }
}
