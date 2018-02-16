using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using MCTS.Interfaces;
using MCTS.Enum;
using pax_infinium.Enum;
using pax_infinium.Buttons;
using pax_infinium.AStar;

namespace pax_infinium
{
    public class Level : IGameState
    {
        private Random random;
        public string seed;
        public Grid grid;
        public int turn;
        public bool moved;
        public bool attacked;
        public bool rotated;
        public TextItem text;
        //public string[] turnOrder;

        public Background background;
        public int perspective;

        public TextItem playerName;
        public TextItem playerStatus;
        public Sprite playerFace;

        public TextItem characterName;
        public TextItem characterStatus;
        public Sprite characterFace;
        public Character highlightedCharacter;

        public Sprite playerStatusIcons;
        public Sprite characterStatusIcons;

        public List<Sprite> turnOrderIcons;
        public List<Sprite> turnOrderTeamIcons;

        public TextItem confirmationText;
        public Sprite confirmationSprite;

        public List<Player> players = new List<Player>();

        public String name = "Level";

        public Vector3 movedFrom;

        public Background startScreen;
        public TextItem title;
        public TextItem playGame;
        public TextItem options;
        public TextItem quit;

        public Sprite actionsFrame;
        public TextItem moveAction;
        public TextItem attackAction;
        public TextItem specialAction;
        public TextItem endTurnAction;

        public Sprite thoughtBubble;
        public Sprite faceThoughtBubble;
        public bool drewThoughtBubble;

        public TextItem battleVictoryDefeat;
        public TimeSpan startTime;
        public bool drawBVD;

        public bool firstRotate;

        public Sprite Compass;
        public TextItem N;
        public TextItem E;
        public TextItem S;
        public TextItem W;
        public IButton leftButton;
        public IButton rightButton;


        public Sprite teamZeroHealth;
        public TextItem zeroHealthText;
        public Sprite teamOneHealth;
        public TextItem oneHealthText;

        public bool drawInfo;

        public TextItem peelStatusText;
        public List<Cube> peelStatus;
        public IButton upButton;
        public IButton downButton;

        public Sprite playerHealthBar;
        public TextItem playerHealthText;
        public Sprite characterHealthBar;
        public TextItem characterHealthText;

        public Sprite playerMagicBar;
        public TextItem playerMagicText;
        public Sprite characterMagicBar;
        public TextItem characterMagicText;

        public List<IButton> buttons;

        public IButton moveButton;
        public IButton attackButton;
        public IButton specialButton;
        public IButton endTurnButton;
        public IButton undoButton;
        public IButton confirmButton;
        public IButton cancelButton;

        public List<Vector3> validMoveSpaces;
        public List<List<Vector3>> validMovePaths;

