using System;
using System.Collections.Generic;
using System.Text;

namespace GraphCache
{
    public interface IConvention<T>
    {
        bool FitsConvetion(T value);
        string CreateKey(T value);
    }
}
