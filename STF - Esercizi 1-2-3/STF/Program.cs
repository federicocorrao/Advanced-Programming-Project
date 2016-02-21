using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace STF
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = System.IO.File.ReadAllText("_input_fixture.txt");

            Table table = (new Parser()).Parse(input);
            string code = table.GenerateCode();

            Console.WriteLine(code);

            Fixture p = new Product();
            string output = p.Execute(table);

            Console.WriteLine(output);

            Console.Read();
        }

    }
}
