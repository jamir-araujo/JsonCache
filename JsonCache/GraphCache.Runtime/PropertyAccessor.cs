namespace GraphCache.Runtime
{
    public abstract class PropertyAccessor
    {
        public abstract object GetValue(object owner);
        public abstract void SetValue(object owner, object value);
    }
}
