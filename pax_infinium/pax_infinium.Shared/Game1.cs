using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Linq;

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
        public String selectedAction;
        public bool confirmAction;
        public Vector2 lastClickMouseState;
        public Vector2 activeMouseState;

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
            selectedAction = "";
            confirmAction = false;
            lastClickMouseState = Vector2.Zero;
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

            World.fontManager.Load("Trajanus Roman 64");
            World.fontManager.Load("Trajanus Roman 12");
            World.fontManager.Load("Trajanus Roman 24");
            World.fontManager.Load("Trajanus Roman 36");

            World.textureManager.Load("Blue Soldier\\Blue Soldier NW");
            World.textureManager.Load("Blue Soldier\\Blue Soldier NE");
            World.textureManager.Load("Blue Soldier\\Blue Soldier SW");
            World.textureManager.Load("Blue Soldier\\Blue Soldier SE");
            World.textureManager.Load("Blue Soldier\\Blue Soldier FL");
            World.textureManager.Load("Blue Soldier\\Blue Soldier FR");

            World.textureManager.Load("Red Soldier\\Red Soldier NW");
            World.textureManager.Load("Red Soldier\\Red Soldier NE");
            World.textureManager.Load("Red Soldier\\Red Soldier SW");
            World.textureManager.Load("Red Soldier\\Red Soldier SE");
            World.textureManager.Load("Red Soldier\\Red Soldier FL");
            World.textureManager.Load("Red Soldier\\Red Soldier FR");

            World.textureManager.Load("Blue Hunter\\Blue Hunter NW");
            World.textureManager.Load("Blue Hunter\\Blue Hunter NE");
            World.textureManager.Load("Blue Hunter\\Blue Hunter SW");
            World.textureManager.Load("Blue Hunter\\Blue Hunter SE");
            World.textureManager.Load("Blue Hunter\\Blue Hunter FL");
            World.textureManager.Load("Blue Hunter\\Blue Hunter FR");

            World.textureManager.Load("Red Hunter\\Red Hunter NW");
            World.textureManager.Load("Red Hunter\\Red Hunter NE");
            World.textureManager.Load("Red Hunter\\Red Hunter SW");
            World.textureManager.Load("Red Hunter\\Red Hunter SE");
            World.textureManager.Load("Red Hunter\\Red Hunter FL");
            World.textureManager.Load("Red Hunter\\Red Hunter FR");

            World.textureManager.Load("Blue Mage\\Blue Mage NW");
            World.textureManager.Load("Blue Mage\\Blue Mage NE");
            World.textureManager.Load("Blue Mage\\Blue Mage SW");
            World.textureManager.Load("Blue Mage\\Blue Mage SE");
            World.textureManager.Load("Blue Mage\\Blue Mage FL");
            World.textureManager.Load("Blue Mage\\Blue Mage FR");

            World.textureManager.Load("Red Mage\\Red Mage NW");
            World.textureManager.Load("Red Mage\\Red Mage NE");
            World.textureManager.Load("Red Mage\\Red Mage SW");
            World.textureManager.Load("Red Mage\\Red Mage SE");
            World.textureManager.Load("Red Mage\\Red Mage FL");
            World.textureManager.Load("Red Mage\\Red Mage FR");

            World.textureManager.Load("Blue Healer\\Blue Healer NW");
            World.textureManager.Load("Blue Healer\\Blue Healer NE");
            World.textureManager.Load("Blue Healer\\Blue Healer SW");
            World.textureManager.Load("Blue Healer\\Blue Healer SE");
            World.textureManager.Load("Blue Healer\\Blue Healer FL");
            World.textureManager.Load("Blue Healer\\Blue Healer FR");

            World.textureManager.Load("Red Healer\\Red Healer NW");
            World.textureManager.Load("Red Healer\\Red Healer NE");
            World.textureManager.Load("Red Healer\\Red Healer SW");
            World.textureManager.Load("Red Healer\\Red Healer SE");
            World.textureManager.Load("Red Healer\\Red Healer FL");
            World.textureManager.Load("Red Healer\\Red Healer FR");

            World.textureManager.Load("Blue Thief\\Blue Thief NW");
            World.textureManager.Load("Blue Thief\\Blue Thief NE");
            World.textureManager.Load("Blue Thief\\Blue Thief SW");
            World.textureManager.Load("Blue Thief\\Blue Thief SE");
            World.textureManager.Load("Blue Thief\\Blue Thief FL");
            World.textureManager.Load("Blue Thief\\Blue Thief FR");

            World.textureManager.Load("Red Thief\\Red Thief NW");
            World.textureManager.Load("Red Thief\\Red Thief NE");
            World.textureManager.Load("Red Thief\\Red Thief SW");
            World.textureManager.Load("Red Thief\\Red Thief SE");
            World.textureManager.Load("Red Thief\\Red Thief FL");
            World.textureManager.Load("Red Thief\\Red Thief FR");

            World.textureManager.Load("Status Icons");

            World.textureManager.Load("Start Screen");

            World.textureManager.Load("Thought Bubble");

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
            //List<Cube> highlightedCubes = new List<Cube>();

            // saves last click state
            if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                lastClickMouseState = transformedMouseState;
                if (world.state == 0)
                {
                    resetConfirmation(); // Fixes highlighting bug
                    world.state = 1;
                }
            }

            // if last mouse state is not arbitrary (0,0), make it the active mouse state
            if (lastClickMouseState != Vector2.Zero)
            {
                activeMouseState = lastClickMouseState;
            }
            else
            {
                activeMouseState = transformedMouseState;
            }

            // press esc to exit
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (world.state == 1)
            {

            // left arrow rotates left
            if (keyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyUp(Keys.Left))
            {
                world.level.grid.rotate(false, world.level);
            }

            // right arrow rotates right
            if (keyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyUp(Keys.Right))
            {
                world.level.grid.rotate(true, world.level);
            }

            // if there are characters
            if (world.level.grid.characters.list.Count > 0)
            {
                player = world.level.grid.characters.list[0];

                // Attack key w/ confirmation
                if (keyboardState.IsKeyDown(Keys.A) && previousKeyboardState.IsKeyUp(Keys.A))
                {
                    if (selectedAction != "")
                    {
                        resetConfirmation();
                    }
                    else
                    {
                        selectedAction = "attack";
                    }
                }
                // Special key w/ confirmation
                else if (keyboardState.IsKeyDown(Keys.S) && previousKeyboardState.IsKeyUp(Keys.S))
                {
                    if (selectedAction != "")
                    {
                        resetConfirmation();
                    }
                    else
                    {
                        selectedAction = "special";
                    }
                }
                // Move key w/ confirmation
                else if (keyboardState.IsKeyDown(Keys.M) && previousKeyboardState.IsKeyUp(Keys.M))
                {
                    if (selectedAction != "")
                    {
                        resetConfirmation();
                    }
                    else
                    {
                        selectedAction = "move";
                    }
                }
                // Confirm key
                else if (keyboardState.IsKeyDown(Keys.Y) && previousKeyboardState.IsKeyUp(Keys.Y))
                {
                    confirmAction = true;
                }
                // Cancel key
                else if (keyboardState.IsKeyDown(Keys.N) && previousKeyboardState.IsKeyUp(Keys.N))
                {
                    if (selectedAction == "" && world.level.moved && !world.level.attacked) // undo movement
                    {
                        player.Move(world.level.movedFrom);
                        world.level.moved = false;
                        world.level.rotated = false;
                    }
                    else
                    {
                        resetConfirmation();
                    }
                }

                foreach (Cube cube in world.level.grid.cubes) // clear highlights
                {
                    cube.highLight = false;
                    if (player.gridPos == cube.gridPos)
                    {
                        cube.invert = true;
                    }
                    else
                    {
                        cube.invert = false;
                    }
                }

                 world.level.handleActionTextColors(selectedAction);

                if (selectedAction != "")
                {
                    if ((player.job == 2 || player.job == 3) && selectedAction == "special" && !world.level.attacked)
                    { // Black Mage or healer or thief
                        foreach (Cube cube in world.level.grid.cubes)
                        {
                            if (player.InMagicRange(cube.gridPos))
                            {
                                cube.highLight = true;
                                //highlightedCubes.Add(cube);
                            }

                            // if the player is in magic range and the standing on a cube the mouse is over
                            // and the cube is on the surface
                            if (player.InMagicRange(cube.gridPos) && cube.topPoly.Contains(activeMouseState) &&
                            world.level.grid.topOfColumn(cube.gridPos) == cube.gridPos.Z)
                            {
                                foreach (Cube c in world.level.grid.cubes)
                                {
                                    if ((c.isAdjacent(cube.gridPos) || c.gridPos == cube.gridPos))
                                    {
                                        c.invert = true;
                                    }
                                }
                            }
                        }
                    }
                    else if (selectedAction == "attack" && !world.level.attacked) // WAttack highlight
                    {
                        if (player.job == 1) // Hunter
                        {
                            foreach (Cube cube in world.level.grid.cubes)
                            {
                                if (player.InWeaponRange(cube.gridPos))
                                {
                                    cube.highLight = true;
                                    //highlightedCubes.Add(cube);
                                }
                            }
                        }
                        else // Everyone Else
                        {
                            foreach (Cube cube in world.level.grid.cubes)
                            {
                                if (cube.isAdjacent(player.gridPos))
                                {
                                    cube.highLight = true;
                                    //highlightedCubes.Add(cube);
                                }
                            }
                        }
                    }
                    else if (selectedAction == "move" && !world.level.moved)
                    { // move highlight
                        foreach (Cube cube in world.level.grid.cubes)
                        {
                            if (player.inMoveRange(cube.gridPos, world.level))
                            {
                                cube.highLight = true;
                                //highlightedCubes.Add(cube);
                            }
                        }
                    }
                    else if ((player.job == 0 || player.job == 4) && selectedAction == "special" && !world.level.attacked) //thief special
                    {
                        foreach (Cube cube in world.level.grid.cubes)
                        {
                            if (player.InMagicRange(cube.gridPos))
                            {
                                cube.highLight = true;
                                //highlightedCubes.Add(cube);
                            }
                        }
                    }
                }

                if (keyboardState.IsKeyDown(Keys.E) && previousKeyboardState.IsKeyUp(Keys.E)) // end turn
                {
                    world.level.endTurn(gameTime);
                }

                bool mouseInBounds = false;

                // highlight scrolled over cube
                foreach (Cube cube in world.level.grid.cubes) // SHOULD BE POSSIBLE TO ONLY CHECK THE CUBE THE MOUSE IS ABOVE
                {
                    bool topVisible = true;
                    foreach (Cube c in world.level.grid.cubes)
                    {
                        if (c != cube)
                        {
                            if (c.topPoly.Contains(activeMouseState))
                            {
                                if (cube.DrawOrder() < c.DrawOrder())
                                {
                                    topVisible = false;
                                }
                            }
                        }
                    }

                    if (cube.topPoly.Contains(activeMouseState) &&
                        world.level.grid.topOfColumn(cube.gridPos) == cube.gridPos.Z && topVisible)
                    {
                        mouseInBounds = true;
                        if (!cube.highLight)
                        {
                            cube.highLight = true;
                        }
                        else
                        {
                            cube.highLight = false;
                        }

                        world.level.grid.onHighlightMoved(cube);
                        world.level.grid.onCharacterMoved(world.level);

                        world.level.clearCharacter();
                        foreach (Character c in world.level.grid.characters.list)
                        {
                            if (cube.gridPos == c.gridPos)
                            {
                                world.level.setCharacter(c);
                            }
                        }

                        if (!world.level.rotated)
                        {
                            player.Rotate(cube.gridPos);
                        }

                        if (!confirmAction && selectedAction != "" && lastClickMouseState != Vector2.Zero)
                        {
                            if ((((player.job == 2 || player.job == 3 || player.job == 4 || player.job == 0) && selectedAction == "special") || selectedAction == "attack") && !world.level.attacked) // Attack
                            {
                                if (selectedAction == "special")
                                {
                                    if (player.CanCast(8))
                                    {
                                        if (player.job == 2 || player.job == 3)
                                        {
                                            foreach (Character character in world.level.grid.characters.list)
                                            {
                                                if (!world.level.attacked && (cube.isAdjacent(character.gridPos) || character.gridPos == cube.gridPos) &&
                                                    player.InMagicRange(cube.gridPos))
                                                {
                                                    if (player.job == 2)
                                                    {
                                                        int[] chanceDamage;
                                                        chanceDamage = player.calculateMageSpecial(character);
                                                        world.level.SetConfirmationText(chanceDamage[0], chanceDamage[1]);
                                                    }
                                                    else if (player.job == 3)// healer
                                                    {
                                                        int health = 15;
                                                        world.level.SetConfirmationText("+" + health + "HP Confirm Y / N");
                                                    }
                                                }
                                            }
                                        }
                                        else if (player.job == 4)// thief special
                                        {
                                            foreach (Character character in world.level.grid.characters.list)
                                            {
                                                if (character != player && !world.level.attacked && cube.gridPos == character.gridPos &&
                                                    player.InMagicRange(character.gridPos))
                                                {
                                                    int chance = player.CalculateThiefSpecial(character);

                                                    world.level.SetConfirmationText("Chance " + chance + "% Confirm Y / N");
                                                }
                                            }
                                        }
                                        else if (player.job == 0)// soldier special
                                        {
                                            foreach (Character character in world.level.grid.characters.list)
                                            {
                                                if (character != player && !world.level.attacked && cube.gridPos == character.gridPos &&
                                                    player.InMagicRange(character.gridPos))
                                                {
                                                    int defenseBoost = 20;
                                                    world.level.SetConfirmationText("+" + defenseBoost + "WD Confirm Y / N");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Not enough mp to cast!");
                                    }
                                }
                                else // attack
                                {
                                    int[] chanceDamage;
                                    foreach (Character character in world.level.grid.characters.list)
                                    {
                                        if (cube.gridPos == character.gridPos)
                                        {

                                            chanceDamage = player.calculateAttack(character);
                                            world.level.SetConfirmationText(chanceDamage[0], chanceDamage[1]);
                                        }
                                    }
                                }
                            }
                            else if (selectedAction == "move" && player.inMoveRange(cube.gridPos, world.level) &&
                                !world.level.moved) // Move
                            {
                                if (world.level.grid.isVacant(cube.gridPos))
                                {
                                    //world.level.SetConfirmationText("Move? Confirm Y / N");
                                    world.level.movedFrom = player.gridPos;
                                    player.Move(cube.gridPos);
                                    //world.level.grid.onCharacterMoved();
                                    world.level.moved = true;
                                    world.level.rotated = false;
                                    resetConfirmation();
                                }
                            }
                        }
                        else if (confirmAction && selectedAction != "" && lastClickMouseState != Vector2.Zero) // Confirmed
                        {
                            if ((((player.job == 2 || player.job == 3 || player.job == 4 || player.job == 0) && selectedAction == "special") || selectedAction == "attack") && !world.level.attacked) // Attack
                            {
                                if (selectedAction == "special")
                                {
                                    if (player.CanCast(8))
                                    {
                                        player.payForCast(8, gameTime);

                                        if (player.job == 2 || player.job == 3)
                                        {
                                            List<Character> toBeKilled = new List<Character>();
                                            foreach (Character character in world.level.grid.characters.list)
                                            {
                                                if (!world.level.attacked && (cube.isAdjacent(character.gridPos) || character.gridPos == cube.gridPos) &&
                                                    player.InMagicRange(cube.gridPos))
                                                {
                                                    if (player.job == 2)
                                                    {
                                                        Character result = player.MageSpecial(character, gameTime);
                                                        if (result != null)
                                                        {
                                                            toBeKilled.Add(result);
                                                        }
                                                    }
                                                    else if (player.job == 3)// healer
                                                    {
                                                        player.HealerSpecial(character, gameTime);
                                                    }
                                                }
                                            }
                                            world.level.attacked = true;
                                            resetConfirmation();
                                            foreach (Character character in toBeKilled)
                                            {
                                                world.level.grid.characters.list.Remove(character);
                                            }
                                        }
                                        else if (player.job == 4)// thief special
                                        {
                                            Character toSkipTurn = null;
                                            foreach (Character character in world.level.grid.characters.list)
                                            {
                                                if (character != player && !world.level.attacked && cube.gridPos == character.gridPos &&
                                                    player.InMagicRange(character.gridPos))
                                                {
                                                    toSkipTurn = player.ThiefSpecial(character, gameTime);

                                                    world.level.attacked = true;
                                                    resetConfirmation();
                                                }
                                            }
                                            if (toSkipTurn != null)
                                            {
                                                world.level.grid.characters.list.Remove(toSkipTurn);
                                                world.level.grid.characters.list.Add(toSkipTurn);
                                                world.level.setupTurnOrderIcons();
                                            }
                                        }
                                        else if (player.job == 0)// soldier special
                                        {
                                            foreach (Character character in world.level.grid.characters.list)
                                            {
                                                if (character != player && !world.level.attacked && cube.gridPos == character.gridPos &&
                                                    player.InMagicRange(character.gridPos))
                                                {
                                                    player.SoldierSpecial(character, gameTime);

                                                    world.level.attacked = true;
                                                    resetConfirmation();
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Not enough mp to cast!");
                                    }
                                }
                                else
                                {
                                    Character toBeKilled = null;
                                    foreach (Character character in world.level.grid.characters.list)
                                    {
                                        if (cube.gridPos == character.gridPos)
                                        {

                                            toBeKilled = player.attack(character, gameTime);

                                            world.level.attacked = true;
                                            resetConfirmation();
                                        }
                                    }
                                    if (toBeKilled != null)
                                    {
                                        world.level.grid.characters.list.Remove(toBeKilled);
                                    }
                                }
                            }
                            /*else if (selectedAction == "move" && player.inMoveRange(cube.gridPos, world.level) &&
                                !world.level.moved) // Move
                            {
                                if (world.level.grid.isVacant(cube.gridPos))
                                {
                                    player.Move(cube.gridPos);
                                    //world.level.grid.onCharacterMoved();
                                    world.level.moved = true;
                                    world.level.rotated = false;
                                    resetConfirmation();
                                }
                            }*/
                        }


                    }
                }
                if (!mouseInBounds)
                {
                    world.level.grid.clearTransparencies();
                    world.level.grid.onCharacterMoved(world.level);
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

        public void resetConfirmation()
        {
            selectedAction = "";
            confirmAction = false;
            lastClickMouseState = Vector2.Zero;
            world.level.SetConfirmationText("");
        }
    }
}
