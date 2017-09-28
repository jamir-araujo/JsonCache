using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;

namespace JsonCache
{
    public class Cache<T> where T : class
    {
        private IValueInspector<T> _inspector;
        private readonly IMemoryCache _memoryCache;

        public Cache(IMemoryCache memoryCache, IValueInspector<T> inspector)
        {
            _inspector = inspector;
            _memoryCache = memoryCache;
        }

        public void Set(T value, TimeSpan expiry)
        {
            _inspector.InspectObject(value, Found, (foundValeu, dependency) =>
            {
                if (FitsConvention(value))
                {
                    var key = CreateKey(value);

                    StoreKeyDependency(key, dependency, expiry);
                }
            });
        }

        private string Found(T value)
        {
            if (FitsConvention(value))
            {
                var key = CreateKey(value);
                _memoryCache.Set(key, value);

                UpdateDependencies(key, value);

                return string.Empty;
            }
            else
            {
                return null;
            }
        }

        private void StoreKeyDependency(string key, Dependency<T> dependency, TimeSpan expiry)
        {
            var dependencyKey = CreateKeyForDependencies(key);

            var dependencies = _memoryCache.Get<ConcurrentDictionary<string, Dependency<T>>>(dependencyKey);
            if (dependencies != null && !dependencies.IsEmpty)
            {
                dependencies[key] = dependency;
            }
            else
            {
                dependencies = new ConcurrentDictionary<string, Dependency<T>>()
                {
                    [dependency.Key] = dependency
                };

                _memoryCache.Set(dependencyKey, dependencies);
            }
        }

        private void UpdateDependencies(string key, T value)
        {
            var dependencyKey = CreateKeyForDependencies(key);

            var dependencies = _memoryCache.Get<ConcurrentDictionary<string, Dependency<T>>>(dependencyKey);
            if (dependencies != null && !dependencies.IsEmpty)
            {
                foreach (var kvp in dependencies)
                {
                    var dependency = kvp.Value;

                    var owner = _memoryCache.Get<T>(dependency.Key);
                    if (owner != null)
                    {
                        dependency.SetValeu(owner, value);
                    }
                }
            }
        }

        private string CreateKeyForDependencies(string key) => string.Empty;

        private string CreateKey(T value) => string.Empty;

        private bool FitsConvention(T value) => true;
    }
}
