using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SHUL
{
    public class Tempalte
    {
        public static Regex[] r = new Regex[3];

        public Tempalte()
        {
            RegexOptions options = RegexOptions.None;
            r[0] = new Regex(@"<%template ([^\[\]\{\}\s]+)%>", options);
            r[1] = new Regex(@"<%loop ((\(([a-zA-Z]+)\) )?)([^\[\]\{\}\s]+) ([^\[\]\{\}\s]+)%>", options);
            r[2] = new Regex(@"<%\/loop%>", options);
        }

        public void PageTemplate()
        {
            foreach (Match m in r[0].Matches("asdasdasdasd<%template sdsfgdsfg)%>asdfasdfsadf"))
            {

            }

        }
    }
}
