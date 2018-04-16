using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using MCTS.V2.Interfaces;
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
        //public IButton undoButton;
        public IButton confirmButton;
        public IButton cancelButton;

        public List<Vector3> validMoveSpaces;
        public List<List<Vector3>> validMovePaths;

        public List<Descriptor> descriptors;

        public List<Character> toBeKilled;
        public Character RotateTo;

        public Sprite playerStatusBacker;
        public Sprite characterStatusBacker;
        public Sprite teamHealthBacker;
        public int zeroStartHealth;
        public int oneStartHealth;

        public bool printedAIStuff;
        public List<int> IterationsPerTurn;
        public List<int> VistsPerChoice;
        public List<int> MovesAnalyzedPerTurn;
        public List<int> TurnsPerSim;
        public int damageDealtByAI;
        public List<int> WinsPerChoice;
        public int SimsEndedEarly;

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
            EPersonality[] personalities = new EPersonality[5];
            for(int i = 0; i < 5; i++)
            {
                int roll = World.Random.Next(0, 3);//7); //0, 5);
                if (roll == 0)
                {
                    personalities[i] = EPersonality.Default;
                }
                else if (roll == 1)
                {
                    personalities[i] = EPersonality.Aggressive;
                }
                else if (roll == 2)
                {
                    personalities[i] = EPersonality.Defensive;
                }
                else if (roll == 3)
                {
                    personalities[i] = EPersonality.SelfishAggressive;
                }
                else if (roll == 4)
                {
                    personalities[i] = EPersonality.SelfishDefensive;
                }
                else if (roll == 5)
                {
                    personalities[i] = EPersonality.Selfish;
                }
                else if (roll == 6)
                {
                    personalities[i] = EPersonality.Survivalist;
                }
            }
            grid.characters.AddCharacter(EJob.Soldier, 0, personalities[0], grid.origin, graphics);
            grid.characters.AddCharacter(EJob.Soldier, 1, EPersonality.Default, grid.origin, graphics);
            grid.characters.AddCharacter(EJob.Hunter, 0, personalities[1], grid.origin, graphics);
            grid.characters.AddCharacter(EJob.Hunter, 1, EPersonality.Default, grid.origin, graphics);
            grid.characters.AddCharacter(EJob.Mage, 0, personalities[2], grid.origin, graphics);
            grid.characters.AddCharacter(EJob.Mage, 1, EPersonality.Default, grid.origin, graphics);
            grid.characters.AddCharacter(EJob.Healer, 0, personalities[3], grid.origin, graphics);
            grid.characters.AddCharacter(EJob.Healer, 1, EPersonality.Default, grid.origin, graphics);
            grid.characters.AddCharacter(EJob.Thief, 0, personalities[4], grid.origin, graphics);
            grid.characters.AddCharacter(EJob.Thief, 1, EPersonality.Default, grid.origin, graphics);
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
            playerName.position = new Vector2(1920 / 3 - 300, 750);
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
            characterName.position = new Vector2(1920 * 2 / 3, 970);

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
            zeroStartHealth = -1;
            oneStartHealth = -1;
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

            Vector2 actionButtonsPos = new Vector2(1920 / 3 - 300, 780);
            moveButton = new MoveButton(actionButtonsPos);
            attackButton = new AttackButton(actionButtonsPos + new Vector2(0, 55));
            specialButton = new SpecialButton(actionButtonsPos + new Vector2(0, 55 * 2));
            endTurnButton = new EndTurnButton(actionButtonsPos + new Vector2(0, 55 * 3));
            //undoButton = new UndoButton(actionButtonsPos + new Vector2(305, 0));
            confirmButton = new ConfirmButton(new Vector2(1920 / 2 - 125, 1080 - 110));
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
            //buttons.Add(undoButton);
            buttons.Add(confirmButton);
            buttons.Add(cancelButton);

            descriptors = new List<Descriptor>();
            descriptors.Add(attackButton.GetDescriptor());
            descriptors.Add(moveButton.GetDescriptor());
            descriptors.Add(endTurnButton.GetDescriptor());
            descriptors.Add(specialButton.GetDescriptor());
            //descriptors.Add(undoButton.GetDescriptor());
            descriptors.Add(upButton.GetDescriptor());
            descriptors.Add(downButton.GetDescriptor());
            descriptors.Add(leftButton.GetDescriptor());
            descriptors.Add(rightButton.GetDescriptor());
            descriptors.Add(confirmButton.GetDescriptor());
            descriptors.Add(cancelButton.GetDescriptor());

            Polygon teamHealthBarPoly = new Polygon();
            float thbpX = 1920 / 2.75f - 400;
            float thbpXb = thbpX + 800;
            Vector2 thbpTopLeft = new Vector2(thbpX, 0);
            Vector2 thbpTopRight = new Vector2(thbpXb, 0);
            Vector2 thbpBotLeft = new Vector2(thbpX, 50);
            Vector2 thbpBotRight = new Vector2(thbpXb, 50);
            teamHealthBarPoly.Lines.Add(new PolyLine(thbpTopLeft, thbpTopRight));
            teamHealthBarPoly.Lines.Add(new PolyLine(thbpTopLeft, thbpBotLeft));
            teamHealthBarPoly.Lines.Add(new PolyLine(thbpTopRight, thbpBotRight));
            teamHealthBarPoly.Lines.Add(new PolyLine(thbpBotLeft, thbpBotRight));
            Descriptor teamHealthBarsDescriptor = new Descriptor(teamHealthBarPoly, "The Total Health Points of each team. Purple is the player and Green is the computer.");
            descriptors.Add(teamHealthBarsDescriptor);

            float offset = 1970 - 25 - 50 - 75 * grid.characters.list.Count + 12 + 5;
            Polygon turnOrderIconsPoly = new Polygon();
            Vector2 toipTopLeft = new Vector2(offset, 0);
            Vector2 toipTopRight = new Vector2(1920, 0);
            Vector2 toipBotLeft = new Vector2(offset, 95);
            Vector2 toipBotRight = new Vector2(1920, 95);
            turnOrderIconsPoly.Lines.Add(new PolyLine(toipTopLeft, toipTopRight));
            turnOrderIconsPoly.Lines.Add(new PolyLine(toipTopLeft, toipBotLeft));
            turnOrderIconsPoly.Lines.Add(new PolyLine(toipTopRight, toipBotRight));
            turnOrderIconsPoly.Lines.Add(new PolyLine(toipBotLeft, toipBotRight));
            Descriptor turnOrderIconsDescriptor = new Descriptor(turnOrderIconsPoly, "The order characters take their turn in from left to right. Updates each turn.");
            descriptors.Add(turnOrderIconsDescriptor);

            Polygon peelPoly = new Polygon();
            Vector2 ppTopLeft = new Vector2(5, 100);
            Vector2 ppTopRight = new Vector2(135, 100);
            Vector2 ppBotLeft = new Vector2(5, 205);
            Vector2 ppBotRight = new Vector2(135, 205);
            peelPoly.Lines.Add(new PolyLine(ppTopLeft, ppTopRight));
            peelPoly.Lines.Add(new PolyLine(ppTopLeft, ppBotLeft));
            peelPoly.Lines.Add(new PolyLine(ppTopRight, ppBotRight));
            peelPoly.Lines.Add(new PolyLine(ppBotLeft, ppBotRight));
            Descriptor peelDescriptor = new Descriptor(peelPoly, "The representation of the peel (cube visibility) state. Lower it to 'see' through layers of cubes.");
            descriptors.Add(peelDescriptor);

            Polygon compassPoly = new Polygon();
            Vector2 cTopLeft = new Vector2(Compass.position.X - 105 + 40, Compass.position.Y - Compass.tex.Height * 1.5f);
            Vector2 cTopRight = new Vector2(Compass.position.X + 68, Compass.position.Y - Compass.tex.Height * 1.5f);
            Vector2 cBotLeft = new Vector2(Compass.position.X - 105 + 40, Compass.position.Y + 80);
            Vector2 cBotRight = new Vector2(Compass.position.X + 68, Compass.position.Y + 80);
            compassPoly.Lines.Add(new PolyLine(cTopLeft, cTopRight));
            compassPoly.Lines.Add(new PolyLine(cTopLeft, cBotLeft));
            compassPoly.Lines.Add(new PolyLine(cTopRight, cBotRight));
            compassPoly.Lines.Add(new PolyLine(cBotLeft, cBotRight));
            Descriptor compassDescriptor = new Descriptor(compassPoly, "The representation of the board's rotation using cardinal directions. At the end of each turn the board is " +
                "rotated automatically so that the character whose turn it is is closest to the bottom of the screen.");
            descriptors.Add(compassDescriptor);

            Polygon playerFacePoly = new Polygon();
            Vector2 pfTopLeft = new Vector2(0, 677);
            Vector2 pfTopRight = new Vector2(258, 677);
            Vector2 pfBotLeft = new Vector2(0, 992);
            Vector2 pfBotRight = new Vector2(258, 992);
            playerFacePoly.Lines.Add(new PolyLine(pfTopLeft, pfTopRight));
            playerFacePoly.Lines.Add(new PolyLine(pfTopLeft, pfBotLeft));
            playerFacePoly.Lines.Add(new PolyLine(pfTopRight, pfBotRight));
            playerFacePoly.Lines.Add(new PolyLine(pfBotLeft, pfBotRight));
            Descriptor playerFaceDescriptor = new Descriptor(playerFacePoly, "The face of the character whose turn it is.");
            descriptors.Add(playerFaceDescriptor);

            Polygon playerHealthBarPoly = new Polygon();
            Vector2 phbTopLeft = new Vector2(0, 1040);
            Vector2 phbTopRight = new Vector2(1920 / 3, 1040);
            Vector2 phbBotLeft = new Vector2(0, 1080);
            Vector2 phbBotRight = new Vector2(1920 / 3, 1080);
            playerHealthBarPoly.Lines.Add(new PolyLine(phbTopLeft, phbTopRight));
            playerHealthBarPoly.Lines.Add(new PolyLine(phbTopLeft, phbBotLeft));
            playerHealthBarPoly.Lines.Add(new PolyLine(phbTopRight, phbBotRight));
            playerHealthBarPoly.Lines.Add(new PolyLine(phbBotLeft, phbBotRight));
            Descriptor playerHealthBarDescriptor = new Descriptor(playerHealthBarPoly, "The amount of health points of the character whose turn it is. Health points are removed when a character is attacked or can be added when they are healed. Characters die when out of health.");
            descriptors.Add(playerHealthBarDescriptor);

            Polygon playerMagicBarPoly = new Polygon();
            Vector2 pmbTopLeft = new Vector2(0, 1000);
            Vector2 pmbTopRight = new Vector2(1920 / 3, 1000);
            Vector2 pmbBotLeft = new Vector2(0, 1040);
            Vector2 pmbBotRight = new Vector2(1920 / 3, 1040);
            playerMagicBarPoly.Lines.Add(new PolyLine(pmbTopLeft, pmbTopRight));
            playerMagicBarPoly.Lines.Add(new PolyLine(pmbTopLeft, pmbBotLeft));
            playerMagicBarPoly.Lines.Add(new PolyLine(pmbTopRight, pmbBotRight));
            playerMagicBarPoly.Lines.Add(new PolyLine(pmbBotLeft, pmbBotRight));
            Descriptor playerMagicBarDescriptor = new Descriptor(playerMagicBarPoly, "The amount of magic points of the character whose turn it is. Magic points are removed when a character uses their special. These points are not regenerated during battle.");
            descriptors.Add(playerMagicBarDescriptor);

            Polygon characterFacePoly = new Polygon();
            Vector2 cfTopLeft = new Vector2(1910 - 258, 677);
            Vector2 cfTopRight = new Vector2(1910, 677);
            Vector2 cfBotLeft = new Vector2(1910 - 258, 992);
            Vector2 cfBotRight = new Vector2(1910, 992);
            characterFacePoly.Lines.Add(new PolyLine(cfTopLeft, cfTopRight));
            characterFacePoly.Lines.Add(new PolyLine(cfTopLeft, cfBotLeft));
            characterFacePoly.Lines.Add(new PolyLine(cfTopRight, cfBotRight));
            characterFacePoly.Lines.Add(new PolyLine(cfBotLeft, cfBotRight));
            Descriptor characterFaceDescriptor = new Descriptor(characterFacePoly, "The face of the character who is selected or hovered over.", characterFace);
            descriptors.Add(characterFaceDescriptor);

            Polygon characterHealthBarPoly = new Polygon();
            Vector2 chbTopLeft = new Vector2(1920 * 2 / 3, 1040);
            Vector2 chbTopRight = new Vector2(1920, 1040);
            Vector2 chbBotLeft = new Vector2(1920 * 2 / 3, 1080);
            Vector2 chbBotRight = new Vector2(1920, 1080);
            characterHealthBarPoly.Lines.Add(new PolyLine(chbTopLeft, chbTopRight));
            characterHealthBarPoly.Lines.Add(new PolyLine(chbTopLeft, chbBotLeft));
            characterHealthBarPoly.Lines.Add(new PolyLine(chbTopRight, chbBotRight));
            characterHealthBarPoly.Lines.Add(new PolyLine(chbBotLeft, chbBotRight));
            Descriptor characterHealthBarDescriptor = new Descriptor(characterHealthBarPoly, "The amount of health points of the character whose turn it is. Health points are removed when a character is attacked or can be added when they are healed. Characters die when out of health.", characterHealthBar);
            descriptors.Add(characterHealthBarDescriptor);

            Polygon characterMagicBarPoly = new Polygon();
            Vector2 cmbTopLeft = new Vector2(1920 * 2 / 3, 1000);
            Vector2 cmbTopRight = new Vector2(1920, 1000);
            Vector2 cmbBotLeft = new Vector2(1920 * 2 / 3, 1040);
            Vector2 cmbBotRight = new Vector2(1920, 1040);
            characterMagicBarPoly.Lines.Add(new PolyLine(cmbTopLeft, cmbTopRight));
            characterMagicBarPoly.Lines.Add(new PolyLine(cmbTopLeft, cmbBotLeft));
            characterMagicBarPoly.Lines.Add(new PolyLine(cmbTopRight, cmbBotRight));
            characterMagicBarPoly.Lines.Add(new PolyLine(cmbBotLeft, cmbBotRight));
            Descriptor characterMagicBarDescriptor = new Descriptor(characterMagicBarPoly, "The amount of magic points of the character who is hovered over or selected. Magic points are removed when a character uses their special. These points are not regenerated during battle.", characterMagicBar);
            descriptors.Add(characterMagicBarDescriptor);

            toBeKilled = new List<Character>();
            RotateTo = null;

            playerStatusBacker = new Sprite(Game1.world.textureConverter.GenBorderedRectangle(1920 / 3, 80, new Color(0, 0, 0, .5f)));
            playerStatusBacker.origin = Vector2.Zero;
            playerStatusBacker.position = new Vector2(0, 1000);

            characterStatusBacker = new Sprite(Game1.world.textureConverter.GenBorderedRectangle(1920 / 3, 80, new Color(0, 0, 0, .5f)));
            characterStatusBacker.origin = Vector2.Zero;
            characterStatusBacker.position = new Vector2(1920 * 2 / 3, 1000);

            teamHealthBacker = new Sprite(Game1.world.textureConverter.GenBorderedRectangle(800, 50, new Color(0, 0, 0, .5f)));
            teamHealthBacker.position = new Vector2(1920 / 2.75f, +25);

            printedAIStuff = false;
            IterationsPerTurn = new List<int>();
            VistsPerChoice = new List<int>();
            MovesAnalyzedPerTurn = new List<int>();
            TurnsPerSim = new List<int>();
            damageDealtByAI = 0;
            WinsPerChoice = new List<int>();
            SimsEndedEarly = 0;
        }

        public void recalcTeamHealthBar()
        {
            if (!OneTeamRemaining())
            {
                int zeroHealth = 0;
                int oneHealth = 0;
                foreach (Character character in grid.characters.list)
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

                if (zeroStartHealth == -1)
                {
                    zeroStartHealth = zeroHealth;
                    oneStartHealth = oneHealth;
                }

                int zeroSize = 400 * zeroHealth / zeroStartHealth;
                int oneSize = 400 * oneHealth / oneStartHealth;
                if (zeroSize > 0)
                {
                    teamZeroHealth = new Sprite(Game1.world.textureConverter.GenBorderedRectangle(zeroSize, 50, Color.Green));
                    teamZeroHealth.origin = Vector2.Zero;
                    teamZeroHealth.position = new Vector2(1920 / 2.75f - zeroSize, 0);
                }
                else
                {
                    teamZeroHealth = null;
                }
                if (oneSize > 0)
                {
                    teamOneHealth = new Sprite(Game1.world.textureConverter.GenBorderedRectangle(oneSize, 50, Color.Purple));
                    teamOneHealth.origin = Vector2.Zero;
                    teamOneHealth.position = new Vector2(1920 / 2.75f, 0);
                }
                else
                {
                    teamOneHealth = null;
                }


                zeroHealthText.Text = zeroHealth.ToString() + " HP";
                zeroHealthText.position = new Vector2(1920 / 2.75f - 200, 27);
                oneHealthText.Text = oneHealth.ToString() + " HP";
                oneHealthText.position = new Vector2(1920 / 2.75f + 200, 27);
            }
        }

        public void initPeelStatus(Cube middleCube)
        {
            peelStatus = new List<Cube>();
            for (int i = 0; i < grid.height; i++)
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
                    c = new Cube(new Vector2(25, 175 - i * .25f * (c.southwestTex.Height + c.topTex.Height) / 2), c.southwestTex, c.southeastTex, c.topTex);
                }
                peelStatus.Add(c);
            }
        }

        public void recalcPeelStatus()
        {
            peelStatusText.Text = grid.peel + "/" + (grid.height - 1);
            int count = 0;
            foreach (Cube c in peelStatus)
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

            if ((int)(fullBarSize * playerHealthRatio) > 0)
            {
                playerHealthBar = new pax_infinium.Sprite(Game1.world.textureConverter.GenBorderedRectangle((int)(fullBarSize * playerHealthRatio), height, Color.Red));
                playerHealthBar.position = new Vector2((fullBarSize * playerHealthRatio) / 2, 1080 - height / 2);
            }
            else
            {
                playerHealthBar = null;
            }
            playerHealthText.Text = grid.characters.list[0].health + " HP";
            playerHealthText.position = new Vector2(fullBarSize / 2, 1080 - height / 2);
            playerHealthText.position.Y += 2;

            if ((int)(fullBarSize * playerMPRatio) > 0)
            {
                playerMagicBar = new pax_infinium.Sprite(Game1.world.textureConverter.GenBorderedRectangle((int)(fullBarSize * playerMPRatio), height, Color.Blue));
                playerMagicBar.position = new Vector2((fullBarSize * playerMPRatio) / 2, 1080 - height - yOffset);
            }
            else
            {
                playerMagicBar = null;
            }
            playerMagicText.Text = grid.characters.list[0].mp + " MP";
            playerMagicText.position = new Vector2(fullBarSize / 2, 1080 - height - yOffset);
            playerMagicText.position.Y += 2;

            if (characterName.Text != "")
            {
                if (highlightedCharacter.health > 0)
                {
                    float characterHealthRatio = (float)highlightedCharacter.health / (float)highlightedCharacter.startingHealth;
                    float characterMPRatio = (float)highlightedCharacter.mp / (float)highlightedCharacter.startingMP;

                    if ((int)(fullBarSize * characterHealthRatio) > 0)
                    {
                        characterHealthBar = new pax_infinium.Sprite(Game1.world.textureConverter.GenBorderedRectangle((int)(fullBarSize * characterHealthRatio), height, Color.Red));
                        characterHealthBar.position = new Vector2(1920 - (fullBarSize * characterHealthRatio) / 2, 1080 - height / 2);
                    }
                    else
                    {
                        characterHealthBar = null;
                    }
                    characterHealthText.Text = highlightedCharacter.health + " HP";
                    characterHealthText.position = new Vector2(1920 - fullBarSize / 2, 1080 - height / 2);
                    characterHealthText.position.Y += 2;

                    if ((int)(fullBarSize * characterMPRatio) > 0)
                    {
                        characterMagicBar = new pax_infinium.Sprite(Game1.world.textureConverter.GenBorderedRectangle((int)(fullBarSize * characterMPRatio), height, Color.Blue));
                        characterMagicBar.position = new Vector2(1920 - (fullBarSize * characterMPRatio) / 2, 1080 - height - yOffset);
                    }
                    else
                    {
                        characterMagicBar = null;
                    }
                    characterMagicText.Text = highlightedCharacter.mp + " MP";
                    characterMagicText.position = new Vector2(1920 - fullBarSize / 2, 1080 - height - yOffset);
                    characterMagicText.position.Y += 2;
                }
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
            foreach (Vector3 v in validMoveSpaces)
            {
                Point newCoords = Game1.world.rotate(new Point((int)v.X, (int)v.Y), grid.deg2Rad(degrees), new Point((int)grid.width / 2 - 1, (int)grid.depth / 2 - 1));
                tempVectList.Add(new Vector3(newCoords.X, newCoords.Y, v.Z));
                List<Vector3> tempVList = new List<Vector3>();
                foreach (Vector3 vect in validMovePaths.First()) {
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

            if (toBeKilled.Count > 0) {
                List<Character> tempCharacters = new List<Character>();
                foreach (Character c in toBeKilled)
                {
                    if (c.hitType == 0)
                    {
                        tempCharacters.Add(c);
                        if (c.team == 0)
                        {
                            c.SaveGene();
                        }
                        grid.characters.list.Remove(c);
                    }
                }
                setupTurnOrderIcons();
                foreach (Character ch in tempCharacters)
                {
                    toBeKilled.Remove(ch);
                }
            }

            if (RotateTo != null)
            {
                bool stillAnimating = false;
                foreach (Character c in grid.characters.list)
                {
                    if (c.hitType != EHitIcon.None)
                    {
                        stillAnimating = true;
                        break;
                    }
                }
                if (!stillAnimating)
                {
                    RotateBoardToCharacter(RotateTo);
                    RotateTo = null;
                }
            }

            if (OneTeamRemaining()) // End of Game
            {
                if (grid.characters.list[0].team == 1)
                {
                    battleVictoryDefeat.Text = "Victory";
                }
                else
                {
                    battleVictoryDefeat.Text = "Defeat";
                }
                if (!printedAIStuff)
                {
                    foreach (Character c in grid.characters.list)
                    {
                        if (c.team == 0)
                        {
                            c.SaveGene();
                        }
                    }
                    int totalIterationsAnalyzed = 0;
                    foreach (int i in IterationsPerTurn)
                    {
                        totalIterationsAnalyzed += i;
                    }
                    float avgIterationsPerTurn = (float) totalIterationsAnalyzed / IterationsPerTurn.Count;

                    int totalVisitsOfChoice = 0;
                    foreach (int j in VistsPerChoice)
                    {
                        totalVisitsOfChoice += j;
                    }
                    float avgVisitsPerChoice = (float) totalVisitsOfChoice / VistsPerChoice.Count;

                    int totalMovesAnalyzedPerTurn = 0;
                    foreach (int k in MovesAnalyzedPerTurn)
                    {
                        totalMovesAnalyzedPerTurn += k;
                    }
                    float avgMovesAnalyzedPerTurn = (float) totalMovesAnalyzedPerTurn / MovesAnalyzedPerTurn.Count;

                    int totalTurnsSimed = 0;
                    foreach (int k in TurnsPerSim)
                    {
                        totalTurnsSimed += k;
                    }
                    float avgTurnsPerSim = (float) totalTurnsSimed / TurnsPerSim.Count;

                    int totalChoiceWins = 0;
                    foreach (int l in WinsPerChoice)
                    {
                        totalChoiceWins += l;
                    }
                    float avgWinsPerChoice = (float)totalChoiceWins / WinsPerChoice.Count;

                    float SimsEndedEarlyPerTurn = (float)SimsEndedEarly / IterationsPerTurn.Count;

                    Console.WriteLine("Avg Iterations: " + avgIterationsPerTurn);
                    Console.WriteLine("Avg Moves Analyzed: " + avgMovesAnalyzedPerTurn);
                    Console.WriteLine("Avg Turns per Sim: " + avgTurnsPerSim);
                    Console.WriteLine("Avg Wins Per Choice: " + avgWinsPerChoice);
                    Console.WriteLine("Avg Visits of Choice: " + avgVisitsPerChoice);
                    Console.WriteLine("Damage By AI: " + damageDealtByAI);
                    Console.WriteLine("Sims ended Early Per Turn: " + SimsEndedEarlyPerTurn);
                    
                    printedAIStuff = true;
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
                if (thoughtBubble.position != Vector2.Zero)
                {
                    foreach (Cube c in grid.cubes)
                    {
                        if (validMoveSpaces.Contains(c.gridPos))
                        {
                            c.highLight = true;
                        }
                    }
                }
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
                    characterFace.visible = true;
                    characterHealthBar.visible = true;
                    characterMagicBar.visible = true;

                    characterName.Draw(spriteBatch);
                    characterFace.Draw(spriteBatch);
                    if (drawInfo)
                    {
                        characterStatus.Draw(spriteBatch);
                        characterStatusIcons.Draw(spriteBatch);
                    }
                    characterStatusBacker.Draw(spriteBatch);
                    if (characterHealthBar != null)
                    {
                        characterHealthBar.Draw(spriteBatch);
                    }
                    characterHealthText.Draw(spriteBatch);
                    if (characterMagicBar != null)
                    {
                        characterMagicBar.Draw(spriteBatch);
                    }
                    characterMagicText.Draw(spriteBatch);
                }
                else
                {
                    if (characterHealthBar != null)
                    {
                        characterFace.visible = false;
                        characterHealthBar.visible = false;
                        characterMagicBar.visible = false;
                    }
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

                if (drawBVD)
                {
                    battleVictoryDefeat.Draw(spriteBatch);
                    if (Game1.world.gameMode == 2)
                    {

                    }
                }

                Compass.Draw(spriteBatch);
                N.Draw(spriteBatch);
                E.Draw(spriteBatch);
                S.Draw(spriteBatch);
                W.Draw(spriteBatch);
                //leftButton.Draw(spriteBatch);
                //rightButton.Draw(spriteBatch);

                teamHealthBacker.Draw(spriteBatch);
                if (teamZeroHealth != null)
                {
                    teamZeroHealth.Draw(spriteBatch);
                }
                if (teamOneHealth != null)
                {
                    teamOneHealth.Draw(spriteBatch);
                }
                //teamOneHealth.Draw(spriteBatch);
                zeroHealthText.Draw(spriteBatch);
                oneHealthText.Draw(spriteBatch);

                peelStatusText.Draw(spriteBatch);
                foreach (Cube c in peelStatus)
                {
                    if (c != null)
                    {
                        c.Draw(spriteBatch);
                    }
                }
                //upButton.Draw(spriteBatch);
                //downButton.Draw(spriteBatch);

                playerStatusBacker.Draw(spriteBatch);
                if (playerHealthBar != null)
                {
                    playerHealthBar.Draw(spriteBatch);
                }
                playerHealthText.Draw(spriteBatch);
                if (playerMagicBar != null)
                {
                    playerMagicBar.Draw(spriteBatch);
                }
                playerMagicText.Draw(spriteBatch);

                foreach (IButton b in buttons)
                {
                    b.Draw(spriteBatch);
                }

                foreach (Descriptor d in descriptors)
                {
                    d.Draw(spriteBatch);
                }

            }
        }

        public void endTurn(GameTime gameTime)
        {
            turn++;
            recalcTeamHealthBar();
            grid.peel = grid.height - 1;
            recalcPeelStatus();
            Character tempCharacter = grid.characters.list[0];
            grid.characters.list.Remove(tempCharacter);
            grid.characters.list.Add(tempCharacter);

            UpdatePersonalityScores();

            CalcValidMoveSpaces();

            Character player = grid.characters.list[0];

            //Console.WriteLine("recalcStatusBars nextCharacter: " + player.name);
            recalcStatusBars();

            setupTurnOrderIcons();

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
                playerName.position = new Vector2(1920 / 3 - 300, 970);
                playerName.color = Color.Green;
                playerStatus.color = Color.Green;

            }
            else if (player.team == 1)
            {
                playerName.position = new Vector2(1920 / 3 - 300, 750);
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
                thoughtBubble.position.X += 10;
                thoughtBubble.position.Y -= 50;
                Game1.world.triggerAIBool = true;
                //Game1.world.triggerAI(this, gameTime);
            }
            else if (Game1.world.gameMode == 2)
            {
                thoughtBubble.position = player.position;
                thoughtBubble.position.X += 10;
                thoughtBubble.position.Y -= 50;
                Game1.world.triggerPlayerBool = true;
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
                turnOrderIcons[index].position = new Vector2(offset + index * width - 9, 5);

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
            Level clone = (Level)this.MemberwiseClone();
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
            clone.grid = (Grid)grid.Clone();
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

        public IGameState PlayRandomlyUntilTheEnd()
        {
            //Console.WriteLine("PlayingRandomlyUntilEnd");
            int startTurn = turn;
            int i = 0;
            bool checkUnwinnable = true;
            int[] genes = new int[7];
            genes = grid.characters.list.First().genes;
            while (!OneTeamRemaining())
            {
                /*if (turn > (maxRollout + maxPlayout))
                {
                    SimsEndedEarly++;
                    Console.WriteLine("Too Many Moves");
                    break;
                }*/
                float playerTeamHealth = 0;
                float aiTeamHealth = 0;
                foreach (Character c in grid.characters.list)
                {
                    if (c.team == 0)
                    {
                        aiTeamHealth += c.health;
                    }
                    else
                    {
                        playerTeamHealth += c.health;
                    }
                }
                if (checkUnwinnable && playerTeamHealth / aiTeamHealth >= 4) // Good for increasing number of moves evaluated
                {
                    if (startTurn == turn)
                    {
                        checkUnwinnable = false;
                    }
                    else
                    {
                        Game1.world.level.SimsEndedEarly++;
                        Console.WriteLine("Unwinnable");
                        break;
                    }
                }
                List<Move> moves = (List<Move>)GetMoves();
                if (true && moves.Count > 0)//turn > startTurn + maxPlayout || maxPlayout == 0) // rollout
                {
                    int random = World.Random.Next(moves.Count);
                    DoMove(moves[random]);
                }
                /*else // playout
                {
                    if (grid.characters.list[0].team == 0) // ai playout uses greedy evaluation/score function
                    {
                        DoMove(GetBestMove(moves, grid.characters.list.First().team, genes));
                    }
                    else
                    {
                        DoMove(GetOpponnentMove(moves, grid.characters.list.First().team));
                    }
                }*/
                i++;
            }
            Game1.world.level.TurnsPerSim.Add(turn - startTurn);
            Console.WriteLine("Game took " + turn + " moves");
            return this;
        }

        /*public void Simulate(int maxPlayout, int maxRollout)
        {
            //Console.WriteLine("PlayingRandomlyUntilEnd");
            int startTurn = turn;
            int i = 0;
            bool checkUnwinnable = true;
            while (!OneTeamRemaining())
            {
                /*if (turn > (maxRollout + maxPlayout))
                {
                    SimsEndedEarly++;
                    Console.WriteLine("Too Many Moves");
                    break;
                }*//*
                float playerTeamHealth = 0;
                float aiTeamHealth = 0;
                foreach (Character c in grid.characters.list)
                {
                    if (c.team == 0)
                    {
                        aiTeamHealth += c.health;
                    }
                    else
                    {
                        playerTeamHealth += c.health;
                    }
                }
                if (checkUnwinnable && playerTeamHealth / aiTeamHealth >= 4) // Good for increasing number of moves evaluated
                {
                    if (startTurn == turn)
                    {
                        checkUnwinnable = false;
                    }
                    else
                    {
                        Game1.world.level.SimsEndedEarly++;
                        Console.WriteLine("Unwinnable");
                        break;
                    }
                }
                List<Move> moves = (List<Move>)GetMoves();
                if (true)//turn > startTurn + maxPlayout || maxPlayout == 0) // rollout
                {
                    int random = World.Random.Next(moves.Count);
                    DoMove(moves[random]);
                }
                else // playout
                {
                    if (grid.characters.list[0].team == 0) // ai playout uses greedy evaluation/score function
                    {
                        DoMove(GetBestMove(moves, grid.characters.list.First().team));
                    }
                    else
                    {
                        DoMove(GetOpponnentMove(moves, grid.characters.list.First().team));
                    }
                }
                i++;
            }
            Game1.world.level.TurnsPerSim.Add(turn - startTurn);
            Console.WriteLine("Game took " + turn + " moves");
        }*/

        public float Playout(Move move, int maxPlayout, float learningRate=.75f)
        {
            Level clone = (Level)Clone();
            clone.DoMove(move);
            float score = 0;
            int startTurn = clone.turn;
            int i = 0;
            int team = grid.characters.list.First().team;
            int[] genes = new int[7];
            genes = grid.characters.list.First().genes;
            while (clone.turn <= startTurn + maxPlayout)
            {
                List<Move> moves = (List<Move>)clone.GetMoves();
                // playout
                if (clone.grid.characters.list[0].team == 0) // ai playout uses greedy evaluation/score function
                {
                    Move m = clone.GetBestMove(moves, clone.grid.characters.list.First().team, genes);
                    clone.DoMove(m);
                    score += (float) Math.Pow(learningRate, clone.turn - startTurn) * clone.Score(team, genes); // Learning discount rule
                }
                else
                {
                    Move m = clone.GetOpponnentMove(moves, clone.grid.characters.list.First().team);
                    clone.DoMove(m);
                    score += (float)Math.Pow(learningRate, clone.turn - startTurn) * clone.Score(team, genes); // Learning discount rule
                }
                i++;
            }
            Console.WriteLine(move.Name + " Playout Score: " + score);
            return score;
        }

        public Move GetBestMove(List<Move> moves, int team, int[] genes)
        {
            Move result = moves[0];
            int score = Score(result, team, genes);
            foreach (Move m in moves)
            {
                int tempScore = Score(m, team, genes);
                if (tempScore > score)
                {
                    result = m;
                    score = tempScore;
                }
            }
            return result;
        }

        /*public int Score(Move move, int team)
        {
            int score = 0;
            Level clone = (Level)Clone();
            clone.DoMove(move);
            if (move.noneMoveBeforeMoveAfter > 0)
            {
                score += 1;
            }
            if (move.nothingAttackSpecial > 0)
            {
                score += 2;
            }
            foreach (Character c in clone.grid.characters.list)
            {
                if (c.team == team)
                {
                    if (c == clone.grid.characters.list.First())
                    {
                        score += 50;// 100; // if they are still alive
                    }
                    else
                    {
                        score += 25;//50; // if they have ally alive
                    }
                    score += c.health;
                }
                else
                {
                    score -= 25;// 50; // if they have enemy alive
                    score -= c.health;
                }
            }
            return score;
        }

        public int Score(int team)
        {
            int score = 0;
            foreach (Character c in grid.characters.list)
            {
                if (c.team == team)
                {
                    if (c == grid.characters.list.First())
                    {
                        score += 50;// 100; // if they are still alive
                    }
                    else
                    {
                        score += 25;//50; // if they have ally alive
                    }
                    score += c.health;
                }
                else
                {
                    score -= 25;// 50; // if they have enemy alive
                    score -= c.health;
                }
            }
            return score;
        }*/

        public int Score(Move move, int team, int[] genes)
        {
            int score = 0;
            Level clone = (Level)Clone();
            clone.DoMove(move);
            if (move.noneMoveBeforeMoveAfter > 0)
            {
                score += genes[0];
            }
            if (move.nothingAttackSpecial > 0)
            {
                score += genes[1];
            }
            int allies = 0;
            int enemies = 0;
            foreach (Character c in clone.grid.characters.list)
            {
                if (c.team == team)
                {
                    if (c.job == grid.characters.list.First().job && c.personality == grid.characters.list.First().personality)
                    {
                        score += genes[2];// 100; // if they are still alive
                    }
                    else
                    {
                        score += genes[3];//50; // if they have ally alive
                    }
                    score += c.health * genes[4];
                    allies++;
                }
                else
                {
                    score -= genes[5];// 50; // if they have enemy alive
                    score -= c.health * genes[6];
                    enemies++;
                }
            }
            if (allies > 0 && enemies == 0)
            {
                score += 1000;
            }
            else if (allies == 0 && enemies > 0)
            {
                score -= 1000;
            }
            return score;
        }

        public int Score(int team, int[] genes)
        {
            int score = 0;
            int allies = 0;
            int enemies = 0;
            foreach (Character c in grid.characters.list)
            {
                if (c.team == team)
                {
                    if (c.job == grid.characters.list.First().job && c.personality == grid.characters.list.First().personality)
                    {
                        score += genes[2];// 100; // if they are still alive
                    }
                    else
                    {
                        score += genes[3];//50; // if they have ally alive
                    }
                    score += c.health * genes[4];
                    allies++;
                }
                else
                {
                    score -= genes[5];// 50; // if they have enemy alive
                    score -= c.health * genes[6];
                    enemies++;
                }
            }
            if (allies > 0 && enemies == 0)
            {
                score += 1000;
            }
            else if (allies == 0 && enemies > 0)
            {
                score -= 1000;
            }
            return score;
        }

        // Some quick move picking to represent human moves
        public Move GetOpponnentMove(List<Move> moves, int team)
        {
            Move result = moves[0];
            Character activeCharacter = grid.characters.list.First();
            Character target;
            if (activeCharacter.job != EJob.Healer)
            {
                if (team == 0)
                {
                    target = GetNearestMemberOfTeam(activeCharacter, 1);
                }
                else
                {
                    target = GetNearestMemberOfTeam(activeCharacter, 0);
                }
            }
            else // Healer
            {
                target = GetNearestMemberOfTeam(activeCharacter, team);
            }
            Move bestMove = null;
            if (target != null)
            {
                int dist = int.MaxValue;
                Move[] tempMoves = new Move[moves.Count];
                moves.CopyTo(tempMoves);
                foreach (Move m in tempMoves)
                {
                    if (activeCharacter.job == EJob.Healer || activeCharacter.job == EJob.Mage) // mage and healer just want to use special on target
                    {
                        if (m.attackSpecialPos == target.gridPos && m.nothingAttackSpecial == 2)
                        {
                            return m;
                        }
                    }
                    else
                    {
                        if (m.attackSpecialPos == target.gridPos && m.nothingAttackSpecial == 1) // everyone else just wants to attack target
                        {
                            return m;
                        }
                    }
                    //int tempDist = Game1.world.cubeDist(m.movePos, target.gridPos);                    
                    Graph graph = new Graph(activeCharacter, grid);
                    List<Vector3> tempList = graph.AStar(target.gridPos);
                    int tempDist;
                    if (tempList == null)
                    {
                        tempDist = Game1.world.cubeDist(m.movePos, target.gridPos); // int.MaxValue;
                    }
                    else
                    {
                        tempDist = tempList.Count;
                    }

                    if (tempDist < dist)
                    {
                        dist = tempDist;
                    }
                    if (dist != tempDist)
                    {
                        moves.Remove(m);
                    }

                }
                tempMoves = new Move[moves.Count];
                moves.CopyTo(tempMoves);
                foreach (Move m in tempMoves) // Delete earlier closest dist moves
                {
                    //int tempDist = Game1.world.cubeDist(m.movePos, target.gridPos);
                    Graph graph = new Graph(activeCharacter, grid);
                    List<Vector3> tempList = graph.AStar(target.gridPos);
                    int tempDist;
                    if (tempList == null)
                    {
                        tempDist = Game1.world.cubeDist(m.movePos, target.gridPos); // int.MaxValue;
                    }
                    else
                    {
                        tempDist = tempList.Count;
                    }

                    if (tempDist != dist)
                    {
                        moves.Remove(m);
                    }
                    else
                    {
                        break;
                    }
                }
                foreach (Move m in moves) // go through list of closest moves
                {
                    if (bestMove == null)
                    {
                        bestMove = m;
                    }
                    if (m.nothingAttackSpecial == 1)
                    {
                        if (activeCharacter.job != EJob.Healer && activeCharacter.job != EJob.Mage && m.attackSpecialPos == target.gridPos)
                        {
                            bestMove = m;
                            break;
                        }
                    }
                    else if (m.nothingAttackSpecial == 2)
                    {
                        if (activeCharacter.job == EJob.Healer || activeCharacter.job == EJob.Mage && m.attackSpecialPos == target.gridPos)
                        {
                            bestMove = m;
                            break;
                        }
                    }
                }
            }
            else
            {
                bestMove = moves.First();
            }
            return bestMove;
        }

        public Character GetNearestMemberOfTeam(Character activeCharacter, int team)
        {
            Character result = null;
            int dist = 0;
            foreach(Character c in grid.characters.list)
            {
                if (c.team == team && c != activeCharacter) {
                    Graph graph = new Graph(activeCharacter, grid);
                    List<Vector3> tempList = graph.AStar(c.gridPos);
                    int tempDist;
                    if (tempList == null)
                    {
                        tempDist = Game1.world.cubeDist(activeCharacter.gridPos, c.gridPos); // int.MaxValue;
                    }
                    else
                    {
                        tempDist = tempList.Count;
                    }
                    //int tempDist = Game1.world.cubeDist(activeCharacter.gridPos, c.gridPos);
                    if (result == null || tempDist < dist)
                    {
                        result = c;
                        dist = tempDist;
                    }
                }
            }
            return result;
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
            if (attacked)
            {
                if (moved)
                {
                    moveButton.SetTextColor(Color.Gray);
                }
                else
                {
                    moveButton.SetTextColor(Color.White);
                }
            }
            else
            {
                if (moved)
                {
                    moveButton.SetText("Undo");
                    moveButton.SetTextColor(Color.White);
                }
                else
                {
                    moveButton.SetText("Move");
                    moveButton.SetTextColor(Color.White);
                }
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
        }

        public void RotateToActiveCharacter()
        {
            RotateBoardToCharacter(grid.characters.list[0]);
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
                    if (tempList != null && tempList.Count <= activeCharacter.move) 
                    {
                        /*Console.WriteLine("\nStart: " + activeCharacter.gridPos + " Dest: " + c.gridPos);
                        foreach(Vector3 v in tempList)
                        {
                            Console.Write(v + " ");
                        }*/
                        validMoveSpaces.Add(c.gridPos);
                        validMovePaths.Add(tempList);
                    }
                }
            }
        }

        public void RotateBoardToCharacter(Character c)
        {
            Character player = c;

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

        public void UpdatePersonalityScores()
        {
            foreach (Character c in grid.characters.list)
            {
                if (c.team == 0)
                {
                    if (c.personality == EPersonality.Default)
                    {
                        foreach (Character character in grid.characters.list)
                        {
                            if (character.team == 0)
                            {
                                c.personalityScore += character.health;
                            }
                            else
                            {
                                c.personalityScore -= character.health;
                            }
                        }
                    }
                    else if (c.personality == EPersonality.Aggressive)
                    {
                        c.personalityScore += 659;
                        foreach (Character character in grid.characters.list)
                        {
                            if (character.team == 1)
                            {
                                c.personalityScore -= character.health;
                            }
                        }
                    }
                    else if (c.personality == EPersonality.Defensive)
                    {
                        foreach (Character character in grid.characters.list)
                        {
                            if (character.team == 0)
                            {
                                c.personalityScore += character.health;
                            }
                        }
                    }
                    else if (c.personality == EPersonality.SelfishAggressive)
                    {
                        c.personalityScore = c.damageInflicted;
                    }
                    else if (c.personality == EPersonality.SelfishDefensive)
                    {
                        c.personalityScore += c.health;
                    }
                    else if (c.personality == EPersonality.Selfish)
                    {
                        c.personalityScore += c.damageInflicted + c.health;
                    }
                    else if (c.personality == EPersonality.Survivalist)
                    {
                        c.personalityScore = turn;
                    }
                }
            }
        }
    }
}
