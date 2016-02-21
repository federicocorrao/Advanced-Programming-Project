using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace STF
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser parser = new Parser();

            // string input = System.IO.File.ReadAllText("_input_columnfixture.txt");
            // Table etab = parser.Parse<ColumnFixture>(input);
            // string code = etab.GenerateCode();
            // Product product = new Product();
            // string output_html = product.Execute(etab);
            
            string input = System.IO.File.ReadAllText("_input_actionfixture.txt");
            Table table = (new Parser()).Parse<ActionFixture>(input);
            string code = table.GenerateCode();
            
            Fixture a = new Action();
            string output_html = a.Execute(table);

            Console.Read();
        }

    }
}
