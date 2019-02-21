using System.Collections.Generic;

using Alexa.NET;
using Alexa.NET.Response;
using Alexa.NET.Response.APL;
using Alexa.NET.Response.Directive;

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


        public SkillResponse GetResponse()
        {
            return Skill;
        }


        public void SetAskSpeech(string text)
        {
            PlainTextOutputSpeech speech = new PlainTextOutputSpeech { Text = text };
            Skill.Response.ShouldEndSession = false;
            Skill.Response.OutputSpeech = speech;
        }


        public void SetTellSpeech(string text)
        {
            PlainTextOutputSpeech speech = new PlainTextOutputSpeech { Text = text };
            Skill.Response.ShouldEndSession = true;
            Skill.Response.OutputSpeech = speech;
        }


        public void AddAudioPlayer(PlayBehavior behavior, string url, string token)
        {
            RemoveDirective();
            Skill = ResponseBuilder.AudioPlayerPlay(behavior, url, token);
        }


        public void AddAudioPlayer(PlayBehavior behavior, string url, string token, int offset)
        {
            RemoveDirective();
            Skill = ResponseBuilder.AudioPlayerPlay(behavior, url, token, offset);
        }


        public void AddAudioPlayer(PlayBehavior behavior, string url, string enqueuedToken, string token, int offset)
        {
            RemoveDirective();
            Skill = ResponseBuilder.AudioPlayerPlay(behavior, url, enqueuedToken, token, offset);
        }


        public void StopAudioPlayer()
        {
            Skill = ResponseBuilder.AudioPlayerStop();
        }


        public void AddVideoApp(string url, string title, string subtitle)
        {
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
            {
                return;
            }

            List<IDirective> newDirectives = new List<IDirective>();
            foreach (var directive in Skill.Response.Directives)
            {
                if (directive.GetType() != typeof(RenderDocumentDirective) && directive.GetType() != typeof(ExecuteCommandsDirective) && directive.GetType() != typeof(AudioPlayerPlayDirective))
                    newDirectives.Add(directive);
            }

            Skill.Response.Directives = newDirectives;
        }


        public void ClearQueue()
        {
            Skill = ResponseBuilder.AudioPlayerClearQueue(ClearBehavior.ClearEnqueued);
        }
    }
}
