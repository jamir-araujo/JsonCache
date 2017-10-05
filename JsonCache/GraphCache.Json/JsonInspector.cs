using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace GraphCache
{
    public class JsonInspector : IInspector<JContainer>
    {
        public void Inspect(JContainer value, Found<JContainer> found, DependencyFound<JContainer> dependencyFound)
        {
            InspectInternal(value, found, dependencyFound, new HashSet<JContainer>(), null);
        }

        private void InspectInternal(
            JContainer value,
            Found<JContainer> found,
            DependencyFound<JContainer> dependencyFound,
            ISet<JContainer> workingItems,
            Dependency<JContainer> dependency)
        {
            if (value == null && !workingItems.Add(value))
            {
                return;
            }

            if (value is JArray arrayValue)
            {
                InspectArray(arrayValue, found, dependencyFound, new HashSet<JContainer>(), dependency);
            }
            else
            {
                var key = found(value);

                if (!key.IsNullOrWhiteSpace())
                {
                    dependency = null;
                }

                InspectProperties(value, found, dependencyFound, new HashSet<JContainer>(), dependency);
            }
        }

        private void InspectArray(JArray arrayValue, Found<JContainer> found, DependencyFound<JContainer> dependencyFound, HashSet<JContainer> hashSet, Dependency<JContainer> dependency)
        {
            throw new NotImplementedException();
        }

        private void InspectProperties(JContainer value, Found<JContainer> found, DependencyFound<JContainer> dependencyFound, HashSet<JContainer> hashSet, Dependency<JContainer> dependency)
        {
            throw new NotImplementedException();
        }
    }
}