        public Level(GraphicsDeviceManager graphics, string seed)
        {
            startScreen = new Background(World.textureManager["Start Screen"], graphics.GraphicsDevice.Viewport);

            title = new TextItem(World.fontManager["Trajanus Roman 64"], "Pax Infinium");
            title.position = new Vector2(1640, 540);
            title.scale = 1;

            playGame = new TextItem(World.fontManager["Trajanus Roman 64"], "Play Game");
            playGame.position = new Vector2(1690, 640);
            playGame.scale = 1;

            options = new TextItem(World.fontManager["Trajanus Roman 64"], "Options");
            options.position = new Vector2(1750, 740);
            options.scale = 1;

            quit = new TextItem(World.fontManager["Trajanus Roman 64"], "Quit");
            quit.position = new Vector2(1810, 840);
            quit.scale = 1;

            random = World.Random;
            int c1, c2, c3, c4;
            c1 = 1;//1;// random.Next(0,5);
            c2 = 1;//1;// random.Next(0,5);
            c3 = 1;//1;// random.Next(0,5);
            c4 = 0;// random.Next(0,5);
            grid = new Grid(graphics, seed, 10, 10, 5, c1, c2, c3, c4, random);
            background = new Background(Game1.world.textureConverter.GenRectangle(1920, 1080, Color.SkyBlue), graphics.GraphicsDevice.Viewport);
            players.Add(new Player("AI"));
            players.Add(new Player("Human"));
            grid.characters = new Characters();
            grid.characters.AddCharacter("Green Soldier", EJob.Soldier, 0, grid.origin, graphics);
            grid.characters.AddCharacter("Purple Soldier", EJob.Soldier, 1, grid.origin, graphics);
            grid.characters.AddCharacter("Green Hunter", EJob.Hunter, 0, grid.origin, graphics);
            grid.characters.AddCharacter("Purple Hunter", EJob.Hunter, 1, grid.origin, graphics);
            grid.characters.AddCharacter("Green Mage", EJob.Mage, 0, grid.origin, graphics);
            grid.characters.AddCharacter("Purple Mage", EJob.Mage, 1, grid.origin, graphics);
            grid.characters.AddCharacter("Green Healer", EJob.Healer, 0, grid.origin, graphics);
            grid.characters.AddCharacter("Purple Healer", EJob.Healer, 1, grid.origin, graphics);
            grid.characters.AddCharacter("Green Thief", EJob.Thief, 0, grid.origin, graphics);
            grid.characters.AddCharacter("Purple Thief", EJob.Thief, 1, grid.origin, graphics);
            grid.characters.list.Sort(Character.CompareBySpeed);
            grid.characters.list.Reverse();
            grid.placeCharacters();
            turn = 0;
            moved = false;
            attacked = false;
            rotated = false;
            text = new TextItem(World.fontManager["Trajanus Roman 36"], "Turn " + turn.ToString());
            text.position = new Vector2(100, 30);
            text.color = Color.Black;
            text.scale = 1f;

            CalcValidMoveSpaces();

            playerName = new TextItem(World.fontManager["Trajanus Roman 36"], grid.characters.list[0].name);
            playerName.origin = Vector2.Zero;
            playerName.position = new Vector2(260, 750);            
            playerName.scale = 1f;

            String t = grid.characters.list[0].health + "               " + grid.characters.list[0].mp;
            t += "\n\n\n" + grid.characters.list[0].move + "                  " + grid.characters.list[0].jump;
            t += "\n\n\n" + grid.characters.list[0].speed + "               " + grid.characters.list[0].evasion;
            t += "\n\n\n" + grid.characters.list[0].WAttack + "               " + grid.characters.list[0].MAttack;
            t += "\n\n\n" + grid.characters.list[0].WDefense + "               " + grid.characters.list[0].MDefense;
            t += "\n\n\n" + grid.characters.list[0].weaponRange + "                  " + grid.characters.list[0].magicRange;
            playerStatus = new TextItem(World.fontManager["InfoFont"], t);
            playerStatus.position = new Vector2(185, 500);            
            playerStatus.scale = 1f;

            if (grid.characters.list[0].team == 0)
            {
                playerName.color = Color.Green;
                playerStatus.color = Color.Green;
            }
            else
            {
                playerName.color = Color.Purple;
                playerStatus.color = Color.Purple;
            }

            playerFace = new Sprite(grid.characters.list[0].faceLeft);
            //playerFace = new Sprite(Game1.world.textureConverter.GenRectangle(90, 160, Color.Blue));
            playerFace.position = new Vector2(playerFace.tex.Width * 2, playerFace.tex.Height * 5 - 80);
            playerFace.scale = 4;

            characterName = new TextItem(World.fontManager["Trajanus Roman 36"], grid.characters.list[0].name);
            characterName.Text = "";
            characterName.origin = Vector2.Zero;
            characterName.position = new Vector2(1920*2/3, 970);

            characterStatus = new TextItem(World.fontManager["InfoFont"], t);
            characterStatus.Text = "";
            characterStatus.position = new Vector2(1795, 500);
            characterStatus.scale = 1f;

            characterFace = new Sprite(grid.characters.list[0].faceRight);
            //characterFace = new Sprite(Game1.world.textureConverter.GenRectangle(90, 160, Color.Blue));
            characterFace.position = new Vector2(characterFace.tex.Width * 18, characterFace.tex.Height * 5 - 80);
            characterFace.scale = 4;

            playerStatusIcons = new Sprite(World.textureManager["Status Icons"]);
            playerStatusIcons.position = new Vector2(playerStatusIcons.tex.Width / 10, playerStatusIcons.tex.Height / 5.1f);
            playerStatusIcons.scale = .15f;

            characterStatusIcons = new Sprite(World.textureManager["Status Icons"]);
            characterStatusIcons.position = new Vector2(characterStatusIcons.tex.Width * 1.45f, characterStatusIcons.tex.Height / 5.1f);
            characterStatusIcons.scale = .15f;

            turnOrderIcons = new List<Sprite>();
            turnOrderTeamIcons = new List<Sprite>();
            setupTurnOrderIcons();

            thoughtBubble = new Sprite(World.textureManager["Thought Bubble"]);
            thoughtBubble.scale = 2;

            faceThoughtBubble = new Sprite(World.textureManager["Thought Bubble"]);
            faceThoughtBubble.scale = 6;
            faceThoughtBubble.position = new Vector2(130, 650);

            drewThoughtBubble = false;

            battleVictoryDefeat = new TextItem(World.fontManager["Trajanus Roman 64"], "Battle");
            battleVictoryDefeat.position = new Vector2(960, 540);
            battleVictoryDefeat.color = Color.Black;
            battleVictoryDefeat.scale = 3;

            startTime = TimeSpan.Zero;
            drawBVD = false;
            firstRotate = false;

            Cube middleCube = grid.cubes[grid.cubes.Count / 2];
            middleCube = grid.getCube((int)middleCube.gridPos.X, (int)middleCube.gridPos.Y, grid.topOfColumn(middleCube.gridPos));
            Compass = new Sprite(middleCube.topTex);
            Compass.position = new Vector2(1800, 200);
            N = new TextItem(World.fontManager["Trajanus Roman 36"], "N");
            N.position = Compass.position - new Vector2(0, 64);
            E = new TextItem(World.fontManager["Trajanus Roman 36"], "E");
            E.position = Compass.position + new Vector2(85, 0);
            S = new TextItem(World.fontManager["Trajanus Roman 36"], "S");
            S.position = Compass.position + new Vector2(0, 64);
            W = new TextItem(World.fontManager["Trajanus Roman 36"], "W");
            W.position = Compass.position - new Vector2(100, 0);
            N.color = E.color = S.color = W.color = Color.Black;
            leftButton = new LeftButton(Compass.position + new Vector2(-105, 40));
            rightButton = new RightButton(Compass.position + new Vector2(68, 40));

            zeroHealthText = new TextItem(World.fontManager["Trajanus Roman 36"], "1000 HP");
            oneHealthText = new TextItem(World.fontManager["Trajanus Roman 36"], "1000 HP");
            recalcTeamHealthBar();

            drawInfo = false;

            peelStatusText = new TextItem(World.fontManager["Trajanus Roman 36"], "4/4");
            peelStatusText.color = Color.Black;
            peelStatusText.position = new Vector2(90, 150);
            upButton = new UpButton(new Vector2(5, 60));
            downButton = new DownButton(new Vector2(5, 205));
            initPeelStatus(middleCube);
           
            playerHealthText = new TextItem(World.fontManager["Trajanus Roman 24"], grid.characters.list[0].startingHealth + " HP");
            characterHealthText = new TextItem(World.fontManager["Trajanus Roman 24"], grid.characters.list[0].startingHealth + " HP");
            playerMagicText = new TextItem(World.fontManager["Trajanus Roman 24"], grid.characters.list[0].startingMP + " MP");
            characterMagicText = new TextItem(World.fontManager["Trajanus Roman 24"], grid.characters.list[0].startingMP + " MP");

            recalcStatusBars();

            Vector2 actionButtonsPos = new Vector2(260, 780);
            moveButton = new MoveButton(actionButtonsPos);
            attackButton = new AttackButton(actionButtonsPos + new Vector2(0, 55));
            specialButton = new SpecialButton(actionButtonsPos + new Vector2(0, 55 * 2));
            endTurnButton = new EndTurnButton(actionButtonsPos + new Vector2(0, 55 * 3));
            undoButton = new UndoButton(actionButtonsPos + new Vector2(305, 0));
            confirmButton = new ConfirmButton(new Vector2(1920/2 - 125, 1080 - 110));
            cancelButton = new CancelButton(new Vector2(1920 / 2 - 125, 1080 - 55));

            buttons = new List<IButton>();
            buttons.Add(upButton);
            buttons.Add(downButton);
            buttons.Add(leftButton);
            buttons.Add(rightButton);
            buttons.Add(moveButton);
            buttons.Add(attackButton);
            buttons.Add(specialButton);
            buttons.Add(endTurnButton);
            buttons.Add(undoButton);
            buttons.Add(confirmButton);
            buttons.Add(cancelButton);
        }

