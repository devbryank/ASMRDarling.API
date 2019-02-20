using System;
using System.Collections.Generic;
using Newtonsoft.Json;

using Alexa.NET.Request;

using Sonnar.Components;

namespace Sonnar.Helpers
{
    class SessionHelper
    {
        public static void Set(string key, object val)
        {
            Session session = Core.Request.GetSession();

            if (session == null)
                session = new Session();

            if (session.Attributes == null)
                session.Attributes = new Dictionary<string, object>();

            string jsonString = JsonConvert.SerializeObject(val);

            if (session.Attributes.ContainsKey(key))
                session.Attributes[key] = jsonString;
            else
                session.Attributes.Add(key, jsonString);

            Core.Logger.Write("SessionHelper.Set()", $"Session attribute {key} has been set to value: {JsonConvert.SerializeObject(val)}.");
        }


        public static T Get<T>(string key)
        {
            Session session = Core.Request.GetSession();

            if (session.Attributes.ContainsKey(key))
            {
                T output = JsonConvert.DeserializeObject<T>(session.Attributes[key].ToString());
                return output;
            }
            else
            {
                Core.Logger.Write("SessionHelper.Get()", $"Requested session {key} does not exist in the session attributes.");
                throw new NullReferenceException($"[SessionHelper.Get()] Requested session {key} does not exist in the session attributes.");
            }
        }


        public static bool Contains(string key)
        {
            Session session = Core.Request.GetSession();
            return session.Attributes.ContainsKey(key);
        }
    }
}
