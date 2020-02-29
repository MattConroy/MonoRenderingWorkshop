using System;

namespace MonoRenderingWorkshop.Rendering.Messages
{
    internal sealed class TimedMessage : Message
    {
        private readonly TimeSpan _expirationTime;

        public TimedMessage(string message, TimeSpan expirationTime) : base(message)
        {
            _expirationTime = expirationTime;
        }

        public override bool Expired(TimeSpan time) =>
            time > _expirationTime;
    }
}