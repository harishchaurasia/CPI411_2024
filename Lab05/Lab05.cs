using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI411.SimpleEngine;

namespace Lab05
{
    public class Lab05 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        SpriteFont font;


        //Lab5
        Matrix view, projection;
        Vector3 cameraPos = new Vector3(0, 0, 10);
        //Vector3 lighPos = new Vector3(10, 10, 10);

        float distance = 10;
        float angle;
        float angle2;

        Skybox skybox;
        string[] skyboxTextures; //names of images


        MouseState prevMouseState;

        public Lab05()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;


            // needed for the shader file
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Font");

            string[] skyboxTextures =
            {
                "Skybox/SunsetPNG2", "Skybox/SunsetPNG1", "Skybox/SunsetPNG4", "Skybox/SunsetPNG3", "Skybox/SunsetPNG6", "Skybox/SunsetPNG5"
            };

            skybox = new Skybox(skyboxTextures, Content, GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //world = Matrix.Identity;
            view = Matrix.CreateLookAt(cameraPos, new Vector3(), new Vector3(0, 1, 0));
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), GraphicsDevice.Viewport.AspectRatio, 0.1f, 100);



            MouseState currMouseState = Mouse.GetState();
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Pressed)
            {

                //angle is updated based on x value of mouse
                angle += (currMouseState.X - prevMouseState.X) / 100f;
                angle2 += (currMouseState.Y - prevMouseState.Y) / 100f;
            }

            prevMouseState = Mouse.GetState();

            cameraPos = Vector3.Transform(new Vector3(0, 0, distance), Matrix.CreateRotationX(angle2) * Matrix.CreateRotationY(angle));
            view = Matrix.CreateLookAt(cameraPos, Vector3.Zero, Vector3.UnitY);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.BlendState = BlendState.Opaque;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            skybox.Draw(view, projection, cameraPos);


            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "angle:" + angle, Vector2.UnitX +
            Vector2.UnitY * 12, Color.White);
            _spriteBatch.DrawString(font, "Use LEFT Mouse Button to look around.", Vector2.UnitX + (Vector2.UnitY * 40), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}