        public void recalcTeamHealthBar()
        {
            int zeroHealth = 0;
            int oneHealth = 0;
            foreach(Character character in grid.characters.list)
            {
                if (character.team == 0)
                {
                    zeroHealth += character.health;
                }
                else
                {
                    oneHealth += character.health;
                }
            }

            int zeroSize = 800 * zeroHealth / (zeroHealth + oneHealth);
            int oneSize = 800 * oneHealth / (zeroHealth + oneHealth);
            teamZeroHealth = new Sprite(Game1.world.textureConverter.GenBorderedRectangle(zeroSize, 50, Color.Green));
            teamZeroHealth.position = new Vector2(1920 / 2.75f - zeroSize / 2, 25);
            teamOneHealth = new Sprite(Game1.world.textureConverter.GenBorderedRectangle(oneSize, 50, Color.Purple));
            teamOneHealth.position = new Vector2(1920 / 2.75f + oneSize / 2, 25);


            zeroHealthText.Text = zeroHealth.ToString() + " HP";
            zeroHealthText.position = new Vector2(zeroSize/5 + teamZeroHealth.position.X, 27);
            oneHealthText.Text = oneHealth.ToString() + " HP";
            oneHealthText.position = new Vector2(-oneSize/5 + teamOneHealth.position.X, 27);
        }

