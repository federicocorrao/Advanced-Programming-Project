using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STF
{
    public class ActionTable : Table
    {
        public ActionTable(string fixtureName) : base(fixtureName) { }

        /* Code Generation */

        private const string codeTemplate =
            "using STF;\n\npublic class $FixtureName$ : ActionFixture \n{ \n" +
            "\tpublic override bool Run()\n\t{\n\t\t$ExecuteBody$\t}\n}\n";

        public override string GenerateCode()
        {
            string executeBody = "float _;\n";

            foreach (ActionRow r in this)
            {
                if (r.Action == "start")
                    executeBody += "\t\t" + r[0] + " " + r[1] + " = new " + r[0] + "();\n";
                else if (r.Action == "call")
                    executeBody += "\t\t_ = " + (((string)r[0] == string.Empty) ? "" : r[0] + ".") +
                        r[1] + "(" + r[2] + ", " + r[3] + ");\n";
                else if (r.Action == "result")
                    executeBody += "\t\t_ = " + (((string)r[0] == string.Empty) ? "" : r[0] + ".") +
                        r[1] + "(_);\n";
                else if (r.Action == "check")
                    executeBody += "\t\treturn (_ == " + r[0] + ");\n";
            }
            return codeTemplate.Replace("$FixtureName$", this.FixtureName)
                .Replace("$ExecuteBody$", executeBody);
        }

        /* HTML Generation */

        private const string htmlTemplate = 
            "<table border=\"1\" style=\"border-collapse:collapse;\">"+
            "\n\t<tr><td>$FixtureName$</td></tr>\n$Rows$</table>\n" ;
        private const string styleTrue = " style=\"background-color:lime;\"";
        private const string styleFalse = " style=\"background-color:red;\"";

        public override string GetHTML(List<bool> outcomes)
        {
            string rows = string.Empty;

            int j = 1;
            foreach (Row r in this)
            {
                rows += "\t<tr><td>" + (r as ActionRow).Action + "</td>";
                int i = 0;
                foreach (object kv in r)
                {
                    string style = string.Empty;
                    if (j == this.Count && i == 0)
                        if (outcomes[0] == true) style = styleTrue;
                        else style = styleFalse;

                    rows += "<td " + style + ">" + kv.ToString() + "</td> ";
                    i++;
                }
                rows += "</tr>\n";
                j++;
            }
            return htmlTemplate
                .Replace("$FixtureName$", this.FixtureName)
                .Replace("$Rows$", rows)
                .Replace(",", ".");
        }
    }
    
}
