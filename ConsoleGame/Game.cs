using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    class Game
    {
        #region Карта
        readonly char[][] levelInit = {
            "###################################".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#               o                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#h                               s#".ToCharArray(),
            "#################################e#".ToCharArray(),
        };
        
        readonly char heroSymbol = EncodingChar(2);
        readonly char wallSymbol = EncodingChar(177);
        readonly char exitSymbol = EncodingChar(176);

        readonly char skeletonSymbol = EncodingChar(2);
        readonly char orgSymbol = EncodingChar(2);
        readonly char stickChar = EncodingChar(47);
        readonly char clubChar = EncodingChar(33);
        readonly char spearChar = EncodingChar(24);
        readonly char saberChar = EncodingChar(108);

        const char emptySymbol = ' ';
        const char exitMapSymbol = 'e';
        const char heroMapSymbol = 'h';
        const char orgMapSymbol = 'o';
        const char skeletonMapSymbol = 's';
        const char wallMapSymbol = '#';
        const char stickMapChar = '1';
        const char clubMapChar = '2';
        const char spearMapChar = '3';
        const char saberMapChar = '4';

        #endregion

        int rowCount = 15;
        int columnCount = 35;
        bool isGameACtive = true;
        char[,] levelData;
        Random rand;

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
            rand = new Random();
            Console.CursorVisible = false;
            levelData = new char[rowCount, columnCount];
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    char symbol = levelInit[row][column];
                    levelData[row, column] = symbol;

                    switch (symbol)
                    {
                        case heroMapSymbol:
                            heroRow = row;
                            heroColumn = column;
                            heroHealth = heroBaseHealth;
                            break;
                        case orgMapSymbol:
                            orgRow = row;
                            orgColumn = column;
                            orgHealath = orgBaseHealth;
                            break;

                        case skeletonMapSymbol:
                            skeletonRow = row;
                            skeletonColumn = column;
                            skeletonHealth = skeletonBaseHealth;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        void Render()
        {
            Console.SetCursorPosition(0, 0);
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    char sym = GetRenderSymbol(levelData[row, column]);
                    ConsoleColor color = GetRenderColor(levelData[row, column]);
                    Console.ForegroundColor = color;
                    Console.Write(sym);
                }
                Console.WriteLine();
            }
        }

        void Update()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.A:
                        MoveHero(heroRow, heroColumn - 1);
                        break;
                    case ConsoleKey.D:
                        MoveHero(heroRow, heroColumn + 1);
                        break;
                    case ConsoleKey.S:
                        MoveHero(heroRow + 1, heroColumn);
                        break;
                    case ConsoleKey.W:
                        MoveHero(heroRow - 1, heroColumn);
                        break;
                }
                UpdateUI();
            }
        }

        void UpdateUI()
        {
            int moveOrg = rand.Next(4);

            switch (moveOrg)
            {
                // Move left
                case 0:
                    MoveOrg(orgRow, orgColumn - 1);
                    break;
                // Move right
                case 1:
                    MoveOrg(orgRow, orgColumn + 1);
                    break;
                //move up
                case 2:
                    MoveOrg(orgRow - 1, orgColumn);
                    break;
                // Move down
                case 3:
                    MoveOrg(orgRow + 1, orgColumn);
                    break;
            }


            int moveSkeleton = rand.Next(4);

            switch (moveSkeleton)
            {
                // Move left
                case 0:
                    MoveSkeleton(skeletonRow, skeletonColumn - 1);
                    break;
                // Move right
                case 1:
                    MoveSkeleton(skeletonRow, skeletonColumn + 1);
                    break;
                //move up
                case 2:
                    MoveSkeleton(skeletonRow - 1, skeletonColumn);
                    break;
                // Move down
                case 3:
                    MoveSkeleton(skeletonRow + 1, skeletonColumn);
                    break;
            }
        }

        void MoveHero(int row, int column)
        {
            bool canMove = false;
            char nextCell = levelData[row, column];

            switch (nextCell)
            {
                case emptySymbol:
                    canMove = true;
                    break;
                case exitMapSymbol:
                    isGameACtive = false;
                    break;
            }

            if (canMove)
            {
                levelData[heroRow, heroColumn] = emptySymbol;
                levelData[row, column] = heroMapSymbol;
                heroRow = row;
                heroColumn = column;
            }
        }

        void MoveOrg(int row, int column)
        {
            bool canMove = false;
            char nextCell = levelData[row, column];

            switch (nextCell)
            {
                case emptySymbol:
                    canMove = true;
                    break;
            }

            if (canMove)
            {
                levelData[orgRow, orgColumn] = emptySymbol;
                levelData[row, column] = orgMapSymbol;
                orgRow = row;
                orgColumn = column;
            }
        }

        void MoveSkeleton(int row, int column)
        {
            bool canMove = false;
            char nextCell = levelData[row, column];

            switch (nextCell)
            {
                case emptySymbol:
                    canMove = true;
                    break;
            }

            if (canMove)
            {
                levelData[skeletonRow, skeletonColumn] = emptySymbol;
                levelData[row, column] = skeletonMapSymbol;
                skeletonRow = row;
                skeletonColumn = column;
            }
        }

        #region Данные игрока
        const int heroBaseHealth = 400;
        int heroRow = 0;
        int heroColumn = 0;
        int heroHealth = 0;

        #endregion


        #region Данные монстров

        const int orgBaseHealth = 60;
        const int skeletonBaseHealth = 80;

        int orgRow = 0;
        int orgColumn = 0;
        int orgHealath = 0;

        int skeletonRow = 0;
        int skeletonColumn = 0;
        int skeletonHealth = 0;

        #endregion

        #region вспомоглательные функции рендера

        static char EncodingChar(byte numberSym)
        {
            Encoding encoder = Encoding.GetEncoding(437);
            byte[] sym = { numberSym };
            var symbol = encoder.GetString(sym)[0];
            return symbol;
        }

        char GetRenderSymbol(char symbol)
        {
            char sym = ' ';
            switch (symbol)
            {
                case wallMapSymbol:
                    sym = wallSymbol;
                    break;
                case heroMapSymbol:
                    sym = heroSymbol;
                    break;
                case orgMapSymbol:
                    sym = orgSymbol;
                    break;
                case skeletonMapSymbol:
                    sym = skeletonSymbol;
                    break;
                case stickMapChar:
                    sym = stickChar;
                    break;
                case clubMapChar:
                    sym = clubChar;
                    break;
                case spearMapChar:
                    sym = spearChar;
                    break;
                case saberMapChar:
                    sym = saberChar;
                    break;
                case exitMapSymbol:
                    sym = exitSymbol;
                    break;
                default:
                    sym = symbol;
                    break;
            }
            return sym;
        }

        ConsoleColor GetRenderColor(char symbol)
        {
            ConsoleColor Color = ConsoleColor.White;
            switch (symbol)
            {
                case wallMapSymbol:
                    Color = ConsoleColor.White;
                    break;
                case heroMapSymbol:
                    Color = ConsoleColor.Yellow;
                    break;
                case orgMapSymbol:
                    Color = ConsoleColor.Green;
                    break;
                case skeletonMapSymbol:
                    Color = ConsoleColor.White;
                    break;
                case stickMapChar:
                    Color = ConsoleColor.DarkYellow;
                    break;
                case clubMapChar:
                    Color = ConsoleColor.DarkRed;
                    break;
                case spearMapChar:
                    Color = ConsoleColor.DarkCyan;
                    break;
                case saberMapChar:
                    Color = ConsoleColor.Cyan;
                    break;
                case exitMapSymbol:
                    Color = ConsoleColor.DarkRed;
                    break;
                default:
                    Color = ConsoleColor.Black;
                    break;
            }

            return Color;
        }

        #endregion
    }
}
