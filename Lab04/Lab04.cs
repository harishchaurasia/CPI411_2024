using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Reflection;

namespace Lab04
{
    public class Lab04 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;


        Model model;
        Effect effect;
        Vector3 cameraPos = new Vector3(0, 0, 2);
        Vector3 lighPos = new Vector3(1, 1, 1);
        float distance = 10;
        float angle;
        float angle2;
        float zoom = 20f;
        float translate = 0.0f;
        float translate2 = 0.0f;
        Matrix world, view, projection;
        SpriteFont font;

        MouseState prevMouseState;



        // ***** Color data set

        Vector4 ambient = new Vector4(0, 0, 0, 0);
        float ambientIntensity = 0.0f;
        Vector4 diffuseColor = new Vector4(1, 1, 1, 1);
        float diffuseIntensity = 1.0f;

        public Lab04()
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
            spriteBatch = new SpriteBatch(GraphicsDevice);


            model = Content.Load<Model>("bunny");
            effect = Content.Load<Effect>("SimpleShading");
            font = Content.Load<SpriteFont>("Font");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            world = Matrix.Identity;
            view = Matrix.CreateLookAt(cameraPos, new Vector3(), new Vector3(0, 1, 0));
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), GraphicsDevice.Viewport.AspectRatio, 0.1f, 100);

            MouseState currMouseState = Mouse.GetState();
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Pressed)
            {

                //angle is updated based on x value of mouse
                angle += (currMouseState.X - prevMouseState.X) / 100f;
                angle2 += (currMouseState.Y - prevMouseState.Y) / 100f;
            }

            if (currMouseState.RightButton == ButtonState.Pressed &&
                prevMouseState.RightButton == ButtonState.Pressed)
            {
                zoom -= (currMouseState.X - prevMouseState.X) / 100f;
            }

            prevMouseState = Mouse.GetState();

            cameraPos = Vector3.Transform(new Vector3(0, 0, zoom), Matrix.CreateRotationX(angle2) * Matrix.CreateRotationY(angle));
            view = Matrix.CreateLookAt(cameraPos, Vector3.Zero, Vector3.UnitY);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = new DepthStencilState();

            //model.Draw(world, view, projection);

            effect.CurrentTechnique = effect.Techniques[0];

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {

                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {

                        // ***** set parameters in effects ******
                        effect.Parameters["World"].SetValue(mesh.ParentBone.Transform);
                        effect.Parameters["View"].SetValue(view);
                        effect.Parameters["Projection"].SetValue(projection);
                        effect.Parameters["AmbientColor"].SetValue(ambient);
                        effect.Parameters["AmbientIntensity"].SetValue(ambientIntensity);
                        effect.Parameters["DiffuseColor"].SetValue(diffuseColor);
                        effect.Parameters["DiffuseIntensity"].SetValue(diffuseIntensity);
                        effect.Parameters["DiffuseLightDirection"].SetValue(lighPos);

                        Matrix worldInverseTranspose = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform));
                        effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTranspose);


                        //************************************
                        pass.Apply();
                        GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                        GraphicsDevice.Indices = part.IndexBuffer;
                        GraphicsDevice.DrawIndexedPrimitives(
                            PrimitiveType.TriangleList,
                            part.VertexOffset,
                            part.StartIndex,
                            part.PrimitiveCount);
                    }
                }
            }

            //** 2D Drawing

            spriteBatch.Begin();
            spriteBatch.DrawString(font, ("Angle:" + angle), Vector2.UnitX + Vector2.UnitY * 12, Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}