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

        public Level(GraphicsDeviceManager graphics, string seed)
        {
            random = World.Random;
            grid = new Grid(graphics, seed, 10, 10, 5, 1, 1, 1, 1, random);
            background = new Background(World.textureManager["BG-Layer"], graphics.GraphicsDevice.Viewport);
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
            grid.characters.AddCharacter("Red Thief", 4, 1, grid.origin, new Vector3(2, 2, 3), "ne", graphics);
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
            text = new TextItem(World.fontManager["InfoFont"], grid.characters.list[0].name + "'s turn:" + turn.ToString());
            text.position = new Vector2(300, 30);
            text.color = Color.Blue;
            text.scale = 3;
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            grid.Draw(spriteBatch);
            //grid.characters.Draw(spriteBatch);
            text.Draw(spriteBatch);
        }

        public void endTurn()
        {
            turn++;
            Character tempCharacter = grid.characters.list[0];
            grid.characters.list.Remove(tempCharacter);
            grid.characters.list.Add(tempCharacter);
            //Console.WriteLine("turn: " + turn);
            //text.Text = turnOrder[turn % turnOrder.Length] + "'s turn:" + turn.ToString();
            text.Text = grid.characters.list[0].name + "'s turn:" + turn.ToString();
            if (grid.characters.list[0].team == 0)
            {
                text.color = Color.Blue;
            }
            else if (grid.characters.list[0].team == 1)
            {
                text.color = Color.Red;
            }
            //grid.onCharacterMoved();
            moved = false;
            attacked = false;
            rotated = false;
        }
        
    }
}
