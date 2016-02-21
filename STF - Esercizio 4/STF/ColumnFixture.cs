using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STF
{
    public abstract class ColumnFixture : Fixture
    {
        public abstract bool Check(Row r);

        public override string Execute(Table table)
        {
            List<bool> outcomes = new List<bool>();
            foreach (Row r in table)
                outcomes.Add(this.Check(r));
            return table.GetHTML(outcomes);
        }        
    }
}
