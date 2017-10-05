using System;
using System.Collections.Generic;
using System.Text;

namespace GraphCache
{
    public class RuntimeObjectInspector : IInspector<object>
    {
        public void Inspect(object value, Found<object> found, DependencyFound<object> dependencyFound)
        {
            throw new NotImplementedException();
        }
    }
}
