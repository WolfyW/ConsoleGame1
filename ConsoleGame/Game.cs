using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    class Game
    {
        readonly char[][] levelData = {
            "###################################".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#h                                #".ToCharArray(),
            "#################################e#".ToCharArray(),
        };

        int rowCount = 15;
        int columnCount = 35;
        bool isGameACtive = true;
        public Game()
        {
            Initialize();
            while (isGameACtive)
            {
                Render();
                Update();
            }
        }

        void Initialize()
        {

        }

        void Render()
        {

        }

        void Update()
        {

        }


    }
}
