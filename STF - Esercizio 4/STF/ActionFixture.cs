using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STF
{
    public abstract class ActionFixture : Fixture
    {
        public abstract bool Run();

        public override string Execute(Table table)
        {
            return table.GetHTML(new List<bool>() { this.Run() });
        }
    }
}
