using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GraphCache.Runtime
{
    public interface IPropertyAccessorFactory
    {
        PropertyAccessor Create(PropertyInfo propertyInfo);
    }
}
