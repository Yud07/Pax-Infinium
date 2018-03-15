using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Linq;
using pax_infinium.Enum;
using pax_infinium.Buttons;

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
        public Descriptor activeDescriptor;
        public TimeSpan descriptorTimer;

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

            World.textureManager.Load("Slash");
            World.textureManager.Load("Heal");

            World.textureManager.Load("AccuracyDown");
            World.textureManager.Load("Arrow");
            World.textureManager.Load("Lightning");
            World.textureManager.Load("Magic");
            World.textureManager.Load("Shield");
            World.textureManager.Load("Skip");

            // create 1x1 texture for line drawing
            world.oneByOne = new Texture2D(GraphicsDevice, 1, 1);
            world.oneByOne.SetData<Color>(
                new Color[] { Color.White });// fill the texture with white

            world.textureConverter = new TextureConverter(graphics.GraphicsDevice);
            //tex2 = world.textureConverter.Rotate90(tex2);
            //tex = world.textureConverter.Rotate(tex, 45);

            //world.level = new Level(graphics, "The world is mine!");
            world.level = new Level(graphics, world.RandomString(18));
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

            if (activeDescriptor != null)
            {
                activeDescriptor.trigger = false;
            }
            bool checkDescriptors = false;
            foreach (Descriptor d in world.level.descriptors)
            {
                if (d.poly.Contains(mouseState.Position.ToVector2()))
                {
                    checkDescriptors = true;
                    if (d == activeDescriptor)
                    {
                        if (gameTime.TotalGameTime > descriptorTimer)
                        {
                            d.trigger = true;
                        }
                    }
                    else
                    {
                        activeDescriptor = d;
                        descriptorTimer = gameTime.TotalGameTime + new TimeSpan(0, 0, 2);
                    }
                }
            }
            if (!checkDescriptors)
            {
                activeDescriptor = null;
            }

            bool clickedAButton = false;
            // saves last click state
            if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                //printRelevantInfo();
                //lastClickMouseState = transformedMouseState;
                if (world.state == 0)
                {
                    resetConfirmation(); // Fixes highlighting bug
                    world.state = 1;
                }
                else
                {
                    //bool clickedAButton = false;
                    foreach (IButton b in world.level.buttons)
                    {
                        if (b.GetPoly().Contains(mouseState.Position.ToVector2()))
                        {
                            //resetConfirmation();
                            //Console.WriteLine("B4");
                            //printRelevantInfo();
                            b.Click();
                            //Console.WriteLine("After");
                            //printRelevantInfo();
                            clickedAButton = true;
                        }
                    }

                    if (!clickedAButton)
                    {
                        lastClickMouseState = transformedMouseState;
                    }
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

            if (world.state == 1 && !world.level.drawBVD)
            {
                if (world.level.grid.characters.list[0].movePath.Count == 0)
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
                }

                if (keyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up))
                {
                    world.level.grid.peel++;
                    if (world.level.grid.peel > world.level.grid.height - 1)
                    {
                        world.level.grid.peel = world.level.grid.height - 1;
                    }
                    world.level.recalcPeelStatus();
                    //Console.WriteLine("Peel=" + world.level.grid.peel);
                }

                if (keyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down))
                {
                    world.level.grid.peel--;
                    if (world.level.grid.peel < 0)
                    {
                        world.level.grid.peel = 0;
                    }
                    world.level.recalcPeelStatus();
                    //Console.WriteLine("Peel=" + world.level.grid.peel);
                }

                // if there are characters
                if (world.level.grid.characters.list.Count > 0)
                {
                    player = world.level.grid.characters.list[0];

                    bool attackTrigger = world.level.attackButton.GetTrigger();
                    bool specialTrigger = world.level.specialButton.GetTrigger();
                    bool moveTrigger = world.level.moveButton.GetTrigger();
                    bool endTurnTrigger = world.level.endTurnButton.GetTrigger();
                    bool cancelTrigger = world.level.cancelButton.GetTrigger();
                    bool confirmTrigger = world.level.confirmButton.GetTrigger();

                    // Attack key w/ confirmation
                    if ((keyboardState.IsKeyDown(Keys.A) && previousKeyboardState.IsKeyUp(Keys.A) || attackTrigger) && !world.level.attacked)
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
                    else if ((keyboardState.IsKeyDown(Keys.S) && previousKeyboardState.IsKeyUp(Keys.S) || specialTrigger) && !world.level.attacked)
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
                    else if ((keyboardState.IsKeyDown(Keys.M) && previousKeyboardState.IsKeyUp(Keys.M) || moveTrigger))
                    {
                        if (!world.level.moved)
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
                        else
                        {
                            if (selectedAction == "" && !world.level.attacked) // undo movement
                            {
                                player.movePath.Clear();
                                player.Move(world.level.movedFrom, true);
                                world.level.moved = false;
                                world.level.rotated = false;
                            }
                        }
                    }
                    // Confirm key
                    else if ((keyboardState.IsKeyDown(Keys.Y) && previousKeyboardState.IsKeyUp(Keys.Y)) || confirmTrigger)
                    {
                        //Console.WriteLine("B4 confirm");
                        //printRelevantInfo();
                        confirmAction = true;
                        //printRelevantInfo();
                        //Console.WriteLine("After confirm");
                    }
                    // Cancel key
                    else if ((keyboardState.IsKeyDown(Keys.N) && previousKeyboardState.IsKeyUp(Keys.N)) || cancelTrigger)
                    {
                        //Console.WriteLine("B4 cancel");
                        //printRelevantInfo();
                        if (selectedAction == "" && world.level.moved && !world.level.attacked) // undo movement
                        {
                            player.Move(world.level.movedFrom, true);
                            world.level.moved = false;
                            world.level.rotated = false;
                        }
                        else
                        {
                            resetConfirmation();
                        }
                        //printRelevantInfo();
                        //Console.WriteLine("After cancel");
                    }
                    else if (keyboardState.IsKeyDown(Keys.I) && previousKeyboardState.IsKeyUp(Keys.I)){
                        world.level.drawInfo = !world.level.drawInfo;
                    }

                    foreach (Cube cube in world.level.grid.cubes) // clear highlights
                    {
                        cube.highLight = false;
                        if (player.gridPos == cube.gridPos && gameTime.TotalGameTime.Seconds%2 == 0 && player.team == 1)
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
                        if ((player.job == EJob.Mage || player.job == EJob.Healer) && selectedAction == "special" && !world.level.attacked)
                        { // Black Mage or healer
                            Cube tempCube = null;
                            foreach (Cube cube in world.level.grid.cubes)
                            {
                                if (player.InMagicRange(cube.gridPos))
                                {
                                    cube.highLight = true;

                                    bool topVisible = true;
                                    foreach (Cube c in world.level.grid.cubes)
                                    {
                                        if (c != cube && c.gridPos.Z < world.level.grid.peel)
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

                                    // if the player is in magic range and standing on a cube the mouse is over
                                    // and the cube is exposed
                                    if (cube.topPoly.Contains(activeMouseState) && world.level.grid.TopExposed(cube.gridPos) && topVisible)
                                    {
                                        if (tempCube == null)
                                        {
                                            tempCube = cube;
                                            foreach (Cube c in world.level.grid.cubes)
                                            {
                                                if ((c.isAdjacent(cube.gridPos) || c.gridPos == cube.gridPos))
                                                {
                                                    c.invert = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (tempCube.DrawOrder() <= cube.DrawOrder())
                                            {
                                                tempCube = cube;
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
                                }
                            }
                        }
                        else if (selectedAction == "attack" && !world.level.attacked) // WAttack highlight
                        {
                            if (player.job == EJob.Hunter) // Hunter
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
                                if (world.level.validMoveSpaces.Contains(cube.gridPos))
                                {
                                    cube.highLight = true;
                                    //highlightedCubes.Add(cube);
                                }
                            }
                        }
                        else if ((player.job == EJob.Soldier || player.job == EJob.Thief || player.job == EJob.Hunter) && selectedAction == "special" && !world.level.attacked) //thief special
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

                    if (((keyboardState.IsKeyDown(Keys.E) && previousKeyboardState.IsKeyUp(Keys.E)) || endTurnTrigger) && world.level.toBeKilled.Count() == 0) // end turn
                    {
                        resetConfirmation();
                        world.level.endTurn(gameTime);
                    }

                    world.level.attackButton.ResetTrigger();
                    world.level.specialButton.ResetTrigger();
                    world.level.moveButton.ResetTrigger();
                    world.level.endTurnButton.ResetTrigger();
                   //world.level.undoButton.ResetTrigger();
                    world.level.cancelButton.ResetTrigger();
                    world.level.confirmButton.ResetTrigger();

                    if (player.movePath.Count > 0)
                    {
                        world.level.grid.getCube(player.movePath.Last()).highLight = true;
                    }

                    bool mouseInBounds = false;

                    /*if (!clickedAButton)
                    {*/
                        // highlight scrolled over cube
                        foreach (Cube cube in world.level.grid.cubes) // SHOULD BE POSSIBLE TO ONLY CHECK THE CUBE THE MOUSE IS ABOVE
                        {
                            bool topVisible = true;
                            foreach (Cube c in world.level.grid.cubes)
                            {
                                if (c != cube && c.gridPos.Z < world.level.grid.peel)
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
                                world.level.grid.TopExposed(cube.gridPos) && topVisible)
                            {
                                mouseInBounds = true;
                                if (!cube.highLight)
                                {
                                    cube.highLight = true;
                                }
                                else
                                {
                                    cube.highLight = false;
                                    cube.invert = true;
                                }

                                world.level.grid.onHighlightMoved(cube);
                                world.level.grid.onCharacterMoved(world.level);
                                world.level.grid.peelCubes();


                                world.level.clearCharacter();
                                foreach (Character c in world.level.grid.characters.list)
                                {
                                    if (cube.gridPos == c.gridPos)
                                    {
                                        /*if (world.level.highlightedCharacter != c)
                                        {
                                            Console.WriteLine("Character Scale = " + c.sprite.scale);
                                        }*/
                                        world.level.setCharacter(c);
                                    }
                                }

                                if (!world.level.rotated && !world.triggerAIBool && player.movePath.Count == 0 && player.team == 1) //ai trigger check prevents accidental rotation on end turn
                                {
                                    player.Rotate(cube.gridPos, true);
                                }

                                if (!confirmAction && selectedAction != "" && lastClickMouseState != Vector2.Zero)
                                {
                                    if ((selectedAction == "special" || selectedAction == "attack") && !world.level.attacked) // Attack
                                    {
                                        if (selectedAction == "special")
                                        {
                                            if (player.CanCast(8))
                                            {
                                                if (player.job == EJob.Mage || player.job == EJob.Healer)
                                                {
                                                    foreach (Character character in world.level.grid.characters.list)
                                                    {
                                                        if (!world.level.attacked && (cube.isAdjacent(character.gridPos) || character.gridPos == cube.gridPos) &&
                                                            player.InMagicRange(cube.gridPos))
                                                        {
                                                            if (player.job == EJob.Mage)
                                                            {
                                                                int[] chanceDamage;
                                                                chanceDamage = player.calculateMageSpecial(character);
                                                                world.level.SetConfirmationText(chanceDamage[0], chanceDamage[1]);
                                                            }
                                                            else if (player.job == EJob.Healer)// healer
                                                            {
                                                                int health = 15;
                                                                world.level.SetConfirmationText("+" + health + "HP");
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (player.job == EJob.Thief)// thief special
                                                {
                                                    foreach (Character character in world.level.grid.characters.list)
                                                    {
                                                        if (character != player && !world.level.attacked && cube.gridPos == character.gridPos &&
                                                            player.InMagicRange(character.gridPos))
                                                        {
                                                            int chance = player.CalculateThiefSpecial(character);

                                                            world.level.SetConfirmationText("Chance " + chance + "% to Skip");
                                                        }
                                                    }
                                                }
                                                else if (player.job == EJob.Soldier)// soldier special
                                                {
                                                    foreach (Character character in world.level.grid.characters.list)
                                                    {
                                                        if (!world.level.attacked && cube.gridPos == character.gridPos &&
                                                            player.InMagicRange(character.gridPos))
                                                        {
                                                            int defenseBoost = 20;
                                                            world.level.SetConfirmationText("+" + defenseBoost + "WD");
                                                        }
                                                    }
                                                }
                                                else if (player.job == EJob.Hunter) // hunter special
                                                {
                                                    foreach (Character character in world.level.grid.characters.list)
                                                    {
                                                        if (!world.level.attacked && cube.gridPos == character.gridPos && 
                                                            player.InMagicRange(character.gridPos))
                                                        {

                                                        int chance = player.CalculateHunterSpecial(character);                                                        
                                                        int accuracyDrop = 5;
                                                            world.level.SetConfirmationText("Chance " + chance + "% -" + accuracyDrop + " ACC");
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
                                    else if (selectedAction == "move" && world.level.validMoveSpaces.Contains(cube.gridPos) &&//player.inMoveRange(cube.gridPos, world.level) &&
                                        !world.level.moved) // Move
                                    {
                                        world.level.grid.peel = world.level.grid.height - 1;
                                        //world.level.SetConfirmationText("Move? Confirm Y / N");
                                        world.level.movedFrom = player.gridPos;
                                        player.Move(cube.gridPos, world.level);
                                        //world.level.grid.onCharacterMoved();
                                        world.level.moved = true;
                                        world.level.rotated = false;
                                        resetConfirmation();
                                    }
                                }
                                else if (confirmAction && selectedAction != "" && lastClickMouseState != Vector2.Zero) // Confirmed
                                {
                                    if ((selectedAction == "special" || selectedAction == "attack") && !world.level.attacked) // Attack
                                    {
                                        if (selectedAction == "special")
                                        {
                                            if (player.CanCast(8))
                                            {
                                                player.payForCast(8, gameTime);

                                                if (player.job == EJob.Mage || player.job == EJob.Healer)
                                                {
                                                    foreach (Character character in world.level.grid.characters.list)
                                                    {
                                                        if (!world.level.attacked && (cube.isAdjacent(character.gridPos) || character.gridPos == cube.gridPos) &&
                                                            player.InMagicRange(cube.gridPos))
                                                        {
                                                            if (player.job == EJob.Mage)
                                                            {
                                                                Character result = player.MageSpecial(character, gameTime);
                                                                if (result != null)
                                                                {
                                                                    world.level.toBeKilled.Add(result);
                                                                }
                                                            }
                                                            else if (player.job == EJob.Healer)// healer
                                                            {
                                                                player.HealerSpecial(character, gameTime);
                                                            }
                                                        }
                                                    }
                                                    world.level.attacked = true;
                                                    resetConfirmation();
                                                }
                                                else if (player.job == EJob.Thief)// thief special
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
                                                else if (player.job == EJob.Soldier)// soldier special
                                                {
                                                    foreach (Character character in world.level.grid.characters.list)
                                                    {
                                                        if (!world.level.attacked && cube.gridPos == character.gridPos &&
                                                            player.InMagicRange(character.gridPos))
                                                        {
                                                            player.SoldierSpecial(character, gameTime);

                                                            world.level.attacked = true;
                                                            resetConfirmation();
                                                        }
                                                    }
                                                }
                                                else if (player.job == EJob.Hunter)
                                                {
                                                    foreach (Character character in world.level.grid.characters.list)
                                                    {
                                                        if (!world.level.attacked && cube.gridPos == character.gridPos &&
                                                            player.InMagicRange(character.gridPos))
                                                        {
                                                            player.HunterSpecial(character, gameTime);

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
                                            Character tBKill = null;
                                            foreach (Character character in world.level.grid.characters.list)
                                            {
                                                if (cube.gridPos == character.gridPos)
                                                {

                                                    tBKill = player.attack(character, gameTime);

                                                    world.level.attacked = true;
                                                    resetConfirmation();
                                                }
                                            }
                                            if (tBKill != null)
                                            {
                                                world.level.toBeKilled.Add(tBKill);
                                            }
                                        }
                                    }
                                }


                            }
                        }
                    if (!mouseInBounds)
                    {
                        if (!clickedAButton)
                        {
                            lastClickMouseState = Vector2.Zero;
                        }

                        world.level.grid.clearTransparencies();
                        world.level.grid.onCharacterMoved(world.level);
                        world.level.grid.peelCubes();
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

        public void printRelevantInfo()
        {
            Console.WriteLine("selectedAction: " + selectedAction + " confirmAction: " + confirmAction);
            Console.WriteLine("activeMouseState: " + activeMouseState.X + ", " + activeMouseState.Y + " lastClickMouseState: " + lastClickMouseState.X + ", " + lastClickMouseState.Y);
        }
    }
}
