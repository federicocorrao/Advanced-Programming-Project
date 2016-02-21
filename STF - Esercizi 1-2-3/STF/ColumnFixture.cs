using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STF
{
    public abstract class ColumnFixture : Fixture
    {
        public override string Execute(Table table)
        {
            List<bool> outcomes = new List<bool>();
            foreach (Row row in table)
                outcomes.Add(this.Check(row));
            return table.GetHTML(outcomes);
        }
    }
}
