﻿using System;
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
            "#     2 #                   #   o #".ToCharArray(),
            "#     o #               s   # #   #".ToCharArray(),
            "#  ######              ###  # #o###".ToCharArray(),
            "#         o     o      #    # #   #".ToCharArray(),
            "#############          #  ### ### #".ToCharArray(),
            "#        o    s       ###     # o #".ToCharArray(),
            "#           #        ##s####### ###".ToCharArray(),
            "#  ###############   #s4       s  #".ToCharArray(),
            "#  #3  o     #o      ##s######### #".ToCharArray(),
            "#  #######   #  o#    ###       # #".ToCharArray(),
            "#        # o   ###              # #".ToCharArray(),
            "######## #######1#      o       # #".ToCharArray(),
            "#h               #              #s#".ToCharArray(),
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

        public const char emptySymbol = ' ';
        public const char exitMapSymbol = 'e';
        public const char heroMapSymbol = 'h';
        public const char orgMapSymbol = 'o';
        public const char skeletonMapSymbol = 's';
        public const char wallMapSymbol = '#';
        public const char stickMapChar = '1';
        public const char clubMapChar  = '2';
        public const char spearMapChar = '3';
        public const char saberMapChar = '4';

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

        Unit[] unitsData;

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

            unitsData = new Unit[maxCountUnit];

            levelData = new char[rowCount, columnCount];
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    char symbol = levelInit[row][column];
                    levelData[row, column] = symbol;
                    bool isUnit = false;

                    switch (symbol)
                    {
                        case heroMapSymbol:
                            heroIndex = unitCounts;
                            isUnit = true;
                            break;
                        case orgMapSymbol:
                        case skeletonMapSymbol:
                            isUnit = true;
                            break;
                    }

                    if (isUnit)
                    {
                        unitsData[unitCounts] = new Unit(symbol, row, column);
                        unitCounts++;
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
                        MoveUnit(unitsData[heroIndex], unitsData[heroIndex].row, unitsData[heroIndex].column - 1);
                        break;
                    case ConsoleKey.D:
                        MoveUnit(unitsData[heroIndex], unitsData[heroIndex].row, unitsData[heroIndex].column + 1);
                        break;
                    case ConsoleKey.S:
                        MoveUnit(unitsData[heroIndex], unitsData[heroIndex].row + 1, unitsData[heroIndex].column);
                        break;
                    case ConsoleKey.W:
                        MoveUnit(unitsData[heroIndex], unitsData[heroIndex].row - 1, unitsData[heroIndex].column);
                        break;
                }
                UpdateUI();
            }
        }

        void UpdateUI()
        {
            for (int i = 0; i < unitCounts; i++)
            {
                // Если это игрок, то перходим к следующему
                if (unitsData[i].Type == UnitType.Hero)
                    continue;

                // Игнорируем мертвых юнитов
                if (unitsData[i].health <= 0)
                    continue;

                // Получаем случайное направление
                int move = rand.Next(4);

                switch (move)
                {
                    // Move left
                    case 0:
                        MoveUnit(unitsData[i], unitsData[i].row, unitsData[i].column - 1);
                        break;
                    // Move right
                    case 1:
                        MoveUnit(unitsData[i], unitsData[i].row, unitsData[i].column + 1);
                        break;
                    //move up
                    case 2:
                        MoveUnit(unitsData[i], unitsData[i].row - 1, unitsData[i].column);
                        break;
                    // Move down
                    case 3:
                        MoveUnit(unitsData[i], unitsData[i].row + 1, unitsData[i].column);
                        break;
                }

            }
        }

        void MoveUnit(Unit unit, int row, int column)
        {
            bool canMove = false;
            char nextCell = levelData[row, column];

            switch (nextCell)
            {
                case emptySymbol:
                    canMove = true;
                    break;
                case heroMapSymbol:
                case orgMapSymbol:
                case skeletonMapSymbol:
                    UnitType destinationType = Unit.GetUnitType(nextCell);
                    // Своих не атакуем
                    if (destinationType != unit.Type)
                    {
                        // Ищем кого надо атаковать
                        for (int u = 0; u < unitCounts; u++)
                        {
                            // игнорируем уже мртвых
                            if (unitsData[u].health <= 0)
                                continue;

                            // нашли
                            if (unitsData[u].row == row && unitsData[u].column == column)
                            {
                                //атакуем
                                unitsData[u].health -= unit.weapon.Damage;

                                // Если враг умер надо его убрать с клетки
                                if (unitsData[u].health <= 0)
                                {
                                    levelData[row, column] = emptySymbol;
                                }
                                // for break
                                break;
                            }
                        }
                    }
                    // switch break
                    break;

            }

            if (unit.Type == UnitType.Hero)
            {
                // Нашли выход
                switch (nextCell)
                {
                    case stickMapChar:
                    case clubMapChar:
                    case spearMapChar:
                    case saberMapChar:
                        canMove = true;
                        WeaponType weapon = Weapon.GetWeaponTypeFromCell(nextCell);
                        if (unit.weapon.Type < weapon)
                            unit.weapon = new Weapon(weapon);
                            //unit.weapon.Type = weapon; // ПРОБЛЕМА!
                        break;
                    case exitMapSymbol:
                        isGameACtive = false;
                        break;
                }
            }

            if (canMove)
            {
                levelData[unit.row, unit.column] = emptySymbol;
                levelData[row, column] = unit.symbol;
                unit.row = row;
                unit.column = column;
            }
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
