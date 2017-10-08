using Newtonsoft.Json.Linq;
using System;

namespace GraphCache.Json
{
    internal class DirectDependency : Dependency<JContainer>
    {
        private readonly string _key;
        private readonly string _propertyName;

        public override string Key => _key;

        public DirectDependency(string propertyName, string key)
        {
            _key = key;
            _propertyName = propertyName;
        }

        public override void SetValeu(JContainer owner, object value)
        {
            throw new NotImplementedException();
        }

        protected override JContainer GetValue(JContainer owner)
        {
            throw new NotImplementedException();
        }
    }

    internal class ChainedDependency : Dependency<JContainer>
    {
        private readonly string _propertyName;
        private readonly Dependency<JContainer> _dependency;

        public override string Key => _dependency.Key;

        public ChainedDependency(string propertyName, Dependency<JContainer> dependency)
        {
            _propertyName = propertyName;
            _dependency = dependency;
        }

        public override void SetValeu(JContainer owner, object value)
        {
            throw new NotImplementedException();
        }

        protected override JContainer GetValue(JContainer owner)
        {
            throw new NotImplementedException();
        }
    }

    internal class DirectIndexedDependency : Dependency<JContainer>
    {
        private readonly string _propertyName;
        private readonly string _key;
        private readonly int _index;

        public override string Key => _key;

        public DirectIndexedDependency(string propertyName, int index, string key)
        {
            _propertyName = propertyName;
            _key = key;
            _index = index;
        }

        public override void SetValeu(JContainer owner, object value)
        {
            throw new NotImplementedException();
        }

        protected override JContainer GetValue(JContainer owner)
        {
            throw new NotImplementedException();
        }
    }

    internal class ChainedIndexedDependency : Dependency<JContainer>
    {
        private readonly string _propertyName;
        private readonly int _index;
        private readonly Dependency<JContainer> _dependency;

        public override string Key => _dependency.Key;

        public ChainedIndexedDependency(string propertyName, int index, Dependency<JContainer> dependency)
        {
            _propertyName = propertyName;
            _index = index;
            _dependency = dependency;
        }

        public override void SetValeu(JContainer owner, object value)
        {
            throw new NotImplementedException();
        }

        protected override JContainer GetValue(JContainer owner)
        {
            throw new NotImplementedException();
        }
    }
}
