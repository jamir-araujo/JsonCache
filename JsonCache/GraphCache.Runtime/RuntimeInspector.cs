using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace GraphCache.Runtime
{
    public class RuntimeInspector : IInspector<object>
    {
        private readonly IPropertyAccessorFactory _propertyAccessorFactory;

        public RuntimeInspector(IPropertyAccessorFactory propertyAccessorFactory)
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
                    InspectInternal(list[index], found, dependencyFound, workingItems, dependency);
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
            var type = value.GetType();
            var properties = type.GetProperties();

            for (int index = 0; index < properties.Length; index++)
            {
                var property = properties[index];

                if (IsValidProperty(property))
                {
                    var propertyAccessor = _propertyAccessorFactory.Create(property);
                    var propertyValue = propertyAccessor.GetValue(value);

                    if (propertyValue is IList list)
                    {
                        InspectListProperty(list, propertyAccessor, found, dependencyFound, workingItems, dependency, key);
                    }
                    else
                    {
                        InspectProperty(propertyValue, propertyAccessor, found, dependencyFound, workingItems, dependency, key);
                    }
                }
            }
        }

        private void InspectListProperty(
            IList propertyValue,
            PropertyAccessor accessor,
            Found<object> found,
            DependencyFound<object> dependencyFound,
            ISet<object> workingItems,
            Dependency<object> dependency,
            string key)
        {
            for (int index = 0; index < propertyValue.Count; index++)
            {
                var value = propertyValue[index];
                var dependencyForIndex = dependency;

                if (dependencyForIndex == null)
                {
                    if (key != null)
                    {
                        dependencyForIndex = new DirectIndexedDependency(accessor, index, key);
                    }
                }
                else
                {
                    dependencyForIndex = new ChainedIndexedDependency(accessor, index, dependencyForIndex);
                }

                InspectInternal(value, found, dependencyFound, workingItems, dependencyForIndex);

                if (dependencyForIndex != null)
                {
                    dependencyFound(value, dependencyForIndex);
                }
            }
        }

        private void InspectProperty(
            object propertyValue,
            PropertyAccessor accessor,
            Found<object> found,
            DependencyFound<object> dependencyFound,
            ISet<object> workingItems,
            Dependency<object> dependency,
            string key)
        {
            if (dependency == null)
            {
                if (key != null)
                {
                    dependency = new DirectDependency(accessor, key);
                }
            }
            else
            {
                dependency = new ChainedDependency(accessor, dependency);
            }

            InspectInternal(propertyValue, found, dependencyFound, workingItems, dependency);

            if (dependency != null)
            {
                dependencyFound(propertyValue, dependency);
            }
        }

        private bool IsValidProperty(PropertyInfo property)
        {
            if (property.PropertyType.IsPrimitive)
            {
                return false;
            }

            if (!property.CanRead || !property.CanWrite)
            {
                return false;
            }

            if (property.PropertyType == typeof(string))
            {
                return false;
            }

            return true;
        }
    }
}
