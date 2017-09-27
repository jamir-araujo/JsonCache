using System;
using System.Collections.Generic;
using System.Text;

namespace JsonCache
{
    public interface IObjectInspector<T> where T : class
    {
        void InspectObject(T value);
    }
}
