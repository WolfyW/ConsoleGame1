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
            "#     o                 s         #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#               o                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#                      o          #".ToCharArray(),
            "#          s                      #".ToCharArray(),
            "#                                 #".ToCharArray(),
            "#               o                 #".ToCharArray(),
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
        int maxCountUnit = 35;
        int unitCounts = 0;
        bool isGameACtive = true;
        char[,] levelData;
        Random rand;

        #region Данные монстров

        const int heroBaseHealth = 400;
        const int orgBaseHealth = 60;
        const int skeletonBaseHealth = 80;

        int[] unitsData;
        int[] rowUnit;
        int[] columnUnit;
        int[] healthUnit;

        int heroIndex = 0;

        #endregion

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

            unitsData = new int[maxCountUnit];
            rowUnit = new int[maxCountUnit];
            columnUnit = new int[maxCountUnit];
            healthUnit = new int[maxCountUnit];

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
                            heroIndex = unitCounts;
                            unitsData[unitCounts] = unitCounts;
                            rowUnit[unitCounts] = row;
                            columnUnit[unitCounts] = column;
                            healthUnit[unitCounts] = GetUnitDefaultHealth(symbol);
                            unitCounts++;
                            break;

                        case orgMapSymbol:
                        case skeletonMapSymbol:
                            unitsData[unitCounts] = unitCounts;
                            rowUnit[unitCounts] = row;
                            columnUnit[unitCounts] = column;
                            healthUnit[unitCounts] = GetUnitDefaultHealth(symbol);
                            unitCounts++;
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
                        MoveUnit(heroIndex, rowUnit[heroIndex], columnUnit[heroIndex] - 1);
                        break;
                    case ConsoleKey.D:
                        MoveUnit(heroIndex, rowUnit[heroIndex], columnUnit[heroIndex] + 1);
                        break;
                    case ConsoleKey.S:
                        MoveUnit(heroIndex, rowUnit[heroIndex] + 1, columnUnit[heroIndex]);
                        break;
                    case ConsoleKey.W:
                        MoveUnit(heroIndex, rowUnit[heroIndex] - 1, columnUnit[heroIndex]);
                        break;
                }
                UpdateUI();
            }
        }

        void UpdateUI()
        {
            for (int i = 0; i < unitCounts; i++)
            {
                if (i == heroIndex)
                    continue;

                int move = rand.Next(4);

                switch (move)
                {
                    // Move left
                    case 0:
                        MoveUnit(i, rowUnit[i], columnUnit[i] - 1);
                        break;
                    // Move right
                    case 1:
                        MoveUnit(i, rowUnit[i], columnUnit[i] + 1);
                        break;
                    //move up
                    case 2:
                        MoveUnit(i, rowUnit[i] - 1, columnUnit[i]);
                        break;
                    // Move down
                    case 3:
                        MoveUnit(i, rowUnit[i] + 1, columnUnit[i]);
                        break;
                }

            }
        }

        void MoveUnit(int unitIndex, int row, int column)
        {
            bool canMove = false;
            char nextCell = levelData[row, column];
            char unitSymbol = levelData[rowUnit[unitIndex], columnUnit[unitIndex]];

            switch (nextCell)
            {
                case emptySymbol:
                    canMove = true;
                    break;
            }

            if (unitSymbol == heroMapSymbol)
            {
                switch (nextCell)
                {
                    case exitMapSymbol:
                        isGameACtive = false;
                        break;
                }
            }

            if (canMove)
            {
                levelData[rowUnit[unitIndex], columnUnit[unitIndex]] = emptySymbol;
                levelData[row, column] = unitSymbol;
                rowUnit[unitIndex] = row;
                columnUnit[unitIndex] = column;
            }
        }

        int GetUnitDefaultHealth(char unit)
        {
            int health = 0;
            switch (unit)
            {
                case heroMapSymbol:
                    health = heroBaseHealth;
                    break;
                case orgMapSymbol:
                    health = orgBaseHealth;
                    break;
                case skeletonMapSymbol:
                    health = skeletonBaseHealth;
                    break;
            }
            return health;
        }

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
