using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    enum UnitType
    {
        Org,
        Skeleton,
        Hero
    }

    class Unit
    {
        public char symbol;
        public UnitType Type;
        public int row;
        public int column;
        public int health;
    }
}
