using Alexa.NET.APL.Components;
using Alexa.NET.Response.APL;
using ASMRDarling.API.Models;
using System;
using System.Collections.Generic;

using System.Text;

namespace ASMRDarling.API.Helpers
{
    class PageHelper
    {
        protected Container page = new Container();

        public PageHelper()
        {
            page.Width = DisplayHelper.GetWidth(1f);
            page.Height = DisplayHelper.GetHeight(1f);
            page.Items = new List<APLComponent>();
            page.AlignItems = "center";
        }

        public APLComponent Build()
        {
            //AddBackground();
            AddContent();
            return page;
        }

        protected virtual void AddContent()
        {

        }

        //protected virtual void AddBackground()
        //{
        //    string url;
        //    if (Skill.IsRound)
        //    {
        //        url = ImageMap.Background_480;
        //    }
        //    else
        //    {
        //        url = ImageMap.Background_1080;
        //    }

        //    page.Items.Add(new Image()
        //    {
        //        Source = url,
        //        Scale = "best-fill",
        //        Width = DisplayHelper.GetWidth(1f),
        //        Height = DisplayHelper.GetHeight(1f),
        //        Position = "absolute"
        //    });
        //}


    }
}
