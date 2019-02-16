using ASMRDarling.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASMRDarling.API.Helpers
{
    class SessionHelper
    {
        public static void Set(string key, object val)
        {
            if (Skill.Input.Session == null)
            {
                Skill.Input.Session = new Alexa.NET.Request.Session();
            }
            if (Skill.Input.Session.Attributes == null)
            {
                Skill.Input.Session.Attributes = new Dictionary<string, object>();
            }

            string jsonString = JsonConvert.SerializeObject(val);

            if (Skill.Input.Session.Attributes.ContainsKey(key))
            {
                Skill.Input.Session.Attributes[key] = jsonString;
            }
            else
            {
                Skill.Input.Session.Attributes.Add(key, jsonString);
            }
        }

        public static T Get<T>(string key)
        {
            if (Skill.Input.Session.Attributes.ContainsKey(key))
            {
                T output = JsonConvert.DeserializeObject<T>(Skill.Input.Session.Attributes[key].ToString());
                return output;
            }
            else
            {
                throw new NullReferenceException("The given key " + key + " does not exist in the session attribute.");
            }

        }

        public static bool Contains(string key)
        {
            return Skill.Input.Session.Attributes.ContainsKey(key);
        }
    }
}
