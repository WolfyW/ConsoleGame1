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

        const char emptySymbol = ' ';
        const char exitMapSymbol = 'e';
        const char heroMapSymbol = 'h';
        const char orgMapSymbol = 'o';
        const char skeletonMapSymbol = 's';
        const char wallMapSymbol = '#';
        const char stickMapChar = '1';
        const char clubMapChar  = '2';
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
                        unitsData[unitCounts] = new Unit()
                        {
                            symbol = symbol,
                            Type = GetUnitType(symbol),
                            row = row,
                            column = column,
                            health = GetUnitDefaultHealth(symbol),
                            weapon = GetUnitWeapon(symbol)
                        };
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
            char unitSymbol = unit.symbol;

            switch (nextCell)
            {
                case emptySymbol:
                    canMove = true;
                    break;
                case heroMapSymbol:
                case orgMapSymbol:
                case skeletonMapSymbol:
                    UnitType destinationType = GetUnitType(nextCell);
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
                                // находим урон
                                int damage = GetWeaponDamage(unit.weapon);

                                //атакуем
                                unitsData[u].health -= damage;

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
                        WeaponType weapon = GetWeaponTypeFromCell(nextCell);
                        if (unit.weapon < weapon)
                            unit.weapon = weapon;
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

        #region Получаем значения

        int GetUnitDefaultHealth(char unit)
        {
            switch (unit)
            {
                case heroMapSymbol: return heroBaseHealth;
                case orgMapSymbol: return orgBaseHealth;
                case skeletonMapSymbol: return skeletonBaseHealth;
                default: return 0;
            }
        }

        UnitType GetUnitType(char unit)
        {
            switch (unit)
            {
                case heroMapSymbol: return UnitType.Hero;
                case orgMapSymbol: return UnitType.Org;
                case skeletonMapSymbol: return UnitType.Skeleton;
                default: return UnitType.None;
            }
        }

        WeaponType GetUnitWeapon(char unit)
        {
            switch (unit)
            {
                case heroMapSymbol: return WeaponType.First;
                case orgMapSymbol: return WeaponType.Club;
                case skeletonMapSymbol: return WeaponType.Saber;
                default: return WeaponType.None;
            }
        }

        WeaponType GetWeaponTypeFromCell(char cell)
        {
            switch (cell)
            {
                case stickMapChar: return WeaponType.Stick;
                case clubMapChar: return WeaponType.Club;
                case spearMapChar: return WeaponType.Spear;
                case saberMapChar: return WeaponType.Saber;
                default: return WeaponType.None;
            }
        }

        int GetWeaponDamage(WeaponType weapon)
        {
            switch (weapon)
            {
                case WeaponType.First: return 2;
                case WeaponType.Stick: return 16;
                case WeaponType.Club:  return 24;
                case WeaponType.Spear: return 32;
                case WeaponType.Saber: return 40;
            }
            return 0;
        }

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
