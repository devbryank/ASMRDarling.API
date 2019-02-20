using System;
using System.Collections.Generic;

using Alexa.NET.Response;
using Alexa.NET.Response.APL;
using Alexa.NET.APL.Components;

using Sonnar.Components;

namespace Sonnar.Helpers
{
    class DisplayHelper
    {
        public static float BaseDeviceHeight { get; set; } = 1080f;


        public static APLComponent MergeIntoPages(string id, params APLComponent[] components)
        {
            Pager pager = new Pager() { Height = GetHeight(1f), Width = GetWidth(1f) };
            pager.Id = id;
            pager.Navigation = "none";

            List<APLComponent> pages = new List<APLComponent>();
            pager.Items = pages;

            for (int i = 0; i < components.Length; i++)
                pages.Add(components[i]);

            return pager;
        }


        public static RenderDocumentDirective GenerateDirective(string token, params APLComponent[] components)
        {
            var directive = new RenderDocumentDirective()
            {
                Token = token,
                Document = new APLDocument()
            };

            directive.Document.MainTemplate = new Layout().AsMain();
            directive.Document.MainTemplate.Items = new List<APLComponent>();

            for (int i = 0; i < components.Length; i++)
                directive.Document.MainTemplate.Items.Add(components[i]);

            return directive;
        }


        public static string GetWidth(float percentage)
        {
            return Math.Round(percentage * Core.Device.Viewport.PixelWidth) + "px";
        }


        public static string GetWidth(float imageWidth, float imageHeight)
        {
            float ratio = imageHeight / BaseDeviceHeight;
            float scaledImageHeight = ratio * Core.Device.Viewport.PixelHeight;
            float widthToHeight = imageWidth / imageHeight;
            return Math.Round(widthToHeight * scaledImageHeight) + "px";
        }


        public static string GetHeight(float percentage)
        {
            return Math.Round(percentage * Core.Device.Viewport.PixelHeight) + "px";
        }


        public static string GetHeight(float imageWidth, float imageHeight)
        {
            return GetHeight(imageHeight / BaseDeviceHeight);
        }


        public static int GetPixelWidth(float percentage)
        {
            return (int)Math.Round(percentage * Core.Device.Viewport.PixelWidth);
        }


        public static int GetPixelWidth(float imageWidth, float imageHeight)
        {
            float ratio = imageHeight / BaseDeviceHeight;
            float scaledImageHeight = ratio * Core.Device.Viewport.PixelHeight;
            float widthToHeight = imageWidth / imageHeight;
            return (int)Math.Round(widthToHeight * scaledImageHeight);
        }


        public static int GetPixelHeight(float percentage)
        {
            return (int)Math.Round(percentage * Core.Device.Viewport.PixelHeight);
        }


        public static int GetPixelHeight(float imageWidth, float imageHeight)
        {
            float ratio = imageHeight / BaseDeviceHeight;
            return (int)Math.Round(ratio * Core.Device.Viewport.PixelHeight);
        }
    }
}
