using Newtonsoft.Json.Linq;
using System;

namespace JsonCache
{
    public class JObjectInspector : IValueInspector<JObject>
    {
        public void InspectObject(JObject value, Found<JObject> objectFound, DependencyFound<JObject> dependencyFound)
        {
            throw new NotImplementedException();
        }
    }
}
