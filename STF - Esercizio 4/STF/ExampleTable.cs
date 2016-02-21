using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STF
{
    public class ExampleTable : Table
    {
        public readonly List<string> ArgNames = new List<string>();
        public readonly List<string> ArgTypes = new List<string>();

        public ExampleTable(string fixtureName, List<string> argNames, List<string> argTypes)
            : base(fixtureName)
        {
            this.ArgNames = argNames;
            this.ArgTypes = argTypes;
        }

        /* Code Generation */

        private const string codeTemplate =
            "using STF;\n\npublic class $FixtureName$ : ColumnFixture { \n" +
            "$Definitions$" +
            "\tpublic $ResultType$ result() { \n\t\t /* Insert code here */ \n\t} \n" +
            "\tpublic override bool Check(Row row) { \n$CheckBody$" +
            "\t\treturn (result() == ($ResultType$)row[$LastIndex$]);\n" + "\t}\n} \n";

        public override string GenerateCode()
        {
            string definitions = string.Empty;
            string checkBody = string.Empty;

            for (int i = 0; i < ArgNames.Count - 1; i++)
            {
                definitions += "\tpublic " + ArgTypes[i] + " " + ArgNames[i] + "; \n";
                checkBody += "\t\tthis." + ArgNames[i] + " = (" + ArgTypes[i] + ")row[" +
                    i + "]; \n";
            }
            return codeTemplate
                .Replace("$FixtureName$", this.FixtureName)
                .Replace("$ResultType$", this.ArgTypes[this.ArgTypes.Count - 1])
                .Replace("$Definitions$", definitions)
                .Replace("$CheckBody$", checkBody)
                .Replace("$LastIndex$", (this.ArgNames.Count - 1).ToString())
                .Replace("\n", "\r\n");
        }

        /* HTML Generation */

        private const string htmlTemplate =
                "<table border=\"1\" style=\"border-collapse:collapse;\">"+
                "\n\t<tr><td>$FixtureName$</td></tr>\n" +
                "\t<tr>$Names$</tr>\n\t<tr>$Types$</tr>\n$Rows$</table>\n";
        private const string styleTrue = " style=\"background-color:lime;\"";
        private const string styleFalse = " style=\"background-color:red;\"";

        public override string GetHTML(List<bool> outcomes)
        {
            string names = string.Empty;
            string types = string.Empty;
            string rows = string.Empty;

            for (int i = 0; i < ArgNames.Count; i++)
            {
                names += "<td>" + ArgNames[i] + "</td> ";
                types += "<td>" + ArgTypes[i] + "</td> ";
            }
            int j = 0;
            foreach (Row r in this)
            {
                rows += "\t<tr>";
                int i = 0;
                foreach (object kv in r)
                {
                    rows += "<td" + ((i == r.Count - 1) ? (outcomes[j] ? styleTrue : styleFalse) : "") +
                        ">" + kv.ToString() + "</td> ";
                    i++;
                }
                rows += "</tr>\n";
                j++;
            }
            return htmlTemplate
                .Replace("$FixtureName$", this.FixtureName)
                .Replace("$Names$", names)
                .Replace("$Types$", types)
                .Replace("$Rows$", rows)
                .Replace(",", ".");
        }
    }
}
