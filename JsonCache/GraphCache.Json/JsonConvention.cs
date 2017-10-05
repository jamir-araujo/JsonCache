using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;

namespace GraphCache.Json
{
    public class JsonConvention : IConvention<JContainer>
    {
        const string SEPARATOR = " -> ";

        private readonly List<string> _properties;

        public JsonConvention(IEnumerable<string> properties)
        {
            _properties = properties.ToList();
        }

        public string CreateKey(JContainer value)
        {
            var builder = new StringBuilder();

            foreach (var property in _properties)
            {
                if (builder.Length != 0)
                {
                    builder.Append(SEPARATOR);
                }

                builder.Append(value[property]);
            }

            return builder.ToString();
        }

        public bool FitsConvetion(JContainer value)
        {
            if (value is JObject objectValue)
            {
                return _properties
                    .All(property => objectValue.TryGetValue(property, out var _));
            }

            return false;
        }
    }
}
