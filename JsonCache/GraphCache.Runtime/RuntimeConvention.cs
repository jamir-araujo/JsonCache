using System;
using System.Collections.Generic;
using System.Text;

namespace GraphCache.Runtime
{
    public class RuntimeConvention : IConvention<object>
    {
        public string CreateKey(object value)
        {
            throw new NotImplementedException();
        }

        public bool FitsConvetion(object value)
        {
            throw new NotImplementedException();
        }
    }
}
