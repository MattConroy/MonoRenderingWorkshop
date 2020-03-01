using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoRenderingWorkshop.MonoGame;
using MonoRenderingWorkshop.Rendering.Messages;
using MonoRenderingWorkshop.Rendering.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoRenderingWorkshop.Rendering
{
    internal sealed class UserInterface
    {
        private const float Spacing = 18;

        private readonly IList<Message> _messages;
        private readonly SpriteBatch _spriteBatch;

        private readonly SpriteFont _arial;
        private double _totalFps = 60;

        public UserInterface(SpriteBatch spriteBatch, ContentManager content)
        {
            _messages = new List<Message>();
            _spriteBatch = spriteBatch;

            _arial = content.Load<SpriteFont>("Arial");
        }

        public void Debug(string message) =>
            _messages.Add(new InstantMessage(message));

        public void Debug(string message, TimeSpan expirationTime) =>
            _messages.Add(new TimedMessage(message, expirationTime));

        public void Debug(string message, GameTime time) =>
            Debug(message, time.TotalGameTime + TimeSpan.FromSeconds(2));

        public void Update(GameTime gameTime)
        {
            for (var i = 0; i < _messages.Count; i++)
            {
                if (_messages[i].Expired(gameTime.TotalGameTime))
                {
                    _messages.RemoveAt(i);
                    --i;
                }
            }
        }

        public void Draw(GameTime time, Renderer renderer)
        {
            const float smoothing = 0.99f;
            var fps = 1.0f / time.GetDeltaTime();
            _totalFps = _totalFps * smoothing + fps * (1.0f - smoothing);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_arial, $"FPS: {_totalFps:N1}", Vector2.Zero, Color.Fuchsia);

            var screenPosition = Vector2.Zero;
            screenPosition.Y += Spacing;

            foreach (var message in _messages.OrderBy(message => message))
            {
                _spriteBatch.DrawString(_arial, message.ToString(), screenPosition, Color.Fuchsia);
                screenPosition.Y += Spacing;
            }

            renderer.DrawDebug(_spriteBatch);

            _spriteBatch.End();
        }
    }
}