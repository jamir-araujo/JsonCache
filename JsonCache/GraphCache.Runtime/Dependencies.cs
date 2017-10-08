using System;

namespace GraphCache.Runtime
{
    internal class DirectDependency : Dependency<object>
    {
        private readonly PropertyAccessor _property;
        private readonly string _key;

        public override string Key => _key;

        public DirectDependency(PropertyAccessor property, string key)
        {
            _property = property;
            _key = key;
        }

        public override void SetValeu(object owner, object value)
        {
            _property.SetValue(owner, value);
        }

        protected override object GetValue(object owner)
        {
            return _property.GetValue(owner);
        }
    }

    internal class ChainedDependency : Dependency<object>
    {
        private readonly PropertyAccessor _property;
        private readonly Dependency<object> _dependency;

        public override string Key => _dependency.Key;

        public ChainedDependency(PropertyAccessor property, Dependency<object> dependency)
        {
            _property = property;
            _dependency = dependency;
        }

        public override void SetValeu(object owner, object value)
        {
            throw new NotImplementedException();
        }

        protected override object GetValue(object owner)
        {
            throw new NotImplementedException();
        }
    }

    internal class DirectIndexedDependency : Dependency<object>
    {
        private readonly PropertyAccessor _property;
        private readonly int _index;
        private readonly string _key;

        public override string Key => _key;

        public DirectIndexedDependency(PropertyAccessor property, int index, string key)
        {
            _property = property;
            _index = index;
            _key = key;
        }

        public override void SetValeu(object owner, object value)
        {
            throw new NotImplementedException();
        }

        protected override object GetValue(object owner)
        {
            throw new NotImplementedException();
        }
    }

    internal class ChainedIndexedDependency : Dependency<object>
    {
        private readonly PropertyAccessor _property;
        private readonly int _index;
        private readonly Dependency<object> _dependency;

        public override string Key => _dependency.Key;

        public ChainedIndexedDependency(PropertyAccessor property, int index, Dependency<object> dependency)
        {
            _property = property;
            _index = index;
            _dependency = dependency;
        }

        public override void SetValeu(object owner, object value)
        {
            throw new NotImplementedException();
        }

        protected override object GetValue(object owner)
        {
            throw new NotImplementedException();
        }
    }
}
