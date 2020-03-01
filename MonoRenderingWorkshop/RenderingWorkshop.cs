using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoRenderingWorkshop.Input;
using MonoRenderingWorkshop.Rendering;
using MonoRenderingWorkshop.Rendering.Renderers;
using MonoRenderingWorkshop.Scenes;
using MonoRenderingWorkshop.Scenes.Cameras;
using MonoRenderingWorkshop.Scenes.Lights;
using DirectionalLight = MonoRenderingWorkshop.Scenes.Lights.DirectionalLight;

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
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsFixedTimeStep = false;

            _keyboard = new KeyboardController(Keyboard.GetState());
            _mouse = new MouseController(Mouse.GetState(), _graphics.GraphicsDevice);

            _renderer = new ForwardRenderer(_graphics, 1280, 720, _keyboard);
            _shaderManager = new ShaderManager(Content, OnShadersReloaded);

            _camera = new Camera(
                _graphics.GraphicsDevice,
                _keyboard, _mouse,
                CameraSettings.DefaultSettings,
                CameraControls.DefaultControls,
                new Vector3(0, 2, 10),
                Vector3.Forward);

            _scene = new Scene(_camera);
            //_scene.Add(new DirectionalLight(new Vector3(-1, -1, -1), Color.White, 0.5f));
            _scene.Add(new PointLight(new Vector3(0, 5, 8), Color.White, 0.5f, 10f));
            _scene.Add(new PointLight(new Vector3(0, 8, 0), Color.White, 0.5f, 10f));
            _scene.Add(new PointLight(new Vector3(0, 5, -8), Color.White, 0.5f, 10f));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _ui = new UserInterface(new SpriteBatch(GraphicsDevice), Content);

            _scene.Add(new Entity(Content.Load<Model>("models/sponza/sponza"),
                Vector3.Zero, Quaternion.Identity, Vector3.One));
            //_scene.Add(new Entity(Content.Load<Model>("models/cube"),
            //    new Vector3(0, -1f, 0), Quaternion.Identity, new Vector3(50, 0.5f, 50)));

            _shaderManager.Reload();
        }

        private void OnShadersReloaded()
        {
            _renderer.SetMainEffect(_shaderManager.Load<Effect>("shaders/forwardRendering"));
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            _shaderManager.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            if (_wasActive != IsActive)
                OnFocusChanged(IsActive);
            _wasActive = IsActive;

            if (!IsActive)
                return;

            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            _keyboard.Update(gameTime, keyboardState);
            _mouse.Update(gameTime, mouseState);

            if (_keyboard.IsKeyDown(Keys.Escape))
                Exit();

            if (_keyboard.WasPressed(Keys.F5))
                _shaderManager.Reload();

            _renderer.Update(gameTime);
            _scene.Update(gameTime);
            _ui.Update(gameTime);

            _ui.Debug($"Time: {gameTime.TotalGameTime.TotalSeconds}");
            _ui.Debug($"Camera Position: {_camera.Position}");
            _ui.Debug(!_renderer.MainEffect.AllLightsActive
                ? $"Active Light is: {_renderer.MainEffect.ActiveLightIndex}"
                : "All Lights Active");

            _mouse.Reset();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _scene.Draw(_renderer);
            _ui.Draw(gameTime);

            base.Draw(gameTime);
        }

        private void OnFocusChanged(bool isActive)
        {
            _mouse.Reset();
        }
    }
}
