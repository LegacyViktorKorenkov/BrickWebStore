﻿using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace BrickWebStore.Utility
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            var sessionValue = JsonSerializer.Serialize(value);
            session.SetString(key, sessionValue);
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
