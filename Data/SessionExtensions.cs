namespace CheckApiWeb.Data
{
    using System.Text.Json;

    public static class SessionExtensions
    {
        // Extension method to get data from the session
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        // Extension method to set data in the session
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
    }
}
