using System.Collections.Generic;

namespace ASMRDarling.API.Models
{
    public class MediaItems
    {
        public static List<MediaItem> GetMediaItems()
        {
            List<MediaItem> mediaItems = new List<MediaItem>
            {
                new MediaItem() {
                    Id = 1,
                    Title = "What is ASMR",
                    FileName = "whatisasmr",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/whatisasmr.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/whatisasmr.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/whatisasmr.mp4"
                },

                new MediaItem() {
                    Id = 2,
                    Title = "10 Triggers to Help You Sleep",
                    FileName = "10triggerstohelpyousleep",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/10triggerstohelpyousleep.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/10triggerstohelpyousleep.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/10triggerstohelpyousleep.mp4"
                },

                new MediaItem() {
                    Id = 3,
                    Title = "20 Triggers to Help You Sleep",
                    FileName = "20triggerstohelpyousleep",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/20triggerstohelpyousleep.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/20triggerstohelpyousleep.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/20triggerstohelpyousleep.mp4"
                },

                new MediaItem() {
                    Id = 4,
                    Title = "100 Triggers to Help You Sleep",
                    FileName = "100triggerstohelpyousleep",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/100triggerstohelpyousleep.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/100triggerstohelpyousleep.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/100triggerstohelpyousleep.mp4"
                },

                new MediaItem() {
                    Id = 5,
                    Title = "A to Z Triggers to Help You Sleep",
                    FileName = "atoztriggerstohelpyousleep",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/atoztriggerstohelpyousleep.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/atoztriggerstohelpyousleep.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/atoztriggerstohelpyousleep.mp4"
                },

                new MediaItem() {
                    Id = 6,
                    Title = "Brushing the Microphone",
                    FileName = "brushingthemicrophone",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/brushingthemicrophone.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/brushingthemicrophone.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/brushingthemicrophone.mp4"
                },

                new MediaItem() {
                    Id = 7,
                    Title = "Relaxing Head Massage",
                    FileName = "relaxingheadmassage",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/relaxingheadmassage.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/relaxingheadmassage.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/relaxingheadmassage.mp4"
                },

                new MediaItem() {
                    Id = 8,
                    Title = "Relaxing Scalp Massage",
                    FileName = "relaxingscalpmassage",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/relaxingscalpmassage.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/relaxingscalpmassage.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/relaxingscalpmassage.mp4"
                },

                new MediaItem() {
                    Id = 9,
                    Title = "Whispered Tapping and Scratching",
                    FileName = "whisperedtappingandscratching",
                    Thumbnail = "https://s3.amazonaws.com/asmr-darling-api-media/png/whisperedtappingandscratching.png",
                    AudioSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp3/whisperedtappingandscratching.mp3",
                    VideoSource = "https://s3.amazonaws.com/asmr-darling-api-media/mp4/whisperedtappingandscratching.mp4"
                },

                new MediaItem() {
                    Id = 10,
                    Title = "Close Up Personal Attention For You to Sleep",
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
