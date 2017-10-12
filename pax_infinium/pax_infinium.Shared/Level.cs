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
        public Characters characters;
        public int turn;
        public bool moved;
        public TextItem text;
        public string[] turnOrder;

        
        public Background background;
        public int perspective;

        public Level(GraphicsDeviceManager graphics, string seed)
        {
            random = World.Random;
            grid = new Grid(graphics, seed, 10, 10, 5, 1, 1, 1, 1, random);
            background = new Background(World.textureManager["BG-Layer"], graphics.GraphicsDevice.Viewport);
            characters = new Characters();
            characters.list.Add(new Character("player", grid.origin, new Vector3(5, 5, 3), Game1.world.textureConverter.GenRectangle(64, 128, Color.Blue), graphics, new SpriteSheetInfo(64, 128)));
            characters.list.Add(new Character("enemy0", grid.origin, new Vector3(3, 7, 3), Game1.world.textureConverter.GenRectangle(64, 128, Color.Red), graphics, new SpriteSheetInfo(64, 128)));
            turnOrder = new string[characters.list.Count];
            turnOrder[0] = characters.list[0].name;
            turnOrder[1] = characters.list[1].name;
            turn = 0;
            moved = false;
            text = new TextItem(World.fontManager["InfoFont"], turnOrder[turn % turnOrder.Length] + "'s turn:" + turn.ToString());
            text.position = new Vector2(175, 30);
            text.color = Color.Blue;
            text.scale = 3;
        }

        public void Update(GameTime gameTime)
        {
            if (moved == true)
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
                moved = false;
            }
            grid.Update(gameTime);
            characters.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            grid.Draw(spriteBatch);
            characters.Draw(spriteBatch);
            text.Draw(spriteBatch);
        }
        
    }
}