        public void initPeelStatus(Cube middleCube)
        {
            peelStatus = new List<Cube>();
            for(int i = 0; i < grid.height; i++)
            {
                Cube c = grid.getCube((int)middleCube.gridPos.X, (int)middleCube.gridPos.Y, i);
                int tries = 0;
                while (c == null && tries < 10)
                {
                    int x = random.Next(grid.width);
                    int y = random.Next(grid.height);
                    c = grid.getCube(x, y, i);
                    tries++;
                }
                if (c != null)
                {
                    c = new Cube(new Vector2(25, 175 - i * .25f * (c.southwestTex.Height + c.topTex.Height)/2), c.southwestTex, c.southeastTex, c.topTex);
                }
                peelStatus.Add(c);
            }
        }
        
        public void recalcPeelStatus()
        {
            peelStatusText.Text = grid.peel + "/" + (grid.height - 1);
            int count = 0;
            foreach(Cube c in peelStatus)
            {
                if (c != null)
                {
                    if (count > grid.peel)
                    {
                        c.top.alpha = 0;
                        c.southwest.alpha = 0;
                        c.southeast.alpha = 0;
                    }
                    else
                    {
                        c.top.alpha = 1;
                        c.southwest.alpha = 1;
                        c.southeast.alpha = 1;
                    }
                }
                count++;
            }
        }

        public void recalcStatusBars()
        {
            int height = 40;
            int yOffset = 20;
            float fullBarSize = 1920 / 3;
            float playerHealthRatio = (float)grid.characters.list[0].health / (float)grid.characters.list[0].startingHealth;
            float playerMPRatio = (float)grid.characters.list[0].mp / (float)grid.characters.list[0].startingMP;

            playerHealthBar = new pax_infinium.Sprite(Game1.world.textureConverter.GenBorderedRectangle((int)(fullBarSize * playerHealthRatio), height, Color.Red));
            playerHealthBar.position = new Vector2((fullBarSize * playerHealthRatio)/2, 1080 - height / 2);
            playerHealthText.Text = grid.characters.list[0].health + " HP";
            playerHealthText.position = playerHealthBar.position;
            playerHealthText.position.Y += 2;

            playerMagicBar = new pax_infinium.Sprite(Game1.world.textureConverter.GenBorderedRectangle((int)(fullBarSize * playerMPRatio), height, Color.Blue));
            playerMagicBar.position = new Vector2((fullBarSize * playerMPRatio) / 2, 1080 - height - yOffset);
            playerMagicText.Text = grid.characters.list[0].mp + " MP";
            playerMagicText.position = playerMagicBar.position;
            playerMagicText.position.Y += 2;

            if (characterName.Text != "")
            {
                float characterHealthRatio = (float)highlightedCharacter.health / (float)highlightedCharacter.startingHealth;
                float characterMPRatio = (float)highlightedCharacter.mp / (float)highlightedCharacter.startingMP;

                characterHealthBar = new pax_infinium.Sprite(Game1.world.textureConverter.GenBorderedRectangle((int)(fullBarSize * characterHealthRatio), height, Color.Red));
                characterHealthBar.position = new Vector2(1920 - (fullBarSize * characterHealthRatio)/2, 1080 - height / 2);
                characterHealthText.Text = highlightedCharacter.health + " HP";
                characterHealthText.position = characterHealthBar.position;
                characterHealthText.position.Y += 2;

                characterMagicBar = new pax_infinium.Sprite(Game1.world.textureConverter.GenBorderedRectangle((int)(fullBarSize * characterMPRatio), height, Color.Blue));
                characterMagicBar.position = new Vector2(1920 - (fullBarSize * characterMPRatio)/2, 1080 - height - yOffset);
                characterMagicText.Text = highlightedCharacter.mp + " MP";
                characterMagicText.position = characterMagicBar.position;
                characterMagicText.position.Y += 2;
            }
        }

        public void RotateAStarPaths(bool clockwise)
        {
            double degrees;

            if (clockwise)
            {
                degrees = 90.0;
            }
            else
            {
                degrees = -90.0;
            }

            List<Vector3> tempVectList = new List<Vector3>();
            List<List<Vector3>> tempPathList = new List<List<Vector3>>();
            foreach(Vector3 v in validMoveSpaces)
            {
                Point newCoords = Game1.world.rotate(new Point((int)v.X, (int)v.Y), grid.deg2Rad(degrees), new Point((int)grid.width / 2 - 1, (int)grid.depth / 2 - 1));
                tempVectList.Add(new Vector3(newCoords.X, newCoords.Y, v.Z));
                List<Vector3> tempVList = new List<Vector3>();
                foreach(Vector3 vect in validMovePaths.First()){
                    Point newCoordinates = Game1.world.rotate(new Point((int)vect.X, (int)vect.Y), grid.deg2Rad(degrees), new Point((int)grid.width / 2 - 1, (int)grid.depth / 2 - 1));
                    tempVList.Add(new Vector3(newCoordinates.X, newCoordinates.Y, vect.Z));
                }
                tempPathList.Add(tempVList);
                validMovePaths.RemoveAt(0);
            }

            validMoveSpaces = tempVectList;
            validMovePaths = tempPathList;
        }

