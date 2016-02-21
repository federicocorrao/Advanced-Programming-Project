using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STF
{
    public abstract class Fixture
    {
         public abstract bool Check(Row row);
         public abstract string Execute(Table table);
    }
}

