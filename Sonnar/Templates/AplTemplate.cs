using System.Collections.Generic;

using Alexa.NET.APL;
using Alexa.NET.Response.APL;
using Alexa.NET.APL.Commands;
using Alexa.NET.APL.Components;

using Sonnar.Models;
using Sonnar.Helpers;
using Sonnar.Components;

namespace Sonnar.Templates
{
    class AplTemplate
    {
        private static string TextColor = "black";
        private static string TitleTextColor = "black";
        private static string FontWeight = "fontWeightLight";
        private static List<MediaItem> Items = MediaItems.GetMediaItems();


        public static Layout GetSpotMenu()
        {
            float space = 0.1f;
            float width = 1f;
            float height = 1f;
            float titleWidth = 0.3f;
            float titleHeight = 0.2f;
            string fontSize = "30dp";
            int maxLines = 2;
            int itemsPerContainer = 1;

            Sequence sequence = GetSequence(itemsPerContainer, width, height);
            return GetLayout(sequence, fontSize, titleWidth, titleHeight, space, maxLines);
        }


        public static Layout GetShowMenu()
        {
            float space = 0.07f;
            float width = 0.3f;
            float height = 1f;
            float titleWidth = 1f;
            float titleHeight = 0.1f;
            string fontSize = "60dp";
            int maxLines = 3;
            int itemsPerContainer = 2;

            Sequence sequence = GetSequence(itemsPerContainer, width, height);
            return GetLayout(sequence, fontSize, titleWidth, titleHeight, space, maxLines);
        }


        private static int GetContainerSize(int itemsPerContainer)
        {
            return (Items.Count % itemsPerContainer) == 0 ? Items.Count / itemsPerContainer : (Items.Count / itemsPerContainer) + 1;
        }


        private static Sequence GetSequence(int itemsPerContainer, float width, float height)
        {
            int containerSize = GetContainerSize(itemsPerContainer);

            Sequence sequence = new Sequence
            {
                Width = DisplayHelper.GetWidth(1f),
                Height = DisplayHelper.GetHeight(1f),
                ScrollDirection = "horizontal"
            };

            Container[] containers = new Container[containerSize];

            for (int i = 0; i < containerSize; i++)
            {
                List<APLComponent> containerItems = new List<APLComponent>();

                containers[i] = new Container
                {
                    Direction = "column",
                    AlignSelf = "center",
                    AlignItems = "center",
                    Width = DisplayHelper.GetWidth(width),
                    Height = DisplayHelper.GetHeight(height)
                };

                for (int j = 0; j < itemsPerContainer; j++)
                {
                    if ((j + i * itemsPerContainer) < Items.Count)
                    {
                        var wrappedItem = GetTouchWrappedItem(Items[j + i * itemsPerContainer]);
                        containerItems.Add(wrappedItem);
                    }
                }

                containers[i].Items = containerItems;
            }

            List<APLComponent> sequenceItems = new List<APLComponent>();
            foreach (Container c in containers)
                sequenceItems.Add(c);

            sequence.Items = sequenceItems;

            return sequence;
        }


