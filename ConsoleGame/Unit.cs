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

    class Unit
    {
#region Consts
        const int heroBaseHealth = 400;
        const int orgBaseHealth = 60;
        const int skeletonBaseHealth = 80;

        const WeaponType heroWeapon = WeaponType.Fist;
        const WeaponType orgWeapon = WeaponType.Club;
        const WeaponType skeletonWeapon = WeaponType.Saber;
#endregion

        public char symbol;
        public UnitType Type;
        public int row;
        public int column;
        public int health;
        public Weapon weapon;

        public Unit(char unit, int row, int column)
        {
            InitDate(unit);
            this.row = row;
            this.column = column;
            symbol = unit;
        }

        public static UnitType GetUnitType(char unit)
        {
            switch (unit)
            {
                case Game.heroMapSymbol: return UnitType.Hero;
                case Game.orgMapSymbol: return UnitType.Org;
                case Game.skeletonMapSymbol: return UnitType.Skeleton;
                default: return UnitType.None;
            }
        }

        private void InitDate(char unitSym)
        {
            switch (unitSym)
            {
                case Game.heroMapSymbol:
                    Type =  UnitType.Hero;
                    weapon = new Weapon(heroWeapon);
                    health = heroBaseHealth;
                    break;
                case Game.orgMapSymbol:
                    Type = UnitType.Org;
                    weapon = new Weapon(orgWeapon);
                    health = orgBaseHealth;
                    break;
                case Game.skeletonMapSymbol:
                    Type = UnitType.Skeleton;
                    weapon = new Weapon(skeletonWeapon);
                    health = skeletonBaseHealth;
                    break;
            }
        }

    }
}
