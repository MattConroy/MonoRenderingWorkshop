using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoRenderingWorkshop.Input;

namespace MonoRenderingWorkshop.Scenes.Cameras
{
    internal sealed class Camera
    {
        public Matrix Projection { get; private set; }
        public Matrix View { get; private set; }
        public Vector3 Position => _position;



        private readonly GraphicsDevice _graphicsDevice;
        private readonly KeyboardController _keyboard;
        private readonly MouseController _mouse;

        private readonly CameraControls _controls;
        private readonly CameraSettings _settings;

        private Vector3 _position;
        private Vector3 _forward;

        public Camera(GraphicsDevice graphicsDevice,
            KeyboardController keyboard, MouseController mouse,
            CameraSettings settings, CameraControls controls,
            Vector3 position, Vector3 forward)
        {
            _graphicsDevice = graphicsDevice;
            _keyboard = keyboard;
            _mouse = mouse;

            _controls = controls;
            _settings = settings;

            _position = position;
            _forward = forward;
        }

        public void Update(float deltaTime)
        {
            var right = Vector3.Cross(_forward, Vector3.Up);
            _forward = Vector3.TransformNormal(_forward,
                Matrix.CreateFromAxisAngle(right, _mouse.DeltaPosition.Y * deltaTime * _controls.MouseSensitivity.Y) *
                Matrix.CreateFromAxisAngle(Vector3.Up, _mouse.DeltaPosition.X * deltaTime * _controls.MouseSensitivity.X));
            _forward.Normalize();

            var rotation = Matrix.Invert(Matrix.CreateLookAt(Vector3.Zero, _forward, Vector3.Up));

            bool modifiedSpeed = _controls.SpeedKeys.Any(_keyboard.IsKeyDown);
            foreach (var mapping in _controls.MovementMap)
            {
                if (_keyboard.IsKeyDown(mapping.Key))
                    _position += GetDeltaPosition(deltaTime, rotation, mapping.Value, modifiedSpeed);
            }

            View = ComputeViewMatrix();
            Projection = ComputeProjectionMatrix();
        }

        private Vector3 GetDeltaPosition(float deltaTime, Matrix rotation, Vector3 rawDirection, bool modifiedSpeed) =>
            Vector3.Transform(rawDirection, rotation) * deltaTime * _controls.GetSpeed(modifiedSpeed);

        private Matrix ComputeViewMatrix() =>
            Matrix.CreateLookAt(
                _position,
                _position + _forward,
                Vector3.Up);

        private Matrix ComputeProjectionMatrix() =>
            Matrix.CreatePerspectiveFieldOfView(
                _settings.FieldOfView,
                _settings.GetAspectRatio(_graphicsDevice),
                _settings.NearPlane,
                _settings.FarPlane);
    }
}