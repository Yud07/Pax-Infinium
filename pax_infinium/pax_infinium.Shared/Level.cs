using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using MCTS.Interfaces;
using MCTS.Enum;
using pax_infinium.Enum;

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
        public TextItem Arrows;

        public Sprite teamZeroHealth;
        public Sprite teamOneHealth;

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
            c1 = 1;// random.Next(0,5);
            c2 = 1;// random.Next(0,5);
            c3 = 1;// random.Next(0,5);
            c4 = 0;// random.Next(0,5);
            grid = new Grid(graphics, seed, 10, 10, 5, c1, c2, c3, c4, random);
            background = new Background(Game1.world.textureConverter.GenRectangle(1920, 1080, Color.SkyBlue), graphics.GraphicsDevice.Viewport);
            players.Add(new Player("AI"));
            players.Add(new Player("Human"));
            grid.characters = new Characters();
            grid.characters.AddCharacter("Blue Soldier", EJob.Soldier, 0, grid.origin, graphics);
            grid.characters.AddCharacter("Red Soldier", EJob.Soldier, 1, grid.origin, graphics);
            grid.characters.AddCharacter("Blue Hunter", EJob.Hunter, 0, grid.origin, graphics);
            grid.characters.AddCharacter("Red Hunter", EJob.Hunter, 1, grid.origin, graphics);
            grid.characters.AddCharacter("Blue Mage", EJob.Mage, 0, grid.origin, graphics);
            grid.characters.AddCharacter("Red Mage", EJob.Mage, 1, grid.origin, graphics);
            grid.characters.AddCharacter("Blue Healer", EJob.Healer, 0, grid.origin, graphics);
            grid.characters.AddCharacter("Red Healer", EJob.Healer, 1, grid.origin, graphics);
            grid.characters.AddCharacter("Blue Thief", EJob.Thief, 0, grid.origin, graphics);
            grid.characters.AddCharacter("Red Thief", EJob.Thief, 1, grid.origin, graphics);
            grid.characters.list.Sort(Character.CompareBySpeed);
            grid.characters.list.Reverse();
            grid.placeCharacters();
            turn = 0;
            moved = false;
            attacked = false;
            rotated = false;
            text = new TextItem(World.fontManager["InfoFont"], "Turn:" + turn.ToString());
            text.position = new Vector2(100, 30);
            text.color = Color.Black;
            text.scale = 3;


            playerName = new TextItem(World.fontManager["InfoFont"], grid.characters.list[0].name);
            playerName.position = new Vector2(430, 1050);            
            playerName.scale = 3;

            String t = grid.characters.list[0].health + "              " + grid.characters.list[0].mp;
            t += "\n\n\n" + grid.characters.list[0].move + "                 " + grid.characters.list[0].jump;
            t += "\n\n\n" + grid.characters.list[0].speed + "              " + grid.characters.list[0].evasion;
            t += "\n\n\n" + grid.characters.list[0].WAttack + "              " + grid.characters.list[0].MAttack;
            t += "\n\n\n" + grid.characters.list[0].WDefense + "              " + grid.characters.list[0].MDefense;
            t += "\n\n\n" + grid.characters.list[0].weaponRange + "                 " + grid.characters.list[0].magicRange;
            playerStatus = new TextItem(World.fontManager["InfoFont"], t);
            playerStatus.position = new Vector2(200, 500);            
            playerStatus.scale = 1.5f;

            if (grid.characters.list[0].team == 0)
            {
                playerName.color = Color.Blue;
                playerStatus.color = Color.Blue;
            }
            else
            {
                playerName.color = Color.Red;
                playerStatus.color = Color.Red;
            }

            playerFace = new Sprite(grid.characters.list[0].faceLeft);
            //playerFace = new Sprite(Game1.world.textureConverter.GenRectangle(90, 160, Color.Blue));
            playerFace.position = new Vector2(playerFace.tex.Width * 2, playerFace.tex.Height * 5);
            playerFace.scale = 4;

            characterName = new TextItem(World.fontManager["InfoFont"], grid.characters.list[0].name);
            characterName.Text = "";
            characterName.position = new Vector2(1500, 1050);
            characterName.scale = 3;

            characterStatus = new TextItem(World.fontManager["InfoFont"], t);
            characterStatus.Text = "";
            characterStatus.position = new Vector2(1810, 500);
            characterStatus.scale = 1.5f;

            characterFace = new Sprite(grid.characters.list[0].faceRight);
            //characterFace = new Sprite(Game1.world.textureConverter.GenRectangle(90, 160, Color.Blue));
            characterFace.position = new Vector2(characterFace.tex.Width * 18, characterFace.tex.Height * 5);
            characterFace.scale = 4;

            playerStatusIcons = new Sprite(World.textureManager["Status Icons"]);
            playerStatusIcons.position = new Vector2(playerStatusIcons.tex.Width / 10, playerStatusIcons.tex.Height / 5.1f);
            playerStatusIcons.scale = .2f;

            characterStatusIcons = new Sprite(World.textureManager["Status Icons"]);
            characterStatusIcons.position = new Vector2(characterStatusIcons.tex.Width * 1.45f, characterStatusIcons.tex.Height / 5.1f);
            characterStatusIcons.scale = .2f;

            turnOrderIcons = new List<Sprite>();
            turnOrderTeamIcons = new List<Sprite>();
            setupTurnOrderIcons();

            confirmationText = new TextItem(World.fontManager["InfoFont"], "Chance: 95% Damage: 43HP Confirm: Y/N");
            confirmationText.Text = "";
            confirmationText.position = new Vector2(950, 1050);
            confirmationText.scale = 2;
            confirmationText.color = Color.Black;

            actionsFrame = new Sprite(Game1.world.textureConverter.GenRectangle(300, 200, Color.Black));
            actionsFrame.position = new Vector2(450, 910);
            actionsFrame.alpha = .5f;

            moveAction = new TextItem(World.fontManager["Trajanus Roman 36"], "Move");
            moveAction.position = new Vector2(450, 845);
            moveAction.color = Color.White;

            attackAction = new TextItem(World.fontManager["Trajanus Roman 36"], "Attack");
            attackAction.position = new Vector2(450, 890);
            attackAction.color = Color.White;

            specialAction = new TextItem(World.fontManager["Trajanus Roman 36"], "Special");
            specialAction.position = new Vector2(450, 935);
            specialAction.color = Color.White;

            endTurnAction = new TextItem(World.fontManager["Trajanus Roman 36"], "End Turn");
            endTurnAction.position = new Vector2(450, 980);
            endTurnAction.color = Color.White;

            thoughtBubble = new Sprite(World.textureManager["Thought Bubble"]);
            thoughtBubble.scale = 2;
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
            Compass.position = new Vector2(1800, 175);
            N = new TextItem(World.fontManager["Trajanus Roman 36"], "N");
            N.position = Compass.position - new Vector2(0, 64);
            E = new TextItem(World.fontManager["Trajanus Roman 36"], "E");
            E.position = Compass.position + new Vector2(85, 0);
            S = new TextItem(World.fontManager["Trajanus Roman 36"], "S");
            S.position = Compass.position + new Vector2(0, 64);
            W = new TextItem(World.fontManager["Trajanus Roman 36"], "W");
            W.position = Compass.position - new Vector2(100, 0);
            N.color = E.color = S.color = W.color = Color.Black;
            Arrows = new TextItem(World.fontManager["Trajanus Roman 36"], "<-    ->");
            Arrows.color = Color.Black;
            Arrows.position = Compass.position + new Vector2(0, 64);

            recalcTeamHealthBar();
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
            teamZeroHealth = new Sprite(Game1.world.textureConverter.GenRectangle(zeroSize, 25, Color.Blue));
            teamZeroHealth.position = new Vector2(1920 / 2.5f - zeroSize / 2, 0);
            teamOneHealth = new Sprite(Game1.world.textureConverter.GenRectangle(oneSize, 25, Color.Red));
            teamOneHealth.position = new Vector2(1920 / 2.5f + oneSize / 2, 0);
        }

        /*
            characterName = new TextItem(World.fontManager["InfoFont"], highlightedCharacter.name);
            characterName.position = new Vector2(1570, 1000);
            characterName.color = Color.Blue;
            characterName.scale = 3;

            characterStatus = new TextItem(World.fontManager["InfoFont"], highlightedCharacter.health + " Health " + highlightedCharacter.mp + " MP");
            characterStatus.position = new Vector2(1570, 1050);
            characterStatus.color = Color.Blue;
            characterStatus.scale = 2;

            characterFace = new Sprite(highlightedCharacter.faceRight);
            //characterFace = new Sprite(Game1.world.textureConverter.GenRectangle(90, 160, Color.Blue));
            characterFace.position = new Vector2(characterFace.tex.Width * 18, characterFace.tex.Height * 5);
            characterFace.scale = 4;
            */

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
                playerStatus.Draw(spriteBatch);
                playerStatusIcons.Draw(spriteBatch);
                if (characterName.Text != "")
                {
                    characterName.Draw(spriteBatch);
                    characterFace.Draw(spriteBatch);
                    characterStatus.Draw(spriteBatch);
                    characterStatusIcons.Draw(spriteBatch);
                }
                foreach (Sprite sp in turnOrderTeamIcons)
                {
                    sp.Draw(spriteBatch);
                }
                foreach (Sprite s in turnOrderIcons)
                {
                    s.Draw(spriteBatch);
                }
                if (confirmationText.Text != "")
                {
                    confirmationText.Draw(spriteBatch);
                }

                if (grid.characters.list[0].team == 1)
                {
                    actionsFrame.Draw(spriteBatch);
                    moveAction.Draw(spriteBatch);
                    attackAction.Draw(spriteBatch);
                    specialAction.Draw(spriteBatch);
                    endTurnAction.Draw(spriteBatch);
                }

                if (thoughtBubble.position != Vector2.Zero)
                {
                    thoughtBubble.Draw(spriteBatch);
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
                Arrows.Draw(spriteBatch);

                teamZeroHealth.Draw(spriteBatch);
                teamOneHealth.Draw(spriteBatch);
            }
        }

        public void endTurn(GameTime gameTime)
        {
            turn++;
            recalcTeamHealthBar();
            Character tempCharacter = grid.characters.list[0];
            grid.characters.list.Remove(tempCharacter);
            grid.characters.list.Add(tempCharacter);

            setupTurnOrderIcons();

            Character player = grid.characters.list[0];

            RotateToActiveCharacter();

            //Console.WriteLine("turn: " + turn);
            //text.Text = turnOrder[turn % turnOrder.Length] + "'s turn:" + turn.ToString();
            text.Text = "Turn:" + turn.ToString();
            playerName.Text = player.name;
            String t = player.health + "              " + player.mp;
            t += "\n\n\n" + player.move + "                 " + player.jump;
            t += "\n\n\n" + player.speed + "              " + player.evasion;
            t += "\n\n\n" + player.WAttack + "              " + player.MAttack;
            t += "\n\n\n" + player.WDefense + "              " + player.MDefense;
            t += "\n\n\n" + player.weaponRange + "                 " + player.magicRange;
            playerStatus.Text = t;
            playerFace.tex = player.faceLeft;
            if (player.team == 0)
            {
                playerName.color = Color.Blue;
                playerStatus.color = Color.Blue;

            }
            else if (player.team == 1)
            {
                playerName.color = Color.Red;
                playerStatus.color = Color.Red;
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
            String t = c.health + "              " + c.mp;
            t += "\n\n\n" + c.move + "                 " + c.jump;
            t += "\n\n\n" + c.speed + "              " + c.evasion;
            t += "\n\n\n" + c.WAttack + "              " + c.MAttack;
            t += "\n\n\n" + c.WDefense + "              " + c.MDefense;
            t += "\n\n\n" + c.weaponRange + "                 " + c.magicRange;
            characterStatus.Text = t;

            if (highlightedCharacter.team == 0)
            {
                characterName.color = Color.Blue;
                characterStatus.color = Color.Blue;
            }
            else
            {
                characterName.color = Color.Red;
                characterStatus.color = Color.Red;
            }
            

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
            float offset = 1970 - 50 * grid.characters.list.Count;
            Texture2D redTex = Game1.world.textureConverter.GenRectangle(50, grid.characters.list[0].faceLeft.Height, Color.Red);
            Texture2D blueTex = Game1.world.textureConverter.GenRectangle(50, grid.characters.list[0].faceLeft.Height, Color.Blue);
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
                turnOrderTeamIcons[index].position = new Vector2(offset - 25 + index * 50, 0);

                /*if (index % 2 == 0)
                {*/
                    turnOrderIcons.Add(new Sprite(c.faceLeft));
                    turnOrderIcons[index].position = new Vector2(offset + index * 50, 0);

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
            confirmationText.Text = "Chance: " + chance + " % Damage: " + damage + "HP Confirm: Y / N";
        }

        public void SetConfirmationText(String text)
        {
            confirmationText.Text = text;
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
                moveAction.color = Color.Gray;
            }
            else
            {
                moveAction.color = Color.White;
            }

            if (attacked)
            {
                attackAction.color = Color.Gray;
                specialAction.color = Color.Gray;
            }
            else
            {
                attackAction.color = Color.White;
                specialAction.color = Color.White;
            }

            if (selectedAction == "move")
            {
                moveAction.color = Color.Yellow;
            }
            if (selectedAction == "attack")
            {
                attackAction.color = Color.Yellow;
            }
            if (selectedAction == "special")
            {
                specialAction.color = Color.Yellow;
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

    }
}
