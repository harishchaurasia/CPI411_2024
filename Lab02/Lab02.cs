using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.MediaFoundation;
using System;

namespace Lab02
{
    public class Lab02 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        Effect effect;
        /*float angle = -0.1f;*/
        float angle;
        Vector3 cameraPos = new Vector3(0, 0, 2);
        float distance = 1.1f;


        VertexPositionTexture[] vertices =
        {
        new VertexPositionTexture(new Vector3(0, 1, 0), new Vector2(0.5f,0)),
        new VertexPositionTexture(new Vector3(1, 0, 0),new Vector2(1,1)),
        new VertexPositionTexture(new Vector3(-1, 0, 0), new Vector2(0,1))
        };

        public Lab02()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            //IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            effect = Content.Load<Effect>("First3DShader");
            effect.Parameters["MyTexture"].SetValue(Content.Load<Texture2D>("logo_mg"));

            Matrix world = Matrix.Identity;
            /*            Matrix view = Matrix.CreateLookAt(
                            new Vector3(1,0,1), 
                            new Vector3(0,0,0), 
                            new Vector3(0,1,0)
                            );*/

            Matrix view = Matrix.CreateLookAt(cameraPos, new Vector3(), new Vector3(0, 1, 0));

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), GraphicsDevice.Viewport.AspectRatio, 0.1f, 100);

            effect.Parameters["World"].SetValue(world);
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);



            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                angle += 0.02f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                angle -= 0.02f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                distance += 0.01f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                distance -= 0.01f;
            }

            Console.WriteLine("hEYYYYYA");


            cameraPos = distance * new Vector3((float)System.Math.Sin(angle), 0, (float)System.Math.Cos(angle));

            Matrix world = Matrix.Identity;
            Matrix view = Matrix.CreateLookAt(cameraPos, new Vector3(), new Vector3(0, 1, 0));
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), GraphicsDevice.Viewport.AspectRatio, 0.1f, 100);

            effect.Parameters["World"].SetValue(world);
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(
                  PrimitiveType.TriangleList, vertices, 0, vertices.Length / 3);
            }

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}