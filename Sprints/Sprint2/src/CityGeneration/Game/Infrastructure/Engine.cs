using OpenTK.Windowing.Common;

namespace Engine.Infrastructure
{
    public class Engine
    {
        private readonly HashSet<GameObject> _gameObjects = new();

        public bool IsStarted { get; private set; }

        public GameObject CreateGameObject()
        {
            var go = new GameObject(this);
            _gameObjects.Add(go);
            return go;
        }

        public bool RemoveGameObject(GameObject go)
        {
            return _gameObjects.Remove(go);
        }

        public void Start()
        {
            foreach (var go in _gameObjects)
            {
                go.Start();
            }

            IsStarted = true;
        }

        public void GameUpdate(FrameEventArgs args)
        {
            foreach (var go in _gameObjects)
            {
                go.GameUpdate(args);
            }
        }

        public void RenderUpdate(FrameEventArgs args)
        {
            foreach (var go in _gameObjects)
            {
                go.RenderUpdate(args);
            }
        }

        public void Stop()
        {
            foreach (var go in _gameObjects)
            {
                go.Stop();
            }

            IsStarted = false;
        }
    }
}