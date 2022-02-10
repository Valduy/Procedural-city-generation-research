using OpenTK.Windowing.Common;

namespace Engine.Infrastructure
{
    public abstract class Component
    {
        public GameObject? GameObject { get; internal set; }

        public virtual void Start() {}
        public virtual void GameUpdate(FrameEventArgs args) {}
        public virtual void RenderUpdate(FrameEventArgs args) {}
        public virtual void Stop() {}
    }
}
