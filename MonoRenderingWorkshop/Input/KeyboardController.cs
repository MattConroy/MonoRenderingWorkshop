using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoRenderingWorkshop.Input
{
    internal class KeyboardController
    {
        private KeyboardState _currentState;
        private KeyboardState _lastState;

        public KeyboardController(KeyboardState startState)
        {
            _currentState = startState;
        }

        public void Update(GameTime time, KeyboardState currentState)
        {
            _lastState = _currentState;
            _currentState = currentState;
        }

        public bool IsKeyDown(Keys key) =>
            _currentState.IsKeyDown(key);

        public bool IsKeyUp(Keys key) =>
            _currentState.IsKeyUp(key);

        public bool WasPressed(Keys key) =>
            _lastState.IsKeyDown(key) &&
            _currentState.IsKeyUp(key);
    }
}