        public void Update(GameTime gameTime)
        {
            /*if (moved == true)
            {
                turn++;
                //Console.WriteLine("turn: " + turn);
                text.Text = turnOrder[turn % turnOrder.Length] + "'s turn:" + turn.ToString();
                if (text.color == Color.Blue)
                {
                    text.color = Color.Red;
                }
                else
                {
                    text.color = Color.Blue;
                }
                grid.onCharacterMoved();
                moved = false;
                attacked = false;
            }*/
            //grid.Update(gameTime);
            grid.characters.Update(gameTime);
            if (Game1.world.state == 1)
            {
                if (startTime == TimeSpan.Zero)
                {
                    startTime = gameTime.TotalGameTime;
                }
                else
                {
                    if (gameTime.TotalGameTime - startTime < new TimeSpan(0, 0, 5))
                    {
                        drawBVD = true;
                    }
                    else
                    {
                        if (drawBVD && !firstRotate)
                        {
                            firstRotate = true;
                            RotateToActiveCharacter();
                        }
                        drawBVD = false;
                    }
                }                
            }     
            
            if (OneTeamRemaining())
            {
                if (grid.characters.list[0].team == 1)
                {
                    battleVictoryDefeat.Text = "Victory";
                }
                else
                {
                    battleVictoryDefeat.Text = "Defeat";
                }
                drawBVD = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Game1.world.state == 0)
            {
                startScreen.Draw(spriteBatch);
                title.Draw(spriteBatch);
                playGame.Draw(spriteBatch);
                options.Draw(spriteBatch);
                quit.Draw(spriteBatch);
            }
            else
            {
                background.Draw(spriteBatch);
                grid.Draw(spriteBatch);
                //grid.characters.Draw(spriteBatch);
                text.Draw(spriteBatch);
                playerName.Draw(spriteBatch);
                playerFace.Draw(spriteBatch);
                if (drawInfo)
                {
                    playerStatus.Draw(spriteBatch);
                    playerStatusIcons.Draw(spriteBatch);
                }
                if (characterName.Text != "")
                {
                    characterName.Draw(spriteBatch);
                    characterFace.Draw(spriteBatch);
                    if (drawInfo)
                    {
                        characterStatus.Draw(spriteBatch);
                        characterStatusIcons.Draw(spriteBatch);
                    }
                    characterHealthBar.Draw(spriteBatch);
                    characterHealthText.Draw(spriteBatch);
                    characterMagicBar.Draw(spriteBatch);
                    characterMagicText.Draw(spriteBatch);
                }
                foreach (Sprite sp in turnOrderTeamIcons)
                {
                    sp.Draw(spriteBatch);
                }
                foreach (Sprite s in turnOrderIcons)
                {
                    s.Draw(spriteBatch);
                }
                if (confirmationText != null)
                {
                    if (confirmationText.Text != "")
                    {
                        confirmationSprite.Draw(spriteBatch);
                        confirmationText.Draw(spriteBatch);
                    }
                }

                if (thoughtBubble.position != Vector2.Zero)
                {
                    thoughtBubble.Draw(spriteBatch);
                    faceThoughtBubble.Draw(spriteBatch);
                    drewThoughtBubble = true;
                }

                if(drawBVD)
                {
                    battleVictoryDefeat.Draw(spriteBatch);
                }

                Compass.Draw(spriteBatch);
                N.Draw(spriteBatch);
                E.Draw(spriteBatch);
                S.Draw(spriteBatch);
                W.Draw(spriteBatch);
                //leftButton.Draw(spriteBatch);
                //rightButton.Draw(spriteBatch);

                teamZeroHealth.Draw(spriteBatch);
                teamOneHealth.Draw(spriteBatch);
                zeroHealthText.Draw(spriteBatch);
                oneHealthText.Draw(spriteBatch);

                peelStatusText.Draw(spriteBatch);
                foreach(Cube c in peelStatus)
                {
                    if (c != null)
                    {
                        c.Draw(spriteBatch);
                    }
                }
                //upButton.Draw(spriteBatch);
                //downButton.Draw(spriteBatch);

                playerHealthBar.Draw(spriteBatch);
                playerHealthText.Draw(spriteBatch);
                playerMagicBar.Draw(spriteBatch);
                playerMagicText.Draw(spriteBatch);

                foreach(IButton b in buttons)
                {
                    b.Draw(spriteBatch);
                }
                
            }
        }

