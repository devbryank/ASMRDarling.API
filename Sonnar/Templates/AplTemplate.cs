using System.Collections.Generic;

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
            float space = 0.05f;
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
            float space = 0f;
            float width = 0.3f;
            float height = 1f;
            float titleWidth = 1f;
            float titleHeight = 0.1f;
            string fontSize = "40dp";
            int maxLines = 1;
            int itemsPerContainer = 3;

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
                    Width = DisplayHelper.GetWidth(width),
                    Height = DisplayHelper.GetHeight(height),
                    Direction = "column",
                    AlignItems = "center",
                    AlignSelf = "center"
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

            TouchWrapper touchWrapper = new TouchWrapper(
                                            new Container(
                                                new Image(mediaItem.Thumbnail)
                                                {
                                                    AlignSelf = "center",
                                                    Width = isSpot ? DisplayHelper.GetWidth(0.7f) : DisplayHelper.GetWidth(0.22f),
                                                    Height = isSpot ? DisplayHelper.GetHeight(0.4f) : DisplayHelper.GetHeight(0.2f),
                                                    Spacing = isSpot ? DisplayHelper.GetHeight(0.05f) : DisplayHelper.GetHeight(0)
                                                },
                                                new Text(mediaItem.Title)
                                                {
                                                    MaxLines = 2,
                                                    Color = TextColor,
                                                    TextAlign = "center",
                                                    FontWeight = FontWeight,
                                                    FontSize = isSpot ? "22dp" : "18dp",
                                                    Width = isSpot ? DisplayHelper.GetWidth(0.6f) : DisplayHelper.GetWidth(0.22f),
                                                    Height = isSpot ? DisplayHelper.GetHeight(0.15f) : DisplayHelper.GetHeight(0.1f),
                                                    Spacing = isSpot ? DisplayHelper.GetHeight(0.02f) : DisplayHelper.GetHeight(0.01f),
                                                }
                                            )
                                            {
                                                Direction = "column",
                                                AlignItems = "center",
                                                Width = isSpot ? DisplayHelper.GetWidth(1f) : DisplayHelper.GetWidth(0.3f),
                                                Height = isSpot ? DisplayHelper.GetHeight(0.8f) : DisplayHelper.GetHeight(0.3f),
                                                Spacing = isSpot ? DisplayHelper.GetHeight(0) : DisplayHelper.GetHeight(0.1f)
                                            }
                                        )
            {
                //Height = isSpot ? DisplayHelper.GetHeight(0.8f) : null,
                //Width = isSpot ? DisplayHelper.GetWidth(1f) : null,
                AlignSelf = "center",
                OnPress = new SendEvent
                {
                    Arguments = new List<string> { mediaItem.Title, mediaItem.VideoSource
}
                }
            };

            return touchWrapper;
        }


        private static Layout GetLayout(Sequence sequence, string titleFontSize, float titleWidth, float titleHeight, float space, int maxLines)
        {
            return new Layout(
                                new Frame(
                              
                                        new Container(
                                            new Container() { },
                                                new Text("ASMR Darling")
                                                {
                                                    Color = TitleTextColor,
                                                    FontWeight = FontWeight,
                                                    FontSize = titleFontSize,
                                                    Width = DisplayHelper.GetWidth(titleWidth),
                                                    Height = DisplayHelper.GetHeight(titleHeight),
                                                    Spacing = DisplayHelper.GetHeight(space),

                                                    AlignSelf = "center",
                                                    TextAlign = "center",
                                                    MaxLines = maxLines
                                                },
                                                sequence
                                            )
                                        {
                                            Width = DisplayHelper.GetWidth(1f),
                                            Height = DisplayHelper.GetHeight(1f),
                                            Direction = "column",
                                            AlignItems = "center"
                                        },
                                          new Image()
                                          {
                                              Source = "https://s3.amazonaws.com/asmr-darling-api-media/png/taylordarlingwhite.jpg",
                                              Scale = ImageScale.BestFill,
                                              Width = DisplayHelper.GetWidth(1f),
                                              Height = DisplayHelper.GetHeight(1f),
                                              Position = "absolute"
                                          }
                                    )
                                { Width = DisplayHelper.GetWidth(1f), Height = DisplayHelper.GetHeight(1f), BackgroundColor = "white", }
                                );
        }
    }
}
