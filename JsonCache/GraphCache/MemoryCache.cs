using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;

namespace GraphCache
{
    public class MomoryCache<T> where T : class
    {
        private IInspector<T> _inspector;
        private readonly IConvention<T> _convention;
        private readonly IMemoryCache _memoryCache;

        public MomoryCache(IMemoryCache memoryCache, IInspector<T> inspector, IConvention<T> convention)
        {
            _inspector = inspector;
            _convention = convention;
            _memoryCache = memoryCache;
        }

        public void Set(T value, TimeSpan expiry)
        {
            _inspector.Inspect(value, Found, (foundValeu, dependency) =>
            {
                if (FitsConvention(foundValeu))
                {
                    var key = CreateKey(foundValeu);

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

        private string CreateKeyForDependencies(string key) => $"{key} -> Dependencies";

        private string CreateKey(T value) => _convention.CreateKey(value);

        private bool FitsConvention(T value) => _convention.FitsConvetion(value);
    }
}
