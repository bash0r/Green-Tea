using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenTea
{
    public class TypeScope : Value
    {
        public override GTType Type
        {
            get { return GTType.Scope; }
        }
    }
}
