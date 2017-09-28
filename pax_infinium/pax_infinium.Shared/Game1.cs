using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace pax_infinium
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        public static World world;

        public KeyboardState previousKeyboardState;
        public MouseState previousMouseState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

#if __MOBILE__
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
#elif WINDOWS
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            IsMouseVisible = true;
#endif
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            world = new World(graphics);
            World.textureManager = new ContentManager<Texture2D>(Content);
            World.fontManager = new ContentManager<SpriteFont>(Content);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            World.textureManager.Load("BG-Layer");
            World.textureManager.Load("m");
            World.fontManager.Load("ScoreFont");
            World.fontManager.Load("InfoFont");
            World.fontManager.Load("Impact-36");

            // create 1x1 texture for line drawing
            world.oneByOne = new Texture2D(GraphicsDevice, 1, 1);
            world.oneByOne.SetData<Color>(
                new Color[] { Color.White });// fill the texture with white

            world.textureConverter = new TextureConverter(graphics.GraphicsDevice);
            //tex2 = world.textureConverter.Rotate90(tex2);
            //tex = world.textureConverter.Rotate(tex, 45);

            world.level = new Level(graphics, "The world is mine!");
            world.rooms.CurrentState.AddDraw(world.level.Draw);
            world.rooms.CurrentState.AddUpdate(world.level.Update);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            // press esc to exit
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (mouseState.LeftButton == ButtonState.Pressed &&
                previousMouseState.LeftButton == ButtonState.Released)
            {
                Vector2 transformedMouseState = Vector2.Transform(mouseState.Position.ToVector2(), world.rooms.CurrentState.cameras.CurrentState.InverseTransform);
                Console.WriteLine("mousePos tx:" + transformedMouseState.X + " ty:" + transformedMouseState.Y);
                foreach (Cube cube in world.level.grid.cubes)
                {
                    if (cube.top.rectangle.Contains(transformedMouseState) && world.level.grid.topOfColumn(cube.gridPos) == cube.gridPos.Z)
                    {
                        cube.darken();
                    }
                }
            }

            /*// left click
            if (mouseState.LeftButton == ButtonState.Pressed && 
                previousMouseState.LeftButton == ButtonState.Released)
            {
                Vector2 transformedMouseState = Vector2.Transform(mouseState.Position.ToVector2(), world.rooms.CurrentState.cameras.CurrentState.InverseTransform);
                foreach(Character character in world.level.characters.list)
                {
                    if (character.top.rectangle.Contains(transformedMouseState))
                    {
                        world.level.characters.selectedCharacter = character;
                    }
                }

            }*/
            /*world.level.characters.selectedCharacter = world.level.characters.list[0];

            // right click
            if (mouseState.LeftButton == ButtonState.Pressed &&
                previousMouseState.LeftButton == ButtonState.Released)
            {
                if (world.level.characters.selectedCharacter != null) {
                    Character selectedCharacter = world.level.characters.selectedCharacter;
                Vector2 transformedMouseState = Vector2.Transform(mouseState.Position.ToVector2(), world.rooms.CurrentState.cameras.CurrentState.InverseTransform);
                    foreach (Cube cube in world.level.grid.cubes)
                    {
                        if (cube.top.rectangle.Contains(transformedMouseState))
                        {
                            selectedCharacter.gridPos = cube.gridPos;
                            selectedCharacter.recalcPos();
                        }
                    }
                }

            }*/

            world.Update(gameTime);            

            previousKeyboardState = keyboardState;
            previousMouseState = mouseState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            world.BeginDraw();
            world.Draw();
            world.EndDraw();

            base.Draw(gameTime);
        }
    }
}
