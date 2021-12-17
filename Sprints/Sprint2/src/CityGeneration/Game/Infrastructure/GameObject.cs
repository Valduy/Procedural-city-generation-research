using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace Engine.Infrastructure
{
    public class GameObject
    {
        private readonly Dictionary<Type, Component> _componentsMap = new();

        public readonly Engine Engine;
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public Vector3 Scale { get; set; } = new(1);

        public event Action<GameObject, Component>? ComponentAdded;
        public event Action<GameObject, Component>? ComponentRemoved;

        internal GameObject(Engine engine)
        {
            Engine = engine;
        }

        public GameObject Add(Component component)
        {
            if (_componentsMap.ContainsKey(component.GetType()))
            {
                return this;
            }

            _componentsMap[_componentsMap.GetType()] = component;
            component.GameObject = this;

            if (Engine.IsStarted)
            {
                component.Start();
            }

            ComponentAdded?.Invoke(this, component);
            return this;
        }

        public Component? Get<T>() where T : Component 
            => _componentsMap.TryGetValue(typeof(T), out var component) ? component : null;

        public Component? Remove<T>() where T : Component
        {
            if (_componentsMap.TryGetValue(typeof(T), out var component))
            {
                _componentsMap.Remove(typeof(T));

                if (Engine.IsStarted)
                {
                    component.Stop();
                }

                component.GameObject = null;
                ComponentRemoved?.Invoke(this, component);
                return component;
            }

            return null;
        }

        public bool Remove(Component component)
        {
            if (_componentsMap.Remove(component.GetType()))
            {
                ComponentRemoved?.Invoke(this, component);
                component.GameObject = null;
                return true;
            }

            return false;
        }

        public void Start()
        {
            foreach (var component in _componentsMap)
            {
                component.Value.Start();
            }
        }

        public void GameUpdate(FrameEventArgs args)
        {
            foreach (var component in _componentsMap)
            {
                component.Value.GameUpdate(args);
            }
        }

        public void RenderUpdate(FrameEventArgs args)
        {
            foreach (var component in _componentsMap)
            {
                component.Value.RenderUpdate(args);
            }
        }

        public void Stop()
        {
            foreach (var component in _componentsMap)
            {
                component.Value.Stop();
            }
        }
    }
}
