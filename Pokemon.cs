using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

/**       
 *--------------------------------------------------------------------
 * 	   File name: Pokemon
 * 	Project name:CrowdisLab3
 *--------------------------------------------------------------------
 * Author’s name and email:	 kinsley crowdis crowdis@etsu.edu			
 *          Course-Section: CSCI 2150-800
 *           Creation Date:	09/14/2023
 * -------------------------------------------------------------------
 */

namespace CrowdisLab3
{
    internal class Pokemon : IClassModel
    {
        public int Id { get; set; }
        public int DexNumber { get; set; }
        public string Name { get; set; }
        public string? Form { get; set; }
        public string Type1 { get; set; }
        public string? Type2 { get; set; }
        public int Total { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpecialAttack { get; set; }
        public int SpecialDefense { get; set; }
        public int Speed { get; set; }
        public int Generation { get; set; }

        public Pokemon() { }
        public Pokemon(int id, int dexNumber, string name, string? form, string type1,
        string type2, int total, int hp, int attack, int defense, int specialAttack,
        int specialDefense, int speed, int generation)
        {
           Id = id;
           DexNumber = dexNumber;
           Name = name;
           Form = form;
           Type1 = type1;
           Type2 = type2;
           Total = total;
           HP = hp;
           Attack = attack;
           Defense = defense;
           SpecialAttack = specialAttack;
           SpecialDefense = specialDefense;
           Speed = speed;
           Generation = generation;
        }

        // tostring

        public override string ToString()
        {
            string msg = "";

            msg += $"Id: {Id}  \n";
            msg += $"Dex Number: {DexNumber}  \n";
            msg += $"Name: {Name}  \n";
            msg += $"Form: {Form}  \n";
            msg += $"Type 1: {Type1}  \n";
            msg += $"Type 2: {Type2}  \n";
            msg += $"Total: {Total}  \n";
            msg += $"Hit Points: {HP}  \n";
            msg += $"Attack: {Attack}  \n";
            msg += $"Defense: {Defense}  \n";
            msg += $"Special Attack: {SpecialAttack}  \n";
            msg += $"Special Defense: {SpecialDefense}  \n";
            msg += $"Speed: {Speed}  \n";
            msg += $"Generation: {Generation}  \n";

            return msg;
        }
    }

}