        public void endTurn(GameTime gameTime)
        {
            turn++;
            recalcTeamHealthBar();
            grid.peel = grid.height-1;
            recalcPeelStatus();
            Character tempCharacter = grid.characters.list[0];
            grid.characters.list.Remove(tempCharacter);
            grid.characters.list.Add(tempCharacter);

            CalcValidMoveSpaces();

            recalcStatusBars();

            setupTurnOrderIcons();

            Character player = grid.characters.list[0];

            RotateToActiveCharacter();

            //Console.WriteLine("turn: " + turn);
            //text.Text = turnOrder[turn % turnOrder.Length] + "'s turn:" + turn.ToString();
            text.Text = "Turn " + turn.ToString();
            playerName.Text = player.name;
            String t = player.health + "               " + player.mp;
            t += "\n\n\n" + player.move + "                  " + player.jump;
            t += "\n\n\n" + player.speed + "               " + player.evasion;
            t += "\n\n\n" + player.WAttack + "               " + player.MAttack;
            t += "\n\n\n" + player.WDefense + "               " + player.MDefense;
            t += "\n\n\n" + player.weaponRange + "                  " + player.magicRange;
            playerStatus.Text = t;
            playerFace.tex = player.faceLeft;
            if (player.team == 0)
            {
                playerName.position = new Vector2(260, 970);
                playerName.color = Color.Green;
                playerStatus.color = Color.Green;

            }
            else if (player.team == 1)
            {
                playerName.position = new Vector2(260, 750);
                playerName.color = Color.Purple;
                playerStatus.color = Color.Purple;
            }
            /*int mpGain = 10;
            if (player.mp + mpGain <= player.maxMP)
            {
                player.mp += mpGain;
            }
            else if (player.mp + mpGain > player.maxMP)
            {
                player.mp = player.maxMP;
            }*/
            //grid.onCharacterMoved();
            moved = false;
            attacked = false;
            rotated = false;

            if (player.team == 0)
            {
                thoughtBubble.position = player.position;
                thoughtBubble.position.Y -= 50;
                Game1.world.triggerAIBool = true;
                //Game1.world.triggerAI(this, gameTime);
            }
        }
        
        public void setCharacter(Character c)
        {
            highlightedCharacter = c;

            characterName.Text = highlightedCharacter.name;
            String t = c.health + "               " + c.mp;
            t += "\n\n\n" + c.move + "                  " + c.jump;
            t += "\n\n\n" + c.speed + "               " + c.evasion;
            t += "\n\n\n" + c.WAttack + "               " + c.MAttack;
            t += "\n\n\n" + c.WDefense + "               " + c.MDefense;
            t += "\n\n\n" + c.weaponRange + "                  " + c.magicRange;
            characterStatus.Text = t;

            if (highlightedCharacter.team == 0)
            {
                characterName.color = Color.Green;
                characterStatus.color = Color.Green;
            }
            else
            {
                characterName.color = Color.Purple;
                characterStatus.color = Color.Purple;
            }

            recalcStatusBars();
            

            characterFace.tex = highlightedCharacter.faceRight;
        }

        public void clearCharacter()
        {
            //highlightedCharacter = null;
            characterName.Text = "";
            characterStatus.Text = "";
            //characterFace.tex = null;

        }

        public void setupTurnOrderIcons()
        {
            turnOrderIcons.Clear();
            turnOrderTeamIcons.Clear();
            int index = 0;
            int width = 75;
            float offset = 1970 - width * grid.characters.list.Count + 12;
            Texture2D redTex = Game1.world.textureConverter.GenBorderedRectangle(width, 100, Color.Purple);
            Texture2D blueTex = Game1.world.textureConverter.GenBorderedRectangle(width, 100, Color.Green);
            foreach (Character c in grid.characters.list)
            {
                if (c.team == 0)
                {
                    turnOrderTeamIcons.Add(new Sprite(blueTex));
                }
                else if (c.team == 1)
                {
                    turnOrderTeamIcons.Add(new Sprite(redTex));
                }
                turnOrderTeamIcons[index].position = new Vector2(offset - 25 + index * width, 40);

                /*if (index % 2 == 0)
                {*/
                    turnOrderIcons.Add(new Sprite(c.faceLeft));
                    turnOrderIcons[index].position = new Vector2(offset + index * width - 9 , 5);

                /*}
                else
                {
                    turnOrderIcons.Add(new Sprite(c.faceRight));
                    turnOrderIcons[index].position = new Vector2(offset + (index-1) * 50, 0);
                }*/
                index++;
            }
            turnOrderIcons.Reverse();
        }        

