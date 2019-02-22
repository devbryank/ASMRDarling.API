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
                    Id = 0,
                    Views = "3.1M",
                    Title = "What is ASMR",
                    Length = "7:18",
                    FileName = "whatisasmr",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/whatisasmr.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/whatisasmr.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/whatisasmr.mp4"
                },

                new MediaItem() {
                    Id = 1,
                    Views = "30M",
                    Title = "10 Triggers to Help You Sleep",
                    Length = "29:25",
                    FileName = "10triggerstohelpyousleep",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/10triggerstohelpyousleep.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/10triggerstohelpyousleep.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/10triggerstohelpyousleep.mp4"
                },

                new MediaItem() {
                    Id = 2,
                    Views = "30M",
                    Title = "20 Triggers to Help You Sleep",
                    Length = "56:42",
                    FileName = "20triggerstohelpyousleep",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/20triggerstohelpyousleep.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/20triggerstohelpyousleep.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/20triggerstohelpyousleep.mp4"
                },

                new MediaItem() {
                    Id = 3,
                    Views = "21M",
                    Title = "100 Triggers to Help You Sleep",
                    Length = "4:26:07",
                    FileName = "100triggerstohelpyousleep",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/100triggerstohelpyousleep.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/100triggerstohelpyousleep.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/100triggerstohelpyousleep.mp4"
                },

                new MediaItem() {
                    Id = 4,
                    Views = "22M",
                    Title = "A to Z Triggers to Help You Sleep",
                    Length = "1:52:43",
                    FileName = "atoztriggerstohelpyousleep",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/atoztriggerstohelpyousleep.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/atoztriggerstohelpyousleep.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/atoztriggerstohelpyousleep.mp4"
                },

                new MediaItem() {
                    Id = 5,
                    Views = "7.5M",
                    Title = "Brushing the Microphone",
                    Length = "35:33",
                    FileName = "brushingthemicrophone",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/brushingthemicrophone.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/brushingthemicrophone.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/brushingthemicrophone.mp4"
                },

                new MediaItem() {
                    Id = 6,
                    Views = "7.2M",
                    Title = "Relaxing Head Massage",
                    Length = "23:44",
                    FileName = "relaxingheadmassage",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/relaxingheadmassage.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/relaxingheadmassage.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/relaxingheadmassage.mp4"
                },

                new MediaItem() {
                    Id = 7,
                    Views = "12M",
                    Title = "Relaxing Scalp Massage",
                    Length = "22:50",
                    FileName = "relaxingscalpmassage",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/relaxingscalpmassage.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/relaxingscalpmassage.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/relaxingscalpmassage.mp4"
                },

                new MediaItem() {
                    Id = 8,
                    Views = "11M",
                    Title = "Whispered Tapping and Scratching",
                    Length = "35:06",
                    FileName = "whisperedtappingandscratching",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/whisperedtappingandscratching.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/whisperedtappingandscratching.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/whisperedtappingandscratching.mp4"
                },

                new MediaItem() {
                    Id = 9,
                    Views = "10M",
                    Title = "Close Up Personal Attention For You to Sleep",
                    Length = "29:32",
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
