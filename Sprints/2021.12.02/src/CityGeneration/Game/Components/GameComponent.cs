using Engine.Infrastructure;

namespace Engine.Game
{
    public class GameComponent : Component
    {
        public readonly Game Game;

        public GameComponent(Game game)
        {
            Game = game;
        }
    }
}
