﻿using MonoRenderingWorkshop.Rendering.Renderers;
using MonoRenderingWorkshop.Scenes.Cameras;
using MonoRenderingWorkshop.Scenes.Lights;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoRenderingWorkshop.Scenes
{
    internal sealed class Scene
    {
        private readonly IList<Entity> _entities;
        private readonly IList<Light> _lights;
        private readonly Camera _camera;

        public Scene(Camera camera)
        {
            _camera = camera;
            _entities = new List<Entity>();
            _lights = new List<Light>();
        }

        public void Add(Entity entity)
        {
            if (!_entities.Contains(entity))
                _entities.Add(entity);
        }

        public void Add(Light light)
        {
            if (!_lights.Contains(light))
                _lights.Add(light);
        }

        public void Update(GameTime time)
        {
            _camera.Update(time);

            foreach (var light in _lights)
                light.Update(time);
        }

        public void Draw(Renderer renderer)
        {
            renderer.Draw(_camera,
                _entities.Select(entity => entity.GetRenderData()).ToList(), 
                _lights.Select(light => light.GetRenderData()).ToList());
        }
    }
}
