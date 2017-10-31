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
            //World.textureManager.Load("m");
            World.fontManager.Load("ScoreFont");
            World.fontManager.Load("InfoFont");
            World.fontManager.Load("Impact-36");
            World.textureManager.Load("Soldier");
            World.textureManager.Load("Soldier2");
            World.textureManager.Load("Hunter");
            World.textureManager.Load("Hunter2");
            World.textureManager.Load("Black Mage");
            World.textureManager.Load("Black Mage2");


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

            // press esc to exit
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (keyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyUp(Keys.Left)) // Rotate Left
            {
                world.level.grid.rotate(false);
            }

            if (keyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyUp(Keys.Right)) // Rotate Right
            {
                world.level.grid.rotate(true);
            }

            if (world.level.grid.characters.list.Count > 0)
            {
                if (world.level.grid.characters.list.Count == 1)
                {
                    player = world.level.grid.characters.list[0];
                }
                else
                {
                    player = world.level.grid.characters.list[0];//world.level.turn % world.level.turnOrder.Length];
                }

                foreach (Cube cube in world.level.grid.cubes) // clear highlights
                {
                    cube.highLight = false;
                }

                if (keyboardState.IsKeyDown(Keys.A) && !world.level.attacked) // WAttack highlight
                {
                    foreach(Cube cube in world.level.grid.cubes)
                    {
                        if ((world.cubeDist(cube.gridPos, player.gridPos) == 1)){
                            cube.highLight = true;
                        }
                    }

                }
                else if (keyboardState.IsKeyDown(Keys.M) && !world.level.moved){ // move highlight
                    foreach(Cube cube in world.level.grid.cubes)
                    {
                        int cubeDist = world.cubeDist(cube.gridPos, player.gridPos);
                        if (cubeDist < player.move && cubeDist > 0)
                        {
                            cube.highLight = true;
                        }
                    }
                }

                if (keyboardState.IsKeyDown(Keys.E) && previousKeyboardState.IsKeyUp(Keys.E)) // end turn
                {
                    world.level.endTurn();
                }

                // highlight scrolled over cube
                foreach (Cube cube in world.level.grid.cubes) // SHOULD BE POSSIBLE TO ONLY CHECK THE CUBE THE MOUSE IS ABOVE
                {
                    if (cube.topPoly.Contains(transformedMouseState) &&
                        world.level.grid.topOfColumn(cube.gridPos) == cube.gridPos.Z)
                    {
                        /* world.level.grid.highlightTex = world.textureConverter.highlightTex(cube.topTex);
                         world.level.grid.highlight = new Sprite(world.level.grid.highlightTex);
                         world.level.grid.highlight.origin = cube.top.origin;
                         world.level.grid.highlight.position = cube.top.position;
                         world.level.grid.highlightedCube = cube;*/
                        if (!cube.highLight)
                        {
                            cube.highLight = true;
                        }
                        else
                        {
                            cube.highLight = false;
                        }
                        world.level.grid.onHighlightMoved(cube);

                        if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                        {
                            if (keyboardState.IsKeyDown(Keys.A) && !world.level.attacked) // Attack
                            {
                                Character toBeKilled = null;
                                foreach (Character character in world.level.grid.characters.list)
                                {
                                    if (character != player && !world.level.attacked && cube.gridPos == character.gridPos &&
                                        world.cubeDist(player.gridPos, character.gridPos) == 1)
                                    {
                                        int chance = 100 - character.evasion;
                                        Console.WriteLine("\nChance to hit: " + chance + "%");
                                        if (chance >= World.Random.Next(1, 101)){
                                            
                                            int damage = (int)((player.WAttack - (character.WDefense / 2)) * .5);
                                            Console.WriteLine("Hit! " + character.name + " takes " + damage + " damage!");
                                            character.health -= damage;

                                            if (character.health <= 0)
                                            {
                                                Console.WriteLine(character.name + " has died!");
                                                toBeKilled = character;
                                            }
                                            else
                                            {
                                                Console.WriteLine(character.name + " health: " + character.health);
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Miss!");
                                        }

                                        world.level.attacked = true;
                                    }
                                }
                                if (toBeKilled != null)
                                {
                                    world.level.grid.characters.list.Remove(toBeKilled);
                                }
                            }
                            else if (keyboardState.IsKeyDown(Keys.M) && Game1.world.cubeDist(player.gridPos, cube.gridPos) < player.move && !world.level.moved) // Move
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
                                    world.level.grid.onCharacterMoved();
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
