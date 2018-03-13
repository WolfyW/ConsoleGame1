using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    enum WeaponType
    {
        None,
        Fist,
        Stick,
        Club,
        Spear,
        Saber
    }

    class Weapon
    {
#region Consts
        const int fistDmg  = 2;
        const int stickDmg = 16;
        const int clubDmg  = 24;
        const int spearDmg = 32;
        const int saberDmg = 40;
#endregion
        public WeaponType Type;
        public int Damage;

        public Weapon(WeaponType type)
        {
            Type = type;
            Damage = GetWeaponDamage(Type);
        }

        public Weapon(char w)
        {
            Type = GetWeaponTypeFromCell(w);
            Damage = GetWeaponDamage(Type);
        }

        public static WeaponType GetWeaponTypeFromCell(char cell)
        {
            switch (cell)
            {
                case Game.stickMapChar: return WeaponType.Stick;
                case Game.clubMapChar: return WeaponType.Club;
                case Game.spearMapChar: return WeaponType.Spear;
                case Game.saberMapChar: return WeaponType.Saber;
                default: return WeaponType.None;
            }
        }

        int GetWeaponDamage(WeaponType weapon)
        {
            switch (weapon)
            {
                case WeaponType.Fist: return fistDmg;
                case WeaponType.Stick: return stickDmg;
                case WeaponType.Club: return clubDmg;
                case WeaponType.Spear: return spearDmg;
                case WeaponType.Saber: return saberDmg;
                default: return 0;
            }
        }
    }
}
