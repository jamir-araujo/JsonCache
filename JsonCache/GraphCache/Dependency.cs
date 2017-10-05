using System;
using System.Collections.Generic;
using System.Text;

namespace GraphCache
{
    public abstract class Dependency<T>
    {
        public abstract string Key { get; }

        protected abstract T GetValue(T owner);
        public abstract void SetValeu(T owner, object value);
    }
}