        private static APLComponent GetTouchWrappedItem(MediaItem mediaItem)
        {
            bool isSpot = Core.Device.IsRound;
            Container empty = new Container();

            // touch input interface
            TouchWrapper touchWrapper = new TouchWrapper()
            {
                AlignSelf = "center",
                OnPress = new SendEvent
                {
                    Arguments = new List<string> { mediaItem.Title, mediaItem.VideoSource }
                }
            };

            // component container
            Container container = new Container()
            {
                Direction = "column",
                AlignItems = "center",
                Width = isSpot ? DisplayHelper.GetWidth(1f) : DisplayHelper.GetWidth(0.3f),
                Height = isSpot ? DisplayHelper.GetHeight(0.8f) : DisplayHelper.GetHeight(0.4f),
                Spacing = isSpot ? DisplayHelper.GetHeight(0) : DisplayHelper.GetHeight(0.01f)
            };

            // thumbnail image
            Image thumbnail = new Image(mediaItem.Thumbnail)
            {
                AlignSelf = "center",
                Width = isSpot ? DisplayHelper.GetWidth(0.7f) : DisplayHelper.GetWidth(0.22f),
                Height = isSpot ? DisplayHelper.GetHeight(0.4f) : DisplayHelper.GetHeight(0.2f),
                Spacing = isSpot ? DisplayHelper.GetHeight(0.03f) : DisplayHelper.GetHeight(0.05f),
                BorderRadius = new APLDimensionValue("10px")
            };

            // name of the media
            Text mediaName = new Text(mediaItem.Title)
            {
                MaxLines = 2,
                Color = TextColor,
                FontWeight = FontWeight,
                FontSize = isSpot ? "28dp" : "24dp",
                TextAlign = isSpot ? "center" : "left",
                PaddingLeft = isSpot ? new APLDimensionValue("0px") : new APLDimensionValue("20px"),
                //Left = isSpot ? new AbsoluteDimension(0, "px") : new AbsoluteDimension(25, "px"),
                Width = isSpot ? DisplayHelper.GetWidth(0.8f) : DisplayHelper.GetWidth(0.25f),
                Height = isSpot ? DisplayHelper.GetHeight(0.15f) : DisplayHelper.GetHeight(0.09f),
                Spacing = isSpot ? DisplayHelper.GetHeight(0.02f) : DisplayHelper.GetHeight(0.02f)
            };

            // view count
            Text views = new Text($"{mediaItem.Views} plays")
            {
                Color = TextColor,
                FontWeight = FontWeight,
                FontSize = isSpot ? "20dp" : "20dp",
                TextAlign = isSpot ? "center" : "left",
                PaddingLeft = isSpot ? new APLDimensionValue("0px") : new APLDimensionValue("20px"),
                Height = DisplayHelper.GetHeight(0.08f),  // this does nothing
                Width = isSpot ? DisplayHelper.GetWidth(0.6f) : DisplayHelper.GetWidth(0.25f),
            };

            // show index if spot
            Text index = new Text($"{mediaItem.Id} of {MediaItems.GetMediaItems().Count}")
            {
                Color = TextColor,
                TextAlign = "center",
                FontSize = "18dp",
                Height = DisplayHelper.GetHeight(0.1f),  // this does nothing
            };

            // set components
            container.Items = isSpot ? new List<APLComponent> { thumbnail, mediaName, views, index } : new List<APLComponent> { empty, thumbnail, mediaName, views };
            touchWrapper.Item = new List<APLComponent> { container };

            return touchWrapper;
        }


        private static Layout GetLayout(Sequence sequence, string titleFontSize, float titleWidth, float titleHeight, float space, int maxLines)
        {
            Layout layout = new Layout();
            Container empty = new Container();

            // section container
            Container container = new Container()
            {
                Direction = "column",
                AlignItems = "center",
                Width = DisplayHelper.GetWidth(1f),
                Height = DisplayHelper.GetHeight(1f)
            };

            // background image
            Image background = new Image()
            {
                Position = "absolute",
                Scale = ImageScale.BestFill,
                Width = DisplayHelper.GetWidth(1f),
                Height = DisplayHelper.GetHeight(1f),
                Source = "https://s3.amazonaws.com/asmr-darling-api-media/png/taylordarlingwhite.jpg"
            };

            // title of the skill
            Text title = new Text("ASMR Darling")
            {
                MaxLines = maxLines,
                TextAlign = "center",
                AlignSelf = "center",
                Color = TitleTextColor,
                FontWeight = FontWeight,
                FontSize = titleFontSize,
                Width = DisplayHelper.GetWidth(titleWidth),
                Height = DisplayHelper.GetHeight(titleHeight),
                Spacing = DisplayHelper.GetHeight(space)
            };

            // set components
            container.Items = new List<APLComponent> { empty, background, title, sequence };
            layout.Items = new List<APLComponent> { container };

            return layout;
        }
    }
}
