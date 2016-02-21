using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STF
{
    public abstract class Table : List<Row>
    {
        public readonly string FixtureName;
       
        public abstract string GenerateCode();
        public abstract string GetHTML(List<bool> outcomes);

        public Table(string fixtureName)
        {
            this.FixtureName = fixtureName;
        }

    }
}
