using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for standings
/// </summary>
namespace standings
{
    public class Currentstandings
    {
        public Overallstandings[] overallstandings { get; set; }

    }

    public class Overallstandings
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public double total { get; set; }

    }

}