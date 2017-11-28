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
            List<Cube> highlightedCubes = new List<Cube>();

            if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                lastClickMouseState = transformedMouseState;
            }

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

            if (keyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyUp(Keys.Left)) // Rotate Left
            {
                world.level.grid.rotate(false);
            }

            if (keyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyUp(Keys.Right)) // Rotate Right
            {
                world.level.grid.rotate(true);
            }

            if (world.level.grid.characters.list.Count > 0) //if there are characters
            {
                /*if (world.level.grid.characters.list.Count == 1)
                {*/
                    player = world.level.grid.characters.list[0];
                /*}
                else
                {
                    player = world.level.grid.characters.list[0];//world.level.turn % world.level.turnOrder.Length];
                }*/

                if (keyboardState.IsKeyDown(Keys.A) && previousKeyboardState.IsKeyUp(Keys.A))
                {
                    if (selectedAction != "")
                    {
                        selectedAction = "";
                        lastClickMouseState = Vector2.Zero;
                        world.level.SetConfirmationText("");
                    }
                    else
                    {
                        selectedAction = "attack";
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.S) && previousKeyboardState.IsKeyUp(Keys.S)) {
                    if (selectedAction != "")
                    {
                        selectedAction = "";
                        lastClickMouseState = Vector2.Zero;
                        world.level.SetConfirmationText("");
                    }
                    else
                    {
                        selectedAction = "special";
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.M) && previousKeyboardState.IsKeyUp(Keys.M))
                {
                    if (selectedAction != "")
                    {
                        selectedAction = "";
                        lastClickMouseState = Vector2.Zero;
                        world.level.SetConfirmationText("");
                    }
                    else
                    {
                        selectedAction = "move";
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.Y) && previousKeyboardState.IsKeyUp(Keys.Y)){
                    confirmAction = true;
                }
                else if (keyboardState.IsKeyDown(Keys.N) && previousKeyboardState.IsKeyUp(Keys.N))
                {
                    selectedAction = "";
                    lastClickMouseState = Vector2.Zero;
                    world.level.SetConfirmationText("");
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

                if (selectedAction != "")
                {
                    if ((player.job == 2 || player.job == 3) && selectedAction == "special" && !world.level.attacked)
                    { // Black Mage or healer or thief
                        foreach (Cube cube in world.level.grid.cubes)
                        {
                            if ((world.cubeDist(cube.gridPos, player.gridPos) <= player.magicRange))
                            {
                                cube.highLight = true;
                                highlightedCubes.Add(cube);

                            }

                            if (world.cubeDist(player.gridPos, cube.gridPos) <= player.magicRange && cube.topPoly.Contains(activeMouseState) &&
                            world.level.grid.topOfColumn(cube.gridPos) == cube.gridPos.Z)
                            {
                                foreach (Cube c in world.level.grid.cubes)
                                {
                                    if (world.cubeDist(c.gridPos, cube.gridPos) <= 1)
                                    {
                                        c.invert = true;
                                    }
                                }
                            }
                        }
                    }
                    else if (selectedAction == "attack" && !world.level.attacked) // WAttack highlight
                    {
                        if (player.job == 1)
                        {
                            foreach (Cube cube in world.level.grid.cubes)
                            {
                                if (world.cubeDist(cube.gridPos, player.gridPos) <= player.weaponRange)
                                {
                                    cube.highLight = true;
                                    highlightedCubes.Add(cube);
                                }
                            }
                        }
                        else
                        {
                            foreach (Cube cube in world.level.grid.cubes)
                            {
                                if (cube.isAdjacent(player.gridPos))
                                {
                                    cube.highLight = true;
                                    highlightedCubes.Add(cube);
                                }
                            }
                        }

                    }
                    else if (selectedAction == "move" && !world.level.moved)
                    { // move highlight
                        foreach (Cube cube in world.level.grid.cubes)
                        {
                            int cubeDist = world.cubeDist(cube.gridPos, player.gridPos);
                            if (cubeDist < player.move && cubeDist > 0 && world.level.grid.topOfColumn(cube.gridPos) == cube.gridPos.Z
                                && Math.Abs(cube.gridPos.Z - player.gridPos.Z) <= player.jump)
                            {
                                cube.highLight = true;
                                highlightedCubes.Add(cube);
                            }
                        }
                    }
                    else if ((player.job == 0 || player.job == 4) && selectedAction == "special" && !world.level.attacked) //thief special
                    {
                        foreach (Cube cube in world.level.grid.cubes)
                        {
                            if ((world.cubeDist(cube.gridPos, player.gridPos) <= player.magicRange))
                            {
                                cube.highLight = true;
                                highlightedCubes.Add(cube);
                            }
                        }
                    }
                }

                if (keyboardState.IsKeyDown(Keys.E) && previousKeyboardState.IsKeyUp(Keys.E)) // end turn
                {
                    world.level.endTurn();
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
                        world.level.grid.onCharacterMoved();

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
                            float x = player.gridPos.X - cube.gridPos.X;
                            float y = player.gridPos.Y - cube.gridPos.Y;
                            
                            if (Math.Abs(x) > Math.Abs(y))
                            {
                                if (x > 0)
                                {
                                    player.direction = "nw";
                                    player.sprite.tex = player.nwTex;
                                }
                                else
                                {
                                    player.direction = "se";
                                    player.sprite.tex = player.seTex;
                                }
                            }
                            else
                            {
                                if (y > 0)
                                {
                                    player.direction = "ne";
                                    player.sprite.tex = player.neTex;
                                }
                                else
                                {
                                    player.direction = "sw";
                                    player.sprite.tex = player.swTex;
                                }
                            }
                        }

                        if (!confirmAction && selectedAction != "" && lastClickMouseState != Vector2.Zero)
                        {
                            if ((((player.job == 2 || player.job == 3 || player.job == 4 || player.job == 0) && selectedAction == "special") || selectedAction == "attack") && !world.level.attacked) // Attack
                            {
                                if (selectedAction == "special")
                                {
                                    if (player.mp >= 8)
                                    {
                                        if (player.job == 2 || player.job == 3)
                                        {
                                            foreach (Character character in world.level.grid.characters.list)
                                            {
                                                if (!world.level.attacked && world.cubeDist(cube.gridPos, character.gridPos) <= 1 &&
                                                    world.cubeDist(player.gridPos, cube.gridPos) <= player.magicRange)
                                                {
                                                    if (player.job == 2)
                                                    {
                                                        int chance = 100 - character.evasion;
                                                        int spellModifier = 0;
                                                        int damage = (int)((player.MAttack + spellModifier - (character.MDefense / 2)) * .75);
                                                        world.level.SetConfirmationText(chance, damage);
                                                    }
                                                    else if (player.job == 3)// healer
                                                    {
                                                        int health = 40;
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
                                                    world.cubeDist(player.gridPos, character.gridPos) <= player.magicRange)
                                                {
                                                    int angleModifier = 0;
                                                    if (character.direction.Contains(player.direction[0]))
                                                        angleModifier++;
                                                    if (character.direction.Contains(player.direction[1]))
                                                        angleModifier++;

                                                    int chance = 45 - character.evasion + angleModifier * 5;

                                                    world.level.SetConfirmationText("Chance " + chance + "% Confirm Y / N");
                                                }
                                            }
                                        }
                                        else if (player.job == 0)// soldier special
                                        { 
                                            foreach (Character character in world.level.grid.characters.list)
                                            {
                                                if (character != player && !world.level.attacked && cube.gridPos == character.gridPos &&
                                                    world.cubeDist(player.gridPos, character.gridPos) <= player.magicRange)
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
                                else
                                {
                                    foreach (Character character in world.level.grid.characters.list)
                                    {
                                        bool canAttack;

                                        canAttack = character != player && !world.level.attacked && cube.gridPos == character.gridPos;

                                        if (player.job != 1)
                                        {
                                            canAttack = canAttack && cube.isAdjacent(player.gridPos);
                                        }
                                        else
                                        {
                                            canAttack = canAttack && world.cubeDist(player.gridPos, character.gridPos) <= player.weaponRange;
                                        }

                                        if (canAttack)
                                        {
                                            int angleModifier = 0;
                                            if (character.direction.Contains(player.direction[0]))
                                                angleModifier++;
                                            if (character.direction.Contains(player.direction[1]))
                                                angleModifier++;

                                            int chance = 90 - character.evasion + angleModifier*5;
                                            int damage = (int)((player.WAttack + player.WAttack * angleModifier/2 - (character.WDefense / 2)) * .5);
                                            world.level.SetConfirmationText(chance, damage);
                                        }
                                    }
                                }
                            }
                            else if (selectedAction == "move" &&
                                Game1.world.cubeDist(player.gridPos, cube.gridPos) < player.move &&
                                world.level.grid.topOfColumn(cube.gridPos) == cube.gridPos.Z &&
                                Math.Abs(cube.gridPos.Z - player.gridPos.Z) <= player.jump &&
                                !world.level.moved) // Move
                            {
                                bool vacant = true;
                                //Console.WriteLine();
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
                                    world.level.SetConfirmationText("Move? Confirm Y / N");
                                }
                            }
                        }
                        else if (confirmAction && selectedAction != "" && lastClickMouseState != Vector2.Zero) // Confirmed
                        {
                            if ((((player.job == 2 || player.job == 3 || player.job == 4 || player.job == 0) && selectedAction == "special") || selectedAction == "attack") && !world.level.attacked) // Attack
                            {
                                if (selectedAction == "special")
                                {
                                    if (player.mp >= 8)
                                    {                                        
                                        player.mp -= 8;
                                        player.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                                        player.text.Text = "-8";
                                        player.text.color = Color.LightBlue;

                                        if (player.job == 2 || player.job == 3)
                                        {
                                            //List<Character> toSkipTurn = new List<Character>();
                                            List<Character> toBeKilled = new List<Character>();
                                            foreach (Character character in world.level.grid.characters.list)
                                            {
                                                if (!world.level.attacked && world.cubeDist(cube.gridPos, character.gridPos) <= 1 &&
                                                    world.cubeDist(player.gridPos, cube.gridPos) <= player.magicRange)
                                                {
                                                    if (player.job == 2)
                                                    {
                                                        int chance = 100 - character.evasion;
                                                        if (chance >= World.Random.Next(1, 101))
                                                        {
                                                            int spellModifier = 0;
                                                            int damage = (int)((player.MAttack + spellModifier - (character.MDefense / 2)) * .75);
                                                            Console.WriteLine("Hit! " + character.name + " takes " + damage + " damage!");
                                                            character.health -= damage;
                                                            character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                                                            character.text.Text = "-" + damage;
                                                            character.text.color = Color.OrangeRed;

                                                            if (character.health <= 0)
                                                            {
                                                                Console.WriteLine(character.name + " has died!");
                                                                toBeKilled.Add(character);
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine(character.name + " health: " + character.health);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Miss!");
                                                            character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                                                            character.text.Text = "Miss";
                                                            character.text.color = Color.Green;
                                                        }
                                                    }
                                                    else if (player.job == 3)// healer
                                                    {
                                                        int health = 40;
                                                        Console.WriteLine(character.name + " heals " + health + " points!");
                                                        character.health += health;
                                                        character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                                                        character.text.Text = "+" + health;
                                                        character.text.color = Color.Green;
                                                    }
                                                }
                                            }
                                            world.level.attacked = true;
                                            confirmAction = false;
                                            lastClickMouseState = Vector2.Zero;
                                            world.level.SetConfirmationText("");
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
                                                    world.cubeDist(player.gridPos, character.gridPos) <= player.magicRange)
                                                {
                                                    int angleModifier = 0;
                                                    if (character.direction.Contains(player.direction[0]))
                                                        angleModifier++;
                                                    if (character.direction.Contains(player.direction[1]))
                                                        angleModifier++;

                                                    int chance = 45 - character.evasion + angleModifier * 5;
                                                    Console.WriteLine("Chance to hit: " + chance + "%");
                                                    if (chance >= World.Random.Next(1, 101))
                                                    {
                                                        toSkipTurn = character;
                                                        Console.WriteLine(character.name + " had their next turn stolen!");
                                                        character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                                                        character.text.Text = "Skipped!";
                                                        character.text.color = Color.OrangeRed;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Miss!");
                                                        character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                                                        character.text.Text = "Miss";
                                                        character.text.color = Color.Green;
                                                    }

                                                    world.level.attacked = true;
                                                    confirmAction = false;
                                                    lastClickMouseState = Vector2.Zero;
                                                    world.level.SetConfirmationText("");
                                                }
                                            }
                                            if (toSkipTurn != null)
                                            {
                                                world.level.grid.characters.list.Remove(toSkipTurn);
                                                world.level.grid.characters.list.Add(toSkipTurn);
                                                Game1.world.level.setupTurnOrderIcons();
                                            }
                                        }
                                        else if (player.job == 0)// soldier special
                                        {
                                            foreach (Character character in world.level.grid.characters.list)
                                            {
                                                if (character != player && !world.level.attacked && cube.gridPos == character.gridPos &&
                                                    world.cubeDist(player.gridPos, character.gridPos) <= player.magicRange)
                                                {
                                                    int defenseBoost = 20;
                                                    Console.WriteLine(character.name + " gains " + defenseBoost + " melee defense!");
                                                    character.MDefense += defenseBoost;
                                                    character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                                                    character.text.Text = "+" + defenseBoost;
                                                    character.text.color = Color.Yellow;

                                                    world.level.attacked = true;
                                                    confirmAction = false;
                                                    lastClickMouseState = Vector2.Zero;
                                                    world.level.SetConfirmationText("");
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
                                        bool canAttack;

                                        canAttack = character != player && !world.level.attacked && cube.gridPos == character.gridPos;

                                        if (player.job != 1)
                                        {
                                            canAttack = canAttack && cube.isAdjacent(player.gridPos);
                                        }
                                        else
                                        {
                                            canAttack = canAttack && world.cubeDist(player.gridPos, character.gridPos) <= player.weaponRange;
                                        }

                                        if (canAttack)
                                        {
                                            int angleModifier = 0;
                                            if (character.direction.Contains(player.direction[0]))
                                                angleModifier++;
                                            if (character.direction.Contains(player.direction[1]))
                                                angleModifier++;

                                            int chance = 90 - character.evasion + angleModifier * 5;
                                            Console.WriteLine("Chance to hit: " + chance + "%");
                                            if (chance >= World.Random.Next(1, 101))
                                            {

                                                int damage = (int)((player.WAttack + player.WAttack * angleModifier / 2 - (character.WDefense / 2)) * .5);
                                                Console.WriteLine("Hit! " + character.name + " takes " + damage + " damage!");
                                                character.health -= damage;
                                                character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                                                character.text.Text = "-" + damage;
                                                character.text.color = Color.OrangeRed;

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
                                                character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                                                character.text.Text = "Miss";
                                                character.text.color = Color.Green;
                                            }

                                            world.level.attacked = true;
                                            confirmAction = false;
                                            lastClickMouseState = Vector2.Zero;
                                            world.level.SetConfirmationText("");
                                        }
                                    }
                                    if (toBeKilled != null)
                                    {
                                        world.level.grid.characters.list.Remove(toBeKilled);
                                    }
                                }
                            }
                            else if (selectedAction == "move" &&
                                Game1.world.cubeDist(player.gridPos, cube.gridPos) < player.move &&
                                world.level.grid.topOfColumn(cube.gridPos) == cube.gridPos.Z &&
                                Math.Abs(cube.gridPos.Z - player.gridPos.Z) <= player.jump &&
                                !world.level.moved) // Move
                            {
                                bool vacant = true;
                                //Console.WriteLine();
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
                                    //world.level.grid.onCharacterMoved();
                                    world.level.moved = true;
                                    confirmAction = false;
                                    lastClickMouseState = Vector2.Zero;
                                    world.level.rotated = false;
                                    world.level.SetConfirmationText("");
                                }
                            }
                        }


                    }
                }
                if (!mouseInBounds)
                {
                    world.level.grid.clearTransparencies();
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
