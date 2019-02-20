using System.Collections.Generic;

namespace Sonnar.Models
{
    class MediaItems
    {
        public static List<MediaItem> GetMediaItems()
        {
            List<MediaItem> mediaItems = new List<MediaItem>
            {
                new MediaItem() {
                    Id = 1,
                    Views = 0,
                    Title = "What is ASMR",
                    Length = "0",
                    FileName = "whatisasmr",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/whatisasmr.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/whatisasmr.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/whatisasmr.mp4"
                },

                new MediaItem() {
                    Id = 2,
                    Views = 0,
                    Title = "10 Triggers to Help You Sleep",
                    Length = "0",
                    FileName = "10triggerstohelpyousleep",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/10triggerstohelpyousleep.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/10triggerstohelpyousleep.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/10triggerstohelpyousleep.mp4"
                },

                new MediaItem() {
                    Id = 3,
                    Views = 0,
                    Title = "20 Triggers to Help You Sleep",
                    Length = "0",
                    FileName = "20triggerstohelpyousleep",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/20triggerstohelpyousleep.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/20triggerstohelpyousleep.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/20triggerstohelpyousleep.mp4"
                },

                new MediaItem() {
                    Id = 4,
                    Views = 0,
                    Title = "100 Triggers to Help You Sleep",
                    Length = "0",
                    FileName = "100triggerstohelpyousleep",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/100triggerstohelpyousleep.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/100triggerstohelpyousleep.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/100triggerstohelpyousleep.mp4"
                },

                new MediaItem() {
                    Id = 5,
                    Views = 0,
                    Title = "A to Z Triggers to Help You Sleep",
                    Length = "0",
                    FileName = "atoztriggerstohelpyousleep",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/atoztriggerstohelpyousleep.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/atoztriggerstohelpyousleep.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/atoztriggerstohelpyousleep.mp4"
                },

                new MediaItem() {
                    Id = 6,
                    Views = 0,
                    Title = "Brushing the Microphone",
                    Length = "0",
                    FileName = "brushingthemicrophone",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/brushingthemicrophone.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/brushingthemicrophone.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/brushingthemicrophone.mp4"
                },

                new MediaItem() {
                    Id = 7,
                    Views = 0,
                    Title = "Relaxing Head Massage",
                    Length = "0",
                    FileName = "relaxingheadmassage",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/relaxingheadmassage.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/relaxingheadmassage.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/relaxingheadmassage.mp4"
                },

                new MediaItem() {
                    Id = 8,
                    Views = 0,
                    Title = "Relaxing Scalp Massage",
                    Length = "0",
                    FileName = "relaxingscalpmassage",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/relaxingscalpmassage.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/relaxingscalpmassage.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/relaxingscalpmassage.mp4"
                },

                new MediaItem() {
                    Id = 9,
                    Views = 0,
                    Title = "Whispered Tapping and Scratching",
                    Length = "0",
                    FileName = "whisperedtappingandscratching",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/whisperedtappingandscratching.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/whisperedtappingandscratching.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/whisperedtappingandscratching.mp4"
                },

                new MediaItem() {
                    Id = 10,
                    Views = 0,
                    Title = "Close Up Personal Attention For You to Sleep",
                    Length = "0",
                    FileName = "closeuppersonalattentionforyoutosleep",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/closeuppersonalattentionforyoutosleep.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/closeuppersonalattentionforyoutosleep.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/closeuppersonalattentionforyoutosleep.mp4"
                }
            };

            return mediaItems;
        }
    }
}
