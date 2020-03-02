using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoRenderingWorkshop.Input;
using MonoRenderingWorkshop.MonoGame;
using MonoRenderingWorkshop.Rendering;
using MonoRenderingWorkshop.Rendering.Renderers;
using MonoRenderingWorkshop.Scenes;
using MonoRenderingWorkshop.Scenes.Cameras;
using MonoRenderingWorkshop.Scenes.Lights;
using System.Collections.Generic;

namespace MonoRenderingWorkshop
{
    public sealed class RenderingWorkshop : Game
    {
        private readonly GraphicsDeviceManager _graphics;

        private KeyboardController _keyboard;
        private MouseController _mouse;

        private ShaderManager _shaderManager;

        private UserInterface _ui;

        private Renderer _renderer;
        private Camera _camera;
        private Scene _scene;

        private bool _wasActive = true;

        public RenderingWorkshop()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                GraphicsProfile = GraphicsProfile.HiDef
            };
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsFixedTimeStep = false;

            _keyboard = new KeyboardController(Keyboard.GetState());
            _mouse = new MouseController(Mouse.GetState(), _graphics.GraphicsDevice);

            _renderer = CreateForwardRenderer();

            _shaderManager = new ShaderManager(Content, _keyboard);
            _shaderManager.ShadersReloaded += OnShadersReloaded;

            _camera = new Camera(
                _graphics.GraphicsDevice,
                _keyboard, _mouse,
                CameraSettings.DefaultSettings,
                CameraControls.DefaultControls,
                new Vector3(0, 2, 10),
                Vector3.Forward);

            _scene = new Scene(_camera);

            _scene.Add(new PointLight(new LightColour(Color.Yellow, 0.01f), new LightColour(Color.Red, 2f), new Vector3(0, 5, 8), 10f));
            _scene.Add(new PointLight(new LightColour(Color.Yellow, 0.01f), new LightColour(Color.Green, 2f), new Vector3(0, 8, 0), 10f));
            _scene.Add(new PointLight(new LightColour(Color.Yellow, 0.01f), new LightColour(Color.Blue, 2f), new Vector3(0, 5, -8), 10f));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _ui = new UserInterface(new SpriteBatch(GraphicsDevice), Content);

            _scene.Add(new Entity(Content.Load<Model>("models/sponza/sponza"),
                Vector3.Zero, Quaternion.Identity, Vector3.One));

            _shaderManager.Load();
        }

        private void OnShadersReloaded(GameTime time)
        {
            _renderer.SetMainEffect(_shaderManager.Load<Effect>($"shaders/{_renderer.MainEffectName}"));
            _ui.Debug("Shaders loaded successfully!", time);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            _shaderManager.Unload();
        }

        protected override void Update(GameTime time)
        {
            if (_wasActive != IsActive)
                OnFocusChanged(IsActive);
            _wasActive = IsActive;

            if (!IsActive)
                return;

            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            _keyboard.Update(time, keyboardState);
            _mouse.Update(time, mouseState);

            if (_keyboard.IsKeyDown(Keys.Escape) && !_keyboard.IsKeyDown(Keys.LeftAlt))
                Exit();

            _shaderManager.Update(time);
            _renderer.Update(time);
            _scene.Update(time);
            _ui.Update(time);

            _ui.Debug($"Time: {time.TotalGameTime.TotalSeconds:N2}");
            _ui.Debug($"Renderer: {_renderer.GetType().Name}");
            _ui.Debug(!_renderer.AllLightsActive
                ? $"Active Light is: {_renderer.ActiveLightIndex}"
                : "All Lights Active");

            _mouse.Reset();

            base.Update(time);
        }

        protected override void Draw(GameTime time)
        {
            _scene.Draw(_renderer);
            //_ui.Draw(time, _renderer);

            base.Draw(time);
        }

        private void OnFocusChanged(bool isActive)
        {
            _mouse.Reset();
        }

        private Renderer CreateForwardRenderer() =>
            new ForwardRenderer(_graphics, 1280, 720, _keyboard);
    }
}
