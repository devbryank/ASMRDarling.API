using Alexa.NET;
using Alexa.NET.Response;
using Alexa.NET.Response.APL;
using Alexa.NET.Response.Directive;
using System.Collections.Generic;

namespace Sonnar.Components
{
    class Response
    {
        protected SkillResponse Skill { get; private set; }


        public Response()
        {
            Skill = new SkillResponse();
            Skill = ResponseBuilder.Empty();
        }


        public SkillResponse GetResponse() { return Skill; }


        public void SetSpeech(bool isSsml, bool endSession, string text)
        {
            if (Core.State.UserState.NumReprompt > 5)
            {
                Core.State.UserState.NumReprompt = 0;
                endSession = true;
            }
            Skill.Response.ShouldEndSession = endSession;
            Skill.Response.OutputSpeech = isSsml ? new SsmlOutputSpeech { Ssml = text } as IOutputSpeech : new PlainTextOutputSpeech { Text = text } as IOutputSpeech;
        }


        public void AddAudioPlayer(PlayBehavior behavior, string url, string token)
        {
            RemoveDirective();
            Skill = ResponseBuilder.AudioPlayerPlay(behavior, url, token);
            Skill.Response.ShouldEndSession = true;

        }


        public void AddAudioPlayer(PlayBehavior behavior, string url, string token, int offset)
        {
            RemoveDirective();
            Skill = ResponseBuilder.AudioPlayerPlay(behavior, url, token, offset);
            Skill.Response.ShouldEndSession = true;

        }


        public void AddAudioPlayer(PlayBehavior behavior, string url, string enqueuedToken, string token, int offset)
        {
            RemoveDirective();
            Skill = ResponseBuilder.AudioPlayerPlay(behavior, url, enqueuedToken, token, offset);
            Skill.Response.ShouldEndSession = true;

        }


        public void ClearAudioPlayer()
        {
            Skill = ResponseBuilder.AudioPlayerClearQueue(ClearBehavior.ClearEnqueued);
        }


        public void StopAudioPlayer()
        {
            Skill = ResponseBuilder.AudioPlayerStop();
        }


        public void AddVideoApp(string url, string title, string subtitle)
        {
            RemoveDirective();

            VideoAppDirective videoAppDirective = new VideoAppDirective
            {
                VideoItem = new VideoItem(url)
                {
                    Metadata = new VideoItemMetadata
                    {
                        Title = title,
                        Subtitle = subtitle
                    }
                }
            };

            Skill.Response.ShouldEndSession = null;
            Skill.Response.Directives = new List<IDirective> { videoAppDirective };
        }


        public void AddAplPage(string token, Layout layout)
        {
            RenderDocumentDirective renderDocumentDirective = new RenderDocumentDirective
            {
                Token = token,
                Document = new APLDocument { MainTemplate = layout }
            };

            Skill.Response.Directives.Add(renderDocumentDirective);
        }


        public void AddDirective(IDirective directive)
        {
            RemoveDirective();
            Skill.Response.Directives.Add(directive);
        }


        public void RemoveDirective()
        {
            if (Skill.Response.Directives == null)
                return;

            List<IDirective> newDirectives = new List<IDirective>();
            foreach (var directive in Skill.Response.Directives)
            {
                if (
                    directive.GetType() != typeof(VideoAppDirective) &&
                    directive.GetType() != typeof(RenderDocumentDirective) &&
                    directive.GetType() != typeof(AudioPlayerPlayDirective) &&
                    directive.GetType() != typeof(ExecuteCommandsDirective))

                    newDirectives.Add(directive);
            }

            Skill.Response.Directives = newDirectives;
        }
    }
}
