using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoRenderingWorkshop.Input
{
    internal class MouseController
    {
        public Vector2 Position => new Vector2(_currentState.Position.X, _currentState.Position.Y);
        public Vector2 DeltaPosition { get; set; }

        private readonly GraphicsDevice _graphicsDevice;

        private MouseState _currentState;
        private MouseState _lastState;

        public MouseController(MouseState startState, GraphicsDevice graphicsDevice)
        {
            DeltaPosition = Vector2.Zero;

            _graphicsDevice = graphicsDevice;
            _currentState = startState;
            _lastState = startState;
        }

        public void Update(float deltaTime, MouseState currentState)
        {
            DeltaPosition = new Vector2(
                _lastState.Position.X - currentState.Position.X,
                _lastState.Position.Y - currentState.Position.Y
                );

            _lastState = _currentState;
            _currentState = currentState;
        }

        public void Reset()
        {
            var (x, y) = _graphicsDevice.Viewport.Bounds.Center;
            Mouse.SetCursor(MouseCursor.Crosshair);
            Mouse.SetPosition(x, y);
            _lastState = new MouseState(x, y, 
                _lastState.ScrollWheelValue,
                _lastState.LeftButton,
                _lastState.MiddleButton,
                _lastState.RightButton,
                _lastState.XButton1,
                _lastState.XButton2,
                _lastState.HorizontalScrollWheelValue);
        }
    }
}