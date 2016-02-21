using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STF
{
    public class Row : List<object> { }

    public class ExampleRow : Row { }

    public class ActionRow : Row
    {
        public readonly string Action;

        public ActionRow(string action)
        {
            this.Action = action;
        }
    }

}
