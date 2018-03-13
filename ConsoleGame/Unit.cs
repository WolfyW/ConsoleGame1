using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    enum UnitType
    {
        None,
        Org,
        Skeleton,
        Hero
    }

    enum WeaponType
    {
        None,
        First,
        Stick,
        Club,
        Spear,
        Saber
    }

    class Unit
    {
        public char symbol;
        public UnitType Type;
        public int row;
        public int column;
        public int health;
        public WeaponType weapon;
    }
}
