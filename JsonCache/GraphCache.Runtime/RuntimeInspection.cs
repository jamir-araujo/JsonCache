using System;
using System.Collections;
using System.Collections.Generic;

namespace GraphCache.Runtime
{
    public class RuntimeInspection : IInspector<object>
    {
        private readonly IPropertyAccessorFactory _propertyAccessorFactory;

        public RuntimeInspection(IPropertyAccessorFactory propertyAccessorFactory)
        {
            _propertyAccessorFactory = propertyAccessorFactory;
        }

        public void Inspect(object value, Found<object> found, DependencyFound<object> dependencyFound)
        {
            InspectInternal(value, found, dependencyFound, new HashSet<object>(), null);
        }

        private void InspectInternal(
            object value,
            Found<object> found,
            DependencyFound<object> dependencyFound,
            ISet<object> workingItems,
            Dependency<object> dependency)
        {
            if (value == null && !workingItems.Add(value))
            {
                return;
            }

            if (value is IList list)
            {
                for (int index = 0; index < list.Count; index++)
                {
                    InspectInternal(value, found, dependencyFound, workingItems, dependency);
                }
            }
            else
            {
                var key = found(value);

                if (value != null)
                {
                    dependency = null;
                }

                InspectProperties(value, found, dependencyFound, workingItems, dependency, key);
            }
        }

        private void InspectProperties(
            object value, 
            Found<object> found, 
            DependencyFound<object> dependencyFound, 
            ISet<object> workingItems, 
            Dependency<object> dependency, 
            string key)
        {
            throw new NotImplementedException();
        }
    }
}
