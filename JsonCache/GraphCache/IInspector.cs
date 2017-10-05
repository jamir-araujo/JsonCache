namespace GraphCache
{
    public delegate string Found<T>(T value);
    public delegate void DependencyFound<T>(T value, Dependency<T> dependency);

    public interface IInspector<T> where T : class
    {
        void Inspect(T value, Found<T> found, DependencyFound<T> dependencyFound);
    }
}
