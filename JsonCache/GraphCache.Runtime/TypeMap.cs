using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GraphCache.Runtime
{
    public interface ITypeMapFactory
    {
        TypeMap Create(Type type);
    }

    public class TypeMapFactory : ITypeMapFactory
    {
        private readonly ConcurrentDictionary<Type, TypeMap> _cache;
        private readonly Func<Type, TypeMap> _createTypeMap;
        private readonly Func<PropertyInfo, bool> _propertyFilter;
        private readonly IPropertyAccessorFactory _propertyAccessorFactory;

        public TypeMapFactory(IPropertyAccessorFactory propertyAccessorFactory, Func<PropertyInfo, bool> propertyFilter)
        {
            _propertyAccessorFactory = propertyAccessorFactory;
            _propertyFilter = propertyFilter;

            _cache = new ConcurrentDictionary<Type, TypeMap>();
            _createTypeMap = CreateTypeMap;
        }

        public TypeMap Create(Type type)
        {
            return _cache.GetOrAdd(type, _createTypeMap);
        }

        private TypeMap CreateTypeMap(Type type)
        {
            var propertyList = new List<Property>();
            var properties = type.GetTypeInfo().GetProperties();
            for (int index = 0; index < properties.Length; index++)
            {
                var property = properties[index];
                if (DefaultPropertyFilter(property) && _propertyFilter(property))
                {
                    propertyList.Add(CreateProperty(property));
                }
            }

            return new TypeMap(type, propertyList);
        }

        private Property CreateProperty(PropertyInfo property)
        {
            return new Property(property, _propertyAccessorFactory.Create(property));
        }

        private static bool DefaultPropertyFilter(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.IsPrimitive)
            {
                return false;
            }

            if (propertyInfo.CanRead && propertyInfo.CanWrite)
            {
                return false;
            }

            if (propertyInfo.PropertyType == typeof(string))
            {
                return false;
            }

            return true;
        }
    }

    public class TypeMap
    {
        public Type Type { get; }

        public IReadOnlyList<Property> Properties { get; }

        public TypeMap(Type type, List<Property> properties)
        {
            Type = type;
            Properties = properties;
        }
    }

    public class Property
    {
        public PropertyInfo PropertyInfo { get; }
        public PropertyAccessor PropertyAccessor { get; }

        public Property(PropertyInfo propertyInfo, PropertyAccessor propertyAccessor)
        {
            PropertyInfo = propertyInfo;
            PropertyAccessor = propertyAccessor;
        }
    }
}
