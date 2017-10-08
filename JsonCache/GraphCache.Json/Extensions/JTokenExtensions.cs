namespace Newtonsoft.Json.Linq
{
    public static class JTokenExtensions
    {
        public static bool IsNull(this JToken jToken) => jToken == null || jToken.Type == JTokenType.Null;
    }
}
