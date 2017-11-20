using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;

namespace pax_infinium
{
    public class Level
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

        public Level(GraphicsDeviceManager graphics, string seed)
        {
            random = World.Random;
            grid = new Grid(graphics, seed, 10, 10, 5, 1, 1, 1, 1, random);
            //background = new Background(World.textureManager["BG-Layer"], graphics.GraphicsDevice.Viewport);
            background = new Background(Game1.world.textureConverter.GenRectangle(1600, 900, Color.SkyBlue), graphics.GraphicsDevice.Viewport);
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
            playerStatus.Draw(spriteBatch);
            playerFace.Draw(spriteBatch);
            playerStatusIcons.Draw(spriteBatch);
            if (characterName.Text != "")
            {
                characterName.Draw(spriteBatch);
                characterStatus.Draw(spriteBatch);
                characterFace.Draw(spriteBatch);
                characterStatusIcons.Draw(spriteBatch);
            }
        }

        public void endTurn()
        {
            turn++;
            Character tempCharacter = grid.characters.list[0];
            grid.characters.list.Remove(tempCharacter);
            grid.characters.list.Add(tempCharacter);
            //Console.WriteLine("turn: " + turn);
            //text.Text = turnOrder[turn % turnOrder.Length] + "'s turn:" + turn.ToString();
            text.Text = "Turn:" + turn.ToString();
            playerName.Text = grid.characters.list[0].name;
            String t = grid.characters.list[0].health + "              " + grid.characters.list[0].mp;
            t += "\n\n\n" + grid.characters.list[0].move + "                 " + grid.characters.list[0].jump;
            t += "\n\n\n" + grid.characters.list[0].speed + "              " + grid.characters.list[0].evasion;
            t += "\n\n\n" + grid.characters.list[0].WAttack + "              " + grid.characters.list[0].MAttack;
            t += "\n\n\n" + grid.characters.list[0].WDefense + "              " + grid.characters.list[0].MDefense;
            t += "\n\n\n" + grid.characters.list[0].weaponRange + "                 " + grid.characters.list[0].magicRange;
            playerStatus.Text = t;
            playerFace.tex = grid.characters.list[0].faceLeft;
            if (grid.characters.list[0].team == 0)
            {
                playerName.color = Color.Blue;
                playerStatus.color = Color.Blue;

            }
            else if (grid.characters.list[0].team == 1)
            {
                playerName.color = Color.Red;
                playerStatus.color = Color.Red;
            }
            int mpGain = 10;
            if (grid.characters.list[0].mp + mpGain <= grid.characters.list[0].maxMP)
            {
                grid.characters.list[0].mp += mpGain;
            }
            else if (grid.characters.list[0].mp + mpGain > grid.characters.list[0].maxMP)
            {
                grid.characters.list[0].mp = grid.characters.list[0].maxMP;
            }
            //grid.onCharacterMoved();
            moved = false;
            attacked = false;
            rotated = false;
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
    }
}
