using System;

namespace MonoRenderingWorkshop.Rendering.Messages
{
    internal abstract class Message
    {
        private readonly string _message;

        protected Message(string message)
        {
            _message = message;
        }

        public abstract bool Expired(TimeSpan time);

        public override string ToString() => _message;
    }
}