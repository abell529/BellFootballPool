using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Xml.Linq;
using System.Xml.XPath;

// Developing for .NET is much easier using Visual Studio
// it is recommended to get the free express and develop your code offline before integrating it into OU Campus
// http://www.asp.net/vwd



public partial class _Default_SA : System.Web.UI.Page
{
    // Notice the using OUC; statement on line 7
    // This file assumes you have uploaded the
    // Wnl.cs file to the App_Code folder
    protected void Page_Load(object sender, EventArgs e)
    {

        string query = Request.Form["query"].ToLower();
        // string tuequivalent = Request.Form["tuequivalent"].ToLower();

        string theurl = "/football/thepool/towson/xml/studyabroad-data.xml";

        string xpath = "/studyabroad/courses/overseascourse";
        // string xpathtest = "/studyabroad/courses/overseascourse";

        // string test = "Success";
        // int qty = 0;
        

        List<XElement> elements = new List<XElement>();
        //List<XElement> elementsTest = new List<XElement>();


        string output = "";


        try
        {
            string url = HttpContext.Current.Server.MapPath(theurl);

            var doc = XDocument.Load(url);

            elements.AddRange(doc.XPathSelectElements(xpath).ToList());
            List<XElement> results = elements.FindAll(delegate (XElement p) { return FindKeyWords(p, query); });

            // elementsTest.AddRange(doc.XPathSelectElements(xpathtest).ToList());

            // List<XElement> resultsFindTUCourse = elementsTest.FindAll(delegate(XElement s) { return FindTUCourse(s, tuequivalent); });
            // List<XElement> resultsFindTUCourse = elementsTest.FindAll(delegate (XElement s) { return s.Element("tuequivalent").Value == tuequivalent; });

            // results.AddRange(resultsFindTUCourse);	


            // output += "\nAfter List XElement results";
            // output += "\nAfter List XElement resultsFindTUCourse";			
            // output += "\nAfter AddRange";			




            /* if (resultsFindTUCourse.Count != 0)
            {
                output += resultsFindTUCourse.Count + " courses equated";
                output += @"<table width=""100%"" border=""0"" cellspacing=""3"" cellpadding=""3"">";
                output += "<thead><tr>";
                output += "<th align=left>Host Institution</th>";
                output += "<th align=left>Country</th>";
                output += "<th align=left>Overseas Course</th>";
                output += "<th align=left>TU Equivalent</th>";
                output += "<th align=left>Approved By</th>";
                output += "<th align=left>Expiration Date</th>";
                output += @"<th width=""90"">Additional info</th>";
                output += "</tr></thead><tbody>";
                resultsFindTUCourse.ForEach(item => {
                    output += "<tr>";
                    output += String.Format("<td>{0}</td>", item.Element("institution").Value);
                    XElement institutionTemp = item.Element("institution");
                    output += String.Format("<td>{0}</td>", institutionTemp.Attribute("country").Value);
                    output += String.Format("<td>{0}</td>", item.Element("title").Value);
                    output += String.Format("<td>{0}</td>", item.Element("tuequivalent").Value);
                    output += String.Format("<td>{0}</td>", item.Element("approvedby").Value);
                    output += String.Format("<td>{0} (to fix)</td>", item.Element("dateapproved").Value);
                    output += String.Format("<td>{0}</td>", item.Element("notes").Value);
                    output += "</tr>";
                });
                output += "</tbody></table>";
            }
            else
            {
                output += "\nSorry, no courses were found using that search criteria.";
            }
        }
        catch (Exception exp)
        {
            output += "\nFailure - exception error message: " + exp.Message;

        } */







        if (results.Count != 0)
        {
            results.ForEach(item => {
                output += String.Format("<a href=\"{0}\" class=\"program_link\">{1}</a>", item.Attribute("url").Value, item.Element("title").Value);
                
            });
        }
        else
        {
            output += "\nNo items found.";
        }
    }
    catch (Exception exp)
    {
        output+= "Failed due to" + exp.Message;

        }


results1.InnerHtml = output;


        //results1.InnerHtml = output;

}     // end page load


    // Explicit predicate delegate. 
    private static bool FindKeyWords(XElement p, string query)
    {
    	bool match = false;
    	string[] keywords = p.Element("search").Elements("keyword")
    		   .Select(k => k.Value.ToLower())
    			   .ToArray();
    	string[] words = query.Split(' ');

    	foreach(string word in words) {
    		if (Array.IndexOf(keywords, word) > -1) {
    			match = true;
    		} 
    	}

    	return match;
    }


    /* private static bool FindTUCourse(XElement s, string tuequivalent)
    {
        bool match = false;
        //string[] keywords = s.Element("overseascourse").Elements("tuequivalent")
        //string[] keywords = s.Element("search").Elements("keyword")
        string[] keywords = s.Elements("tuequivalent")
                   .Select(k => k.Value.ToLower())
                   .ToArray();
        string[] words = tuequivalent.Split(' ');

        foreach (string word in words)
        {
            if (Array.IndexOf(keywords, word) > -1)
            {
                match = true;
            }
        }

        return match;
    } */


}
