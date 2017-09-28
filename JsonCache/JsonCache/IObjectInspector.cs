namespace JsonCache
{
    public abstract class Dependency<T>
    {
        public abstract string Key { get; }

        public abstract T GetValue(T owner);
        public abstract void SetValeu(T owner, object value);
    }

    public delegate string Found<T>(T value);
    public delegate void DependencyFound<T>(T value, Dependency<T> dependency);

    public interface IValueInspector<T> where T : class
    {
        void InspectObject(T value, Found<T> objectFound, DependencyFound<T> dependencyFound);
    }
}
