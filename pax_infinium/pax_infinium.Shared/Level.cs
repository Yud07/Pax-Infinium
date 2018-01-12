using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using MCTS.V2.Interfaces;
using MCTS.V2.UCT;
using MCTS.Enum;

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

        public Level(GraphicsDeviceManager graphics, string seed)
        {
            random = World.Random;
            grid = new Grid(graphics, seed, 10, 10, 5, 1, 1, 1, 1, random);
            //background = new Background(World.textureManager["BG-Layer"], graphics.GraphicsDevice.Viewport);
            background = new Background(Game1.world.textureConverter.GenRectangle(1600, 900, Color.SkyBlue), graphics.GraphicsDevice.Viewport);
            //players.Add(new Player("Human"));
            players.Add(new Player("AI"));
            players.Add(new Player("Human"));
            grid.characters = new Characters();
            grid.characters.AddCharacter("Blue Soldier", 0, 0, grid.origin, new Vector3(5, 7, 3), "nw", graphics);
            grid.characters.AddCharacter("Red Soldier", 0, 1, grid.origin, new Vector3(4, 2, 3), "ne", graphics);
            grid.characters.AddCharacter("Blue Hunter", 1, 0, grid.origin, new Vector3(6, 7, 3), "sw", graphics);
            grid.characters.AddCharacter("Red Hunter", 1, 1, grid.origin, new Vector3(5, 2, 3), "se", graphics);
            grid.characters.AddCharacter("Blue Mage", 2, 0, grid.origin, new Vector3(7, 7, 3), "nw", graphics);
            grid.characters.AddCharacter("Red Mage", 2, 1, grid.origin, new Vector3(6, 2, 3), "ne", graphics);
            grid.characters.AddCharacter("Blue Healer", 3, 0, grid.origin, new Vector3(4, 7, 3), "nw", graphics);
            grid.characters.AddCharacter("Red Healer", 3, 1, grid.origin, new Vector3(3, 2, 3), "ne", graphics);
            grid.characters.AddCharacter("Blue Thief", 4, 0, grid.origin, new Vector3(3, 7, 3), "nw", graphics);
            grid.characters.AddCharacter("Red Thief", 4, 1, grid.origin, new Vector3(4, 1, 3), "ne", graphics);
            grid.characters.list.Sort(Character.CompareBySpeed);
            grid.characters.list.Reverse();
            /*foreach (Cube cube in grid.cubes)
            {
                if (grid.characters.list[0].gridPos == cube.gridPos)
                {
                    cube.invert = true;
                }
            }*/
            //turnOrder = new string[grid.characters.list.Count];
            //turnOrder[0] = grid.characters.list[0].name;
            //turnOrder[1] = grid.characters.list[1].name;
            turn = 0;
            moved = false;
            attacked = false;
            rotated = false;
            text = new TextItem(World.fontManager["InfoFont"], "Turn:" + turn.ToString());
            text.position = new Vector2(100, 30);
            text.color = Color.Black;
            text.scale = 3;


            playerName = new TextItem(World.fontManager["InfoFont"], grid.characters.list[0].name);
            playerName.position = new Vector2(350, 1050);            
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
        }

        public void Draw(SpriteBatch spriteBatch)
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
        }

        public void endTurn()
        {
            turn++;
            Character tempCharacter = grid.characters.list[0];
            grid.characters.list.Remove(tempCharacter);
            grid.characters.list.Add(tempCharacter);

            setupTurnOrderIcons();

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
            /*else
            {
                Console.WriteLine("south");
            }*/

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
            int mpGain = 10;
            if (player.mp + mpGain <= player.maxMP)
            {
                player.mp += mpGain;
            }
            else if (player.mp + mpGain > player.maxMP)
            {
                player.mp = player.maxMP;
            }
            //grid.onCharacterMoved();
            moved = false;
            attacked = false;
            rotated = false;

            Game1.world.triggerAI();
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
        public object Clone()
        {
            Level clone = (Level) this.MemberwiseClone();
            clone.grid.characters.list = grid.characters.list.Select(item => (Character)item.Clone()).ToList();
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
            return clone;
        }

        public IPlayer PlayerJustMoved => players[grid.characters.list.Last().team]; // thief special will break this on success

        public IEnumerable<IMove> GetMoves() // need to add character rotation
        {
            return grid.characters.list.First().GetMoves(this);
        }

        public IGameState PlayRandomlyUntilTheEnd()
        {
            Console.WriteLine("PlayingRandomlyUntilEnd");
            int i = 0;
            Level clone = (Level) Clone();
            while (!clone.OneTeamRemaining())
            {
                Console.WriteLine("Turn " + i);
                List<Move> moves = (List<Move>) clone.GetMoves();
                int random = World.Random.Next(moves.Count);
                clone = (Level) moves[random].DoMove();
                i++;
            }
            return clone;
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
            if (grid.characters.list[0].team == goal)
            {
                return EGameFinalStatus.GameWon;
            }
            else
            {
                return EGameFinalStatus.GameLost;
            }
        }

    }
}
