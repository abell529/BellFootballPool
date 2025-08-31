using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nflgames;
using standings;
using previousseason;

/// <summary>
/// Summary description for requiredVars
/// </summary>
namespace requiredVars
{
    public class PickList
    {
        public PickList(
            string pickfirst, 
            string picklast, 
            string pick1, 
            string pick2, 
            string pick3, 
            string pick4, 
            string pick5, 
            string pick6, 
            string pick7, 
            string pick8, 
            string pick9, 
            string pick10, 
            string pick11, 
            string pick12, 
            string pick13,
            string pick14, 
            string pick15,
            string pick16)
        {
            FirstNamepicks = pickfirst;
            LastNamepicks = picklast;
            PickGame1 = pick1;
            PickGame2 = pick2;
            PickGame3 = pick3;
            PickGame4 = pick4;
            PickGame5 = pick5;
            PickGame6 = pick6;
            PickGame7 = pick7;
            PickGame8 = pick8;
            PickGame9 = pick9;
            PickGame10 = pick10;
            PickGame11 = pick11;
            PickGame12 = pick12;
            PickGame13 = pick13;
            PickGame14 = pick14;
            PickGame15 = pick15;
            PickGame16 = pick16;

        }
        public string FirstNamepicks { get; set; }
        public string LastNamepicks { get; set; }
        public string PickGame1 { get; set; }
        public string PickGame2 { get; set; }
        public string PickGame3 { get; set; }
        public string PickGame4 { get; set; }
        public string PickGame5 { get; set; }
        public string PickGame6 { get; set; }
        public string PickGame7 { get; set; }
        public string PickGame8 { get; set; }
        public string PickGame9 { get; set; }
        public string PickGame10 { get; set; }
        public string PickGame11 { get; set; }
        public string PickGame12 { get; set; }
        public string PickGame13 { get; set; }
        public string PickGame14 { get; set; }
        public string PickGame15 { get; set; }
        public string PickGame16 { get; set; }


    }
}