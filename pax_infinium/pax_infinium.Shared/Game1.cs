using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Input.Touch;
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
            Vector2 transformedMouseState = Vector2.Transform(mouseState.Position.ToVector2(), world.rooms.CurrentState.cameras.CurrentState.InverseTransform);
            Cube exampleCube = world.level.grid.cubes[0];
            Character player;
            if (world.level.grid.characters.list.Count > 0)
            {
                if (world.level.grid.characters.list.Count == 1)
                {
                    player = world.level.grid.characters.list[0];
                }
                else
                {
                    player = world.level.grid.characters.list[world.level.turn % world.level.turnOrder.Length];
                }
                //TouchPanelState touchPanelState = TouchPanel.GetState();

                // press esc to exit
                if (keyboardState.IsKeyDown(Keys.Escape))
                {
                    Exit();
                }

                // highlight scrolled over cube
                foreach (Cube cube in world.level.grid.cubes) // SHOULD BE POSSIBLE TO ONLY CHECK THE CUBE THE MOUSE IS ABOVE
                {
                    if (cube.topPoly.Contains(transformedMouseState) &&
                        world.level.grid.topOfColumn(cube.gridPos) == cube.gridPos.Z)
                    {
                        world.level.grid.highlightTex = world.textureConverter.highlightTex(cube.topTex);
                        world.level.grid.highlight = new Sprite(world.level.grid.highlightTex);
                        world.level.grid.highlight.origin = cube.top.origin;
                        world.level.grid.highlight.position = cube.top.position;
                        world.level.grid.highlightedCube = cube;

                        if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                        {
                            if (keyboardState.IsKeyDown(Keys.A)) // Attack
                            {
                                Character toBeKilled = null;
                                foreach (Character character in world.level.grid.characters.list)
                                {
                                    if (character != player && !world.level.attacked && cube.gridPos == character.gridPos &&
                                        world.cubeDist(player.gridPos, character.gridPos) == 1)
                                    {
                                        character.health -= player.strength;
                                        if (character.health <= 0)
                                        {
                                            Console.WriteLine(character.name + " has died!");
                                            toBeKilled = character;
                                        }
                                        else
                                        {
                                            Console.WriteLine(character.name + " health=" + character.health);
                                        }
                                        world.level.attacked = true;
                                    }
                                }
                                if (toBeKilled != null)
                                {
                                    world.level.grid.characters.list.Remove(toBeKilled);
                                }
                            }
                            else if (Game1.world.cubeDist(player.gridPos, cube.gridPos) < player.moveDist)
                            {
                                bool vacant = true;
                                Console.WriteLine();
                                foreach (Character character in world.level.grid.characters.list)
                                {
                                    if (character.gridPos == cube.gridPos)
                                    {
                                        vacant = false;
                                        break;
                                    }
                                }
                                if (vacant)
                                {
                                    player.gridPos = cube.gridPos;
                                    player.recalcPos();
                                    world.level.moved = true;
                                }
                            }
                        }


                    }
                }
            }

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
