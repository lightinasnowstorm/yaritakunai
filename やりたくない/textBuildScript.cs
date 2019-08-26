using System;
using System.Collections.Generic;
using System.Linq;


class textBuildScript
{
    /// <summary>
    /// Outputs required Monogame content XML character sets for all of the input strings.
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        LinkedList<char> allChars = new LinkedList<char>();
        foreach (string s in args)
        {
            foreach (char c in s)
            {
                if (!allChars.Contains(c))
                    allChars.AddLast(c);
            }
        }
        allChars.OrderBy(x => (int)x);
        string xmlString = "<CharacterRegions>\n";
        string regionStart = "<CharacterRegion>\n";
        string regionEnd = "</CharacterRegion>\n";
        string startString = "<Start>";
        string closeStart = "</Start>\n";
        string endString = "<End>";
        string closeEnd = "</End>\n";
        foreach (char c in allChars)
        {
            if (c == allChars.First.Value)
            {
                //Must start a region, as is first character.
                xmlString += regionStart +
                    startString + wrap(c) + closeStart;
            }


            if (c != allChars.First.Value)
                if (allChars.Find(c).Previous.Value != c - 1)
                {
                    xmlString += regionStart +
                        startString + wrap(c) + closeStart;
                }


            if (c == allChars.Last.Value)
            {
                //Record value on end and close.
                xmlString += endString + wrap(c) + closeEnd +
                    regionEnd;
            }

            if (c != allChars.Last.Value)
                if (allChars.Find(c).Next.Value != c + 1)
                {
                    //Record value on end and close.
                    xmlString += endString + wrap(c) + closeEnd +
                        regionEnd;
                }

        }
        //End region of characters.
        xmlString += "</CharacterRegions>";
        //output & pause
        Console.WriteLine(xmlString);
        Console.ReadLine();
    }
    static string wrap(char c) => $"&#{(int)c};";

}