using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ParallelEngine;

namespace XNA
{
    public class Game1 : Game
    {
        object _mapLocker = new object();
        bool _fullScreen;
        float _aspectRatio;
        float _modelRotation;
        float _rotationLeftRight;
        Vector3 _modelPosition = Vector3.Zero;
        Vector3 _modelVelocity = Vector3.Zero;
        Manager _engine;
        Map _map;        
        Rectangle _box;
        Vector3 _camera = new Vector3(0.0f, 50.0f, 5000.0f);
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;        
        #region Content
        Model _model;
        Texture2D _background;
        SpriteFont _font;
        #endregion

        public Game1(bool fullScreen)
        {
            _fullScreen = fullScreen;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            if (_fullScreen)
            {
                _graphics.ToggleFullScreen();
                _graphics.ApplyChanges();
            }
            _engine = new Manager(20, 10, 40, 2000);
            _engine.OnMapChanged += engine_OnMapChanged;
            _map = _engine.Map;            
            _box = new Rectangle(50, 30, GraphicsDevice.Viewport.Width - 100, GraphicsDevice.Viewport.Height - 60);
            IsMouseVisible = true;
            base.Initialize();
            _engine.Start();
        }

        void engine_OnMapChanged(Manager manager, MapChangedEventArgs args)
        {
            lock(_mapLocker)
                _map = args.Map;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _background = Content.Load<Texture2D>("Ground");
            _font = Content.Load<SpriteFont>("CountFont");
            _model = Content.Load<Model>("Models\\Wedge");
            _aspectRatio = GraphicsDevice.Viewport.AspectRatio;
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Dispose(bool disposing)
        {
            //TODO: do we need this
            //_engine.OnMapChanged -= engine_OnMapChanged;
            //_engine.Stop();
            base.Dispose(disposing);
        }

        protected override void Update(GameTime gameTime)
        {
            var gamePad = GamePad.GetState(PlayerIndex.One);
            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Escape))
                this.Exit();

            // Перемещаемся на 400 пикселей в секунду
            float moveFactorPerSecond = 400 * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            if (gamePad.DPad.Up == ButtonState.Pressed ||
                                   keyboard.IsKeyDown(Keys.Up))
                scrollPosition += moveFactorPerSecond;
            if (gamePad.DPad.Down == ButtonState.Pressed ||
                                 keyboard.IsKeyDown(Keys.Down))
                scrollPosition -= moveFactorPerSecond;

            if (keyboard.IsKeyDown(Keys.Q))
            {
                _modelRotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.1f);
            }
            else if (keyboard.IsKeyDown(Keys.E))
            {
                _modelRotation -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.1f);
            }
            if (keyboard.IsKeyDown(Keys.W))
            {
                _modelVelocity += new Vector3(0.0f, 0.0f, -1.0f); ;
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                _modelVelocity += new Vector3(0.0f, 0.0f, 1.0f); ;
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                _rotationLeftRight += (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.1f);
            }
            if (keyboard.IsKeyDown(Keys.D))
                _rotationLeftRight -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.1f);
            _rotationLeftRight = Math.Min(_rotationLeftRight, 0.5f);
            _rotationLeftRight = Math.Max(_rotationLeftRight, -0.5f);

            _modelPosition += _modelVelocity;
            _modelVelocity *= 0.95f;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            DrawBackGround();
            DrawMap(_map);
            //DrawModel(_model);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawModel(Model model)
        {
            var transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] *
                        Matrix.CreateRotationY(_modelRotation) *
                        Matrix.CreateRotationZ(_rotationLeftRight) *
                        Matrix.CreateTranslation(_modelPosition) * 
                        Matrix.CreateRotationX(0.8f);
                    effect.View = Matrix.CreateLookAt(_camera, Vector3.Zero, new Vector3(0.0f, 0.1f, 0.0f));
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), _aspectRatio, 1.0f, 10000.0f);
                }
                mesh.Draw();
            }
        }

        private void DrawMap(Map map)
        {
            lock (_mapLocker)
            {
                int stepx = (_box.Width / map.Width),
                    stepy = (_box.Width / map.Height);
                for (int x = _box.Left, i = 0; x < _box.Right; x += stepx, ++i)
                {
                    //todo: draw buffer in future
                    //var buffer = new VertexBuffer(GraphicsDevice, MyVertexType.Declaration, 2, BufferUsage.WriteOnly);
                    //buffer.SetData<MyVertexType>(new MyVertexType[4] {
                        //new MyVertexType(),
                        //x,
                        //_box.Top,
                        //x,
                        //_box.Bottom
                    //});
                    //GraphicsDevice.SetVertexBuffer(buffer);
                    //GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, 2);
                    for (int y = _box.Top, j = 0; y < _box.Bottom; y += stepy, ++j)
                    {
                        _spriteBatch.DrawString(_font, map.Count[i,j].ToString(), new Vector2(x,y), Color.Red);
                    }
                }
            }
        }

        private void DrawBackGround()
        {
            int resolutionWidth = _graphics.GraphicsDevice.Viewport.Width;
            int resolutionHeight = _graphics.GraphicsDevice.Viewport.Height;

            for (int x = 0; x <= resolutionWidth / _background.Width; x++)
                for (int y = -1; y <= resolutionHeight / _background.Height; y++)
                {
                    Vector2 position = new Vector2(
                                     x * _background.Width,
                                     y * _background.Height +
                                     ((int)scrollPosition) % _background.Height);
                    _spriteBatch.Draw(_background, position, Color.White);
                } // for for
        }

        float scrollPosition = 0;
    }
}
