using System.Collections.Generic;
using Alexa.NET.APL;
using Alexa.NET.Request;
using Sonnar.Helpers;

namespace Sonnar.Core
{
    public class Device
    {
        APLSkillRequest input;


        public Device()
        {
            Init();
        }


        void Init()
        {
            input = Core.Input;
        }


        public bool IsRound
        {
            get
            {
                try
                {
                    return input.Context.Viewport.Shape == ViewportShape.Round;
                }
                catch
                {
                    return false;
                }
            }
        }


        public bool HasScreen
        {
            get
            {
                try
                {
                    return input.Context.System.Device.IsInterfaceSupported("Alexa.Presentation.APL");
                }
                catch
                {
                    return false;
                }
            }
        }


        public AlexaViewport ViewPort
        {
            get
            {
                if (input.Context.Viewport == null)
                {
                    input.Context.Viewport = SessionHelper.Get<AlexaViewport>("viewport");
                    return input.Context.Viewport;
                }
                else
                {
                    if (input.Session == null)
                    {
                        input.Session = new Session();
                    }

                    if (input.Session.Attributes == null)
                    {
                        input.Session.Attributes = new Dictionary<string, object>();
                    }

                    if (!input.Session.Attributes.ContainsKey("viewport"))
                    {
                        SessionHelper.Set("viewport", input.Context.Viewport);
                    }
                    return input.Context.Viewport;
                }
            }
        }
    }
}
