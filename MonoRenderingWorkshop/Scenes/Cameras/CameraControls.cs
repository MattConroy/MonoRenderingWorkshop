using System.Collections.Generic;
using Microsoft.Xna.Framework;
using KeyboardKeys = Microsoft.Xna.Framework.Input.Keys;

namespace MonoRenderingWorkshop.Scenes.Cameras
{
    public sealed class CameraControls
    {
        public IDictionary<KeyboardKeys, Vector3> MovementMap { get; }
        public IEnumerable<KeyboardKeys> SpeedKeys { get; }
        public Vector2 MouseSensitivity { get; }

        private readonly float _speedModifier;
        private readonly float _speed;

        public CameraControls(IDictionary<KeyboardKeys, Vector3> movementMap,
            IEnumerable<KeyboardKeys> speedKeys,
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
            new CameraControls(new Dictionary<KeyboardKeys, Vector3>
                {
                    [KeyboardKeys.W] = Vector3.Forward,
                    [KeyboardKeys.S] = Vector3.Backward,
                    [KeyboardKeys.A] = Vector3.Left,
                    [KeyboardKeys.D] = Vector3.Right,
                    [KeyboardKeys.Q] = Vector3.Down,
                    [KeyboardKeys.E] = Vector3.Up
                },
                new[] { KeyboardKeys.LeftShift, KeyboardKeys.RightShift },
                new Vector2(0.8f, 0.6f), 8f, 4f);
    }
}