        public void SetConfirmationText(int chance, int damage)
        {
            String text = "Chance: " + chance + " % Damage: " + damage + "HP";
            confirmationText = new TextItem(World.fontManager["Trajanus Roman 36"], text);
            confirmationText.position = new Vector2(1920 / 2, 920);
            confirmationText.color = Color.Black;
            Rectangle confirmRect = confirmationText.rectangle;
            if (confirmationText.position.X - confirmRect.Width / 2 < 1920 / 3 + 5)
            {
                //Console.WriteLine("cd third");
                confirmationText.origin = Vector2.Zero;
                confirmationText.position = new Vector2(1920 / 3 + 15, 920);
            }

            confirmationSprite = new Sprite(Game1.world.textureConverter.GenBorderedRectangle(confirmRect.Width + 20, 60, Color.White));
            confirmationSprite.origin = confirmationText.origin;
            confirmationSprite.position = confirmationText.position;
            confirmationSprite.position.Y -= 25;
            confirmationSprite.position.X -= 10;
            
        }

        public void SetConfirmationText(String text)
        {
            confirmationText = new TextItem(World.fontManager["Trajanus Roman 36"], text);
            confirmationText.position = new Vector2(1920 / 2, 920);
            confirmationText.color = Color.Black;
            Rectangle confirmRect = confirmationText.rectangle;
            if (confirmationText.position.X - confirmRect.Width / 2 < 1920 / 3 + 5)
            {
                //Console.WriteLine("t third confirmRectL: " + confirmRect.Left);
                confirmationText.origin = Vector2.Zero;
                confirmationText.position = new Vector2(1920 / 3 + 15, 920);
            }

            if (text != "")
            {
                confirmationSprite = new Sprite(Game1.world.textureConverter.GenBorderedRectangle(confirmRect.Width + 20, 60, Color.White));
                confirmationSprite.origin = confirmationText.origin;
                confirmationSprite.position = confirmationText.position;
                confirmationSprite.position.Y -= 25;
                confirmationSprite.position.X -= 10;
            }   
        }
        
