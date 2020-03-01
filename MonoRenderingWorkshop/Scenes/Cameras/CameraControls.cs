using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoRenderingWorkshop.Scenes.Cameras
{
    public sealed class CameraControls
    {
        public IDictionary<Keys, Vector3> MovementMap { get; }
        public IEnumerable<Keys> SpeedKeys { get; }
        public Vector2 MouseSensitivity { get; }

        private readonly float _speedModifier;
        private readonly float _speed;

        public CameraControls(IDictionary<Keys, Vector3> movementMap,
            IEnumerable<Keys> speedKeys,
            Vector2 mouseSensitivity,
            float speed, float speedModifier)
        {
            MovementMap = movementMap;
            SpeedKeys = speedKeys;

            MouseSensitivity = mouseSensitivity;
            _speedModifier = speedModifier;
            _speed = speed;
        }

        public float GetSpeed(bool isModified) =>
            _speed * (isModified ? _speedModifier : 1f);

        public static CameraControls DefaultControls =>
            new CameraControls(new Dictionary<Keys, Vector3>
                {
                    [Keys.W] = Vector3.Forward,
                    [Keys.S] = Vector3.Backward,
                    [Keys.A] = Vector3.Left,
                    [Keys.D] = Vector3.Right,
                    [Keys.Q] = Vector3.Down,
                    [Keys.E] = Vector3.Up
                },
                new[] { Keys.LeftShift, Keys.RightShift },
                new Vector2(0.8f, 0.6f), 8f, 4f);
    }
}
