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
        Fist,
        Stick,
        Club,
        Spear,
        Saber
    }

    class Unit
    {
        const int heroBaseHealth = 400;
        const int orgBaseHealth = 60;
        const int skeletonBaseHealth = 80;

        const WeaponType heroWeapon = WeaponType.Fist;
        const WeaponType orgWeapon = WeaponType.Club;
        const WeaponType skeletonWeapon = WeaponType.Saber;

        public char symbol;
        public UnitType Type;
        public int row;
        public int column;
        public int health;
        public WeaponType weapon;

        public Unit(char unit, int row, int column)
        {
            InitDate(unit);
            this.row = row;
            this.column = column;
            symbol = unit;
        }

        private void InitDate(char unitSym)
        {
            switch (unitSym)
            {
                case Game.heroMapSymbol:
                    Type =  UnitType.Hero;
                    weapon = heroWeapon;
                    health = heroBaseHealth;
                    break;
                case Game.orgMapSymbol:
                    Type = UnitType.Org;
                    weapon = orgWeapon;
                    health = orgBaseHealth;
                    break;
                case Game.skeletonMapSymbol:
                    Type = UnitType.Skeleton;
                    weapon = skeletonWeapon;
                    health = skeletonBaseHealth;
                    break;
            }
        }

    }
}
