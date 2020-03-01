using System;

namespace MonoRenderingWorkshop.Rendering.Messages
{
    internal abstract class Message : IComparable<Message>
    {
        public abstract int SortOrder { get; }

        private readonly string _message;


        protected Message(string message)
        {
            _message = message;
        }

        public int CompareTo(Message other) => 
            SortOrder.CompareTo(other.SortOrder);

        public abstract bool Expired(TimeSpan time);

        public override string ToString() => _message;
    }
}