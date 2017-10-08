using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace GraphCache.Json
{
    public class JsonInspector : IInspector<JContainer>
    {
        public void Inspect(JContainer value, Found<JContainer> found, DependencyFound<JContainer> dependencyFound)
        {
            InspectInternal(value, found, dependencyFound, new HashSet<JToken>(), null);
        }

        private void InspectInternal(
            JToken value,
            Found<JContainer> found,
            DependencyFound<JContainer> dependencyFound,
            ISet<JToken> workingItems,
            Dependency<JContainer> dependency)
        {
            if (value.IsNull() || !workingItems.Add(value))
            {
                return;
            }

            if (value is JArray jArray)
            {
                for (int i = 0; i < jArray.Count; i++)
                {
                    InspectInternal(jArray[i], found, dependencyFound, workingItems, dependency);
                }
            }
            else if (value is JObject @object)
            {
                var key = found(@object);

                if (!key.IsNullOrWhiteSpace())
                {
                    dependency = null;
                }

                InspectProperties(@object, key, found, dependencyFound, workingItems, dependency);
            }
        }

        private void InspectProperties(
            JObject value,
            string key,
            Found<JContainer> found,
            DependencyFound<JContainer> dependencyFound,
            ISet<JToken> workingItems,
            Dependency<JContainer> dependency)
        {
            foreach (var property in value.Properties())
            {
                if (property.Value is JArray jArray)
                {
                    InspectArrayProperty(jArray, property.Name, key, found, dependencyFound, workingItems, dependency);
                }
                else if (property.Value is JObject @object)
                {
                    InspectProperty(@object, property.Name, key, found, dependencyFound, workingItems, dependency);
                }
            }
        }

        private void InspectProperty(
            JObject value,
            string propertyName,
            string key,
            Found<JContainer> found,
            DependencyFound<JContainer> dependencyFound,
            ISet<JToken> workingItems,
            Dependency<JContainer> dependency)
        {
            if (dependency == null)
            {
                if (key != null)
                {
                    dependency = new DirectDependency(propertyName, key);
                }
            }
            else
            {
                dependency = new ChainedDependency(propertyName, dependency);
            }

            InspectInternal(value, found, dependencyFound, workingItems, dependency);

            if (dependency != null)
            {
                dependencyFound(value, dependency);
            }
        }

        private void InspectArrayProperty(
            JArray propertyValue,
            string propertyName,
            string key,
            Found<JContainer> found,
            DependencyFound<JContainer> dependencyFound,
            ISet<JToken> workingItems,
            Dependency<JContainer> dependency)
        {
            for (int i = 0; i < propertyValue.Count; i++)
            {
                var element = propertyValue[i];

                if (element is JContainer jContainer)
                {
                    var dependencyKeyForIndex = dependency;

                    if (dependencyKeyForIndex == null)
                    {
                        if (key != null)
                        {
                            dependencyKeyForIndex = new DirectIndexedDependency(propertyName, i, key);
                        }
                    }
                    else
                    {
                        dependencyKeyForIndex = new ChainedIndexedDependency(propertyName, i, dependencyKeyForIndex);
                    }

                    InspectInternal(jContainer, found, dependencyFound, workingItems, dependencyKeyForIndex);

                    if (dependencyKeyForIndex != null)
                    {
                        dependencyFound(jContainer, dependencyKeyForIndex);
                    }
                }
            }
        }
    }
}
