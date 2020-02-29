using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoRenderingWorkshop.Rendering.Messages;
using System;
using System.Collections.Generic;

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

        public void Draw(float deltaTime)
        {
            const float smoothing = 0.99f;
            var fps = 1.0f / deltaTime;
            _totalFps = _totalFps * smoothing + fps * (1.0f - smoothing);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_arial, $"FPS: {_totalFps:N1}", Vector2.Zero, Color.Black);

            var screenPosition = Vector2.Zero;
            screenPosition.Y += Spacing;

            foreach (var message in _messages)
            {
                _spriteBatch.DrawString(_arial, message.ToString(), screenPosition, Color.Black);
                screenPosition.Y += Spacing;
            }

            _spriteBatch.End();
        }
    }
}