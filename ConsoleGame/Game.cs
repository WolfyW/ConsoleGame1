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
        
        readonly char heroSymbol = EncodingChar(2);
        readonly char wallSymbol = EncodingChar(177);
        readonly char exitSymbol = EncodingChar(176);

        readonly char skeletonSymbol = EncodingChar(2);
        readonly char orgSymbol = EncodingChar(2);
        readonly char stickChar = EncodingChar(47);
        readonly char clubChar = EncodingChar(33);
        readonly char spearChar = EncodingChar(24);
        readonly char saberChar = EncodingChar(108);

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
            levelData = new char[rowCount, columnCount];
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    char symbol = levelInit[row][column];
                    levelData[row, column] = symbol;
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
