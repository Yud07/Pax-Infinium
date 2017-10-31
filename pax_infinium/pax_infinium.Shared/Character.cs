using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace pax_infinium
{
    public class Character : IDrawable1
    {
        public Sprite top;
        public Vector2 position;
        public Vector3 gridPos;
        public Vector2 origin;
        public Texture2D topTex;
        GraphicsDeviceManager graphics;
        SpriteSheetInfo spriteSheetInfo;
        public int cubeWidth = 64;
        public int cubeHeight = (int)(64 * 1.5 + 1);
        public int move;
        public string name;
        public int health;
        public int mp;
        public int WAttack;
        public int WDefense;
        public int MAttack;
        public int MDefense;
        public int jump;
        public int evasion;
        public int speed;
        TextItem text;
        public int team;


        public Character(string name, int team, Vector2 origin, Vector3 gridPos, Texture2D topTex, GraphicsDeviceManager graphics, SpriteSheetInfo spriteSheetInfo)
        {
            this.name = name;
            this.topTex = topTex;
            this.origin = origin;
            this.gridPos = gridPos;
            this.position = origin + Game1.world.twoDToIso(new Point((int)(gridPos.X * cubeWidth), (int)(gridPos.Y * cubeHeight * .65f))).ToVector2();
            this.position.Y -= gridPos.Z * cubeHeight * .65F + topTex.Height / 2;
            this.graphics = graphics;
            this.spriteSheetInfo = spriteSheetInfo;
            //this.move = World.Random.Next(3, 6);
            //this.health = World.Random.Next(300, 501);
            //this.WAttack = World.Random.Next(100, 301);
            //this.mp = World.Random.Next(50, 151);
            //this.WDefense = World.Random.Next(100, 251);
            //this.jump = World.Random.Next(2, 5);
            //this.evasion = World.Random.Next(0, 6);
            //this.speed = World.Random.Next(75, 100);
            this.team = team;

            top = new Sprite(topTex, graphics, spriteSheetInfo);
            top.position = position;
            top.origin = new Vector2(topTex.Width / 2, topTex.Height / 2);
            top.scale = 1f;

            text = new TextItem(World.fontManager["InfoFont"], DrawOrder().ToString());
            text.position = position;

            //Console.WriteLine("Character X:" + position.X + " Y:" + position.Y);
        }

        public void recalcPos()
        {
            this.position = origin + Game1.world.twoDToIso(new Point((int)(gridPos.X * cubeWidth), (int)(gridPos.Y * cubeHeight * .65f))).ToVector2();
            this.position.Y -= gridPos.Z * cubeHeight * .65F + topTex.Height / 2;

            top.position = position;

            text.Text = DrawOrder().ToString();
            text.position = position;

            //darken();
        }

        public void Update(GameTime gameTime)
        {
            //recalcPos();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            top.Draw(spriteBatch);
            //text.Draw(spriteBatch);
        }

        public int DrawOrder()
        {
            return (int)(gridPos.X + gridPos.Y + gridPos.Z);
        }

        public void SetAlpha(float alpha)
        {
            top.alpha = alpha;
            text.alpha = alpha;
        }

        public void onCharacterMoved()
        {
            SetAlpha(1f);
            foreach (Character character in Game1.world.level.grid.characters.list)
            {
                if (character != this && DrawOrder() > character.DrawOrder() && Math.Abs(Game1.world.cubeDist(gridPos, character.gridPos)) < 5)
                {
                    SetAlpha(.5f);
                }
            }
        }

        public static int CompareBySpeed(Character x, Character y){
            if (x == null || y == null)
            {
                throw new ArgumentNullException();
            }
            if (x.speed < y.speed) {
                return -1;
            }
            else if (x.speed == y.speed)
            {
                return 0;
            }
            else
            {
                return 1;
            }
    
        }
    }
}
