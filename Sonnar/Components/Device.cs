using Alexa.NET.APL;
using Alexa.NET.Request;
using Sonnar.Helpers;
using System.Collections.Generic;

namespace Sonnar.Components
{
    class Device
    {
        protected APLSkillRequest Input { get; private set; }


        public Device(APLSkillRequest input)
        {
            Input = input;
            SetDeviceHeight();
            Core.Logger.Write("Device.Device()", $"Display availability: {HasScreen}");
            Core.Logger.Write("Device.Device()", $"Display Height: {DisplayHelper.BaseDeviceHeight}");
            Core.Logger.Write("Device.Device()", $"Display is round: {IsRound}");
        }


        public bool IsRound
        {
            get
            {
                try
                {
                    return Input.Context.Viewport.Shape == ViewportShape.Round;
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
                    return Input.Context.System.Device.IsInterfaceSupported("Alexa.Presentation.APL");
                }
                catch
                {
                    return false;
                }
            }
        }


        public AlexaViewport Viewport
        {
            get
            {
                if (Input.Context.Viewport == null)
                {
                    Input.Context.Viewport = SessionHelper.Get<AlexaViewport>("viewport");
                    return Input.Context.Viewport;
                }
                else
                {
                    if (Input.Session == null)
                        Input.Session = new Session();

                    if (Input.Session.Attributes == null)
                        Input.Session.Attributes = new Dictionary<string, object>();

                    if (!Input.Session.Attributes.ContainsKey("viewport"))
                        SessionHelper.Set("viewport", Input.Context.Viewport);

                    return Input.Context.Viewport;
                }
            }
        }


        private void SetDeviceHeight()
        {
            DisplayHelper.BaseDeviceHeight = IsRound ? 480f : 1080f;
        }
    }
}
