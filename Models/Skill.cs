using Alexa.NET.APL;
using Alexa.NET.Request;
using ASMRDarling.API.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASMRDarling.API.Models
{
    class Skill
    {
        public static APLSkillRequest Input { get; set; }
        //public static SaveFile SaveData { get; set; }

        public static bool IsRound
        {
            get
            {
                try
                {
                    return Input.Context.Viewport.Shape == Alexa.NET.APL.ViewportShape.Round;
                }
                catch
                {
                    return false;
                }
            }
        }
        public static bool HasScreen
        {
            get
            {
                try
                {
                    return Input.Context.System.Device.IsInterfaceSupported("Alexa.Presentation.APL");
                }
                catch
                {
                    return false;
                }
            }
        }

        public static AlexaViewport ViewPort
        {
            get
            {
                if (Input.Context.Viewport == null)
                {
                    Input.Context.Viewport = SessionHelper.Get<AlexaViewport>(SessionKeys.Viewport);
                    return Input.Context.Viewport;
                }
                else
                {
                    if (Skill.Input.Session == null)
                    {
                        Skill.Input.Session = new Session();
                    }

                    if (Skill.Input.Session.Attributes == null)
                    {
                        Skill.Input.Session.Attributes = new Dictionary<string, object>();
                    }

                    if (!Skill.Input.Session.Attributes.ContainsKey(SessionKeys.Viewport))
                    {
                        SessionHelper.Set(SessionKeys.Viewport, Input.Context.Viewport);
                    }
                    return Input.Context.Viewport;
                }


            }
        }

    }
}
