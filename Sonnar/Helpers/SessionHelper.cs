using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Alexa.NET.Request;

namespace Sonnar.Helpers
{
    class SessionHelper
    {
        static Session session = Core.Core.Input.Session;


        public static void Set(string key, object val)
        {
            if (session == null)
            {
                session = new Alexa.NET.Request.Session();
            }
            if (session.Attributes == null)
            {
                session.Attributes = new Dictionary<string, object>();
            }

            string jsonString = JsonConvert.SerializeObject(val);

            if (session.Attributes.ContainsKey(key))
            {
                session.Attributes[key] = jsonString;
            }
            else
            {
                session.Attributes.Add(key, jsonString);
            }
        }


        public static T Get<T>(string key)
        {
            if (session.Attributes.ContainsKey(key))
            {
                T output = JsonConvert.DeserializeObject<T>(session.Attributes[key].ToString());
                return output;
            }
            else
            {
                throw new NullReferenceException("The given key " + key + " does not exist in the session attribute.");
            }
        }


        public static bool Contains(string key)
        {
            return session.Attributes.ContainsKey(key);
        }
    }
}