        public bool OneTeamRemaining()
        {
            int team = -1;
            foreach (Character c in grid.characters.list)
            {
                if (team == -1)
                {
                    team = c.team;
                }
                else
                {
                    if (c.team != team)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // ---  MCTS ----
        public IGameState Clone()
        {
            Level clone = (Level) this.MemberwiseClone();
            String[] words = clone.name.Split(' ');
            if (words.Length > 1)
            {
                String number = words[words.Length - 1];
                int num = Int32.Parse(number);
                num++;
                clone.name = words[0] + " " + num;
            }
            else
            {
                clone.name += " 1";
            }
            clone.grid = (Grid) grid.Clone();
            return clone;
        }

        public void DoMove(IMove move)
        {
            ((Move)move).DoMove(this);
        }

        public void DoMove(IMove move, GameTime gameTime)
        {
            ((Move)move).DoMove(this, gameTime);
        }

        public void PlayRandomlyUntilTheEnd()
        {
            //Console.WriteLine("PlayingRandomlyUntilEnd");
            int startTurn = turn;
            int i = 0;
            while (!OneTeamRemaining())
            {
                //Console.WriteLine("Turn " + i);
                if (turn > (300 - startTurn))
                {
                    //Console.WriteLine("Draw");
                    break;
                }
                List<Move> moves = (List<Move>) GetMoves();
                int random = World.Random.Next(moves.Count);
                DoMove(moves[random]);
                i++;
            }
        }

        public IPlayer PlayerJustMoved => players[grid.characters.list.Last().team]; // thief special will break this on success also broken by a death

        public IEnumerable<IMove> GetMoves() // need to add character rotation
        {
            return grid.characters.list.First().GetMoves(this);
        }

        public EGameFinalStatus GetResult(IPlayer player)
        {
            int goal;
            if (player.Name == "Human")
            {
                goal = 1;                
            }
            else
            {
                goal = 0;
            }
            if (!OneTeamRemaining())
            {
                return EGameFinalStatus.GameDraw;
            }
            else
            {
                if (grid.characters.list.Count > 0)
                {
                    if (grid.characters.list[0].team == goal)
                    {
                        return EGameFinalStatus.GameWon;
                    }
                    else
                    {
                        return EGameFinalStatus.GameLost;
                    }
                }
                else
                {
                    return EGameFinalStatus.GameDraw;
                }
            }
        }

        // MCTS End Region

        public void handleActionTextColors(String selectedAction)
        {
            if (moved)
            {
                moveButton.SetTextColor(Color.Gray);
            }
            else
            {
                moveButton.SetTextColor(Color.White);
            }

            if (attacked)
            {
                attackButton.SetTextColor(Color.Gray);
                specialButton.SetTextColor(Color.Gray);
            }
            else
            {
                attackButton.SetTextColor(Color.White);
                specialButton.SetTextColor(Color.White);
            }

            if (selectedAction == "move")
            {
                moveButton.SetTextColor(Color.Yellow);
            }
            if (selectedAction == "attack")
            {
                attackButton.SetTextColor(Color.Yellow);
            }
            if (selectedAction == "special")
            {
                specialButton.SetTextColor(Color.Yellow);
            }

            if (selectedAction == "" && moved && !attacked)
            {
                undoButton.SetTextColor(Color.White);
            }
            else
            {
                undoButton.SetTextColor(Color.Gray);
            }
        }

        public void RotateToActiveCharacter()
        {
            Character player = grid.characters.list[0];

            float distanceToNorth = Game1.world.cubeDist(player.gridPos, new Vector3(0, 0, player.gridPos.Z));
            float distanceToEast = Game1.world.cubeDist(player.gridPos, new Vector3(grid.width, 0, player.gridPos.Z));
            float distanceToSouth = Game1.world.cubeDist(player.gridPos, new Vector3(grid.width, grid.depth, player.gridPos.Z));
            float distanceToWest = Game1.world.cubeDist(player.gridPos, new Vector3(0, grid.depth, player.gridPos.Z));

            List<float> distancesToCorners = new List<float>();
            //East right once
            //North right twice
            //West right 3 times


            distancesToCorners.Add(distanceToEast);
            distancesToCorners.Add(distanceToNorth);
            distancesToCorners.Add(distanceToWest);
            distancesToCorners.Add(distanceToSouth);

            float min = distancesToCorners.Min();
            //Console.WriteLine("e:" + distancesToCorners[0] + " n:" + distancesToCorners[1] + " w:" + distancesToCorners[2] + " s:" + distancesToCorners[3]);
            if (min != distanceToSouth)
            {
                if (distanceToEast == min)
                {
                    //Console.WriteLine("east");
                    grid.rotate(true, this);
                }
                else if (distanceToWest == min)
                {
                    //Console.WriteLine("west");
                    grid.rotate(false, this);
                }
                else
                {
                    //Console.WriteLine("north");
                    grid.rotate(true, this);
                    grid.rotate(true, this);
                }
            }
        }

        public void RotateCompass()
        {
            switch (grid.activeView)
            {
                case 0:
                    N.Text = "N";
                    E.Text = "E";
                    S.Text = "S";
                    W.Text = "W";
                    N.position = Compass.position - new Vector2(0, 64);
                    E.position = Compass.position + new Vector2(85, 0);
                    S.position = Compass.position + new Vector2(0, 64);
                    W.position = Compass.position - new Vector2(100, 0);
                    break;
                case 1:
                    N.Text = "W";
                    E.Text = "N";
                    S.Text = "E";
                    W.Text = "S";
                    N.position = Compass.position - new Vector2(0, 64);
                    E.position = Compass.position + new Vector2(85, 0);
                    S.position = Compass.position + new Vector2(0, 64);
                    W.position = Compass.position - new Vector2(75, 0);
                    break;
                case 2:
                    N.Text = "S";
                    E.Text = "W";
                    S.Text = "N";
                    W.Text = "E";
                    N.position = Compass.position - new Vector2(-10, 64);
                    E.position = Compass.position + new Vector2(85, 0);
                    S.position = Compass.position + new Vector2(-10, 64);
                    W.position = Compass.position - new Vector2(75, 0);
                    break;
                case 3:
                    N.Text = "E";
                    E.Text = "S";
                    S.Text = "W";
                    W.Text = "N";
                    N.position = Compass.position - new Vector2(-10, 64);
                    E.position = Compass.position + new Vector2(85, 0);
                    S.position = Compass.position + new Vector2(-10, 64);
                    W.position = Compass.position - new Vector2(85, 0);
                    break;
            }
        }

        public void CalcValidMoveSpaces()
        {
            Character activeCharacter = grid.characters.list[0];
            Graph graph = new Graph(activeCharacter, grid);
            validMoveSpaces = new List<Vector3>();
            validMovePaths = new List<List<Vector3>>();
            foreach (Cube c in grid.cubes)
            {
                if (c.gridPos != activeCharacter.gridPos && activeCharacter.inMoveRange(c.gridPos, this) && grid.isVacant(c.gridPos))
                {
                    List<Vector3> tempList = graph.AStar(c.gridPos);
                    if (tempList != null) 
                    {
                        validMoveSpaces.Add(c.gridPos);
                        validMovePaths.Add(tempList);
                    }
                }
            }
        }

    }
}
