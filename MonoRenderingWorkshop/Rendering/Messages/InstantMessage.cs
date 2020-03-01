using System;

namespace MonoRenderingWorkshop.Rendering.Messages
{
    internal sealed class InstantMessage : Message
    {
        public override int SortOrder => 0;

        public InstantMessage(string message) : base(message)
        {
        }

        public override bool Expired(TimeSpan time) => true;
    }
}