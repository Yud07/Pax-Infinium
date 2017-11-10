using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace pax_infinium
{
    public class Character : IDrawable1
    {
        public Sprite sprite;
        public Vector2 position;
        public Vector3 gridPos;
        public Vector2 origin;
        public Texture2D nwTex;
        public Texture2D neTex;
        public Texture2D swTex;
        public Texture2D seTex;
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
        public TextItem text;
        public TimeSpan textTime;
        public int team;
        public int weaponRange;
        public int magicRange;
        public int job;
        public string direction;


        public Character(string name, int team, Vector2 origin, Vector3 gridPos, String direction, Texture2D nwTex, Texture2D neTex, Texture2D swTex, Texture2D seTex, GraphicsDeviceManager graphics, SpriteSheetInfo spriteSheetInfo)
        {
            this.name = name;
            this.nwTex = nwTex;
            this.neTex = neTex;
            this.swTex = swTex;
            this.seTex = seTex;
            this.origin = origin;
            this.gridPos = gridPos;
            this.position = origin + Game1.world.twoDToIso(new Point((int)(gridPos.X * cubeWidth), (int)(gridPos.Y * cubeHeight * .65f))).ToVector2();
            this.position.Y -= gridPos.Z * cubeHeight * .65F + nwTex.Height / 2;
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
            this.direction = direction;

            Texture2D tex;
            if (direction == "nw")
                tex = nwTex;
            else if (direction == "ne")
                tex = neTex;
            else if (direction == "sw")
                tex = swTex;
            else
                tex = seTex;

            sprite = new Sprite(tex, graphics, spriteSheetInfo);
            sprite.position = position;
            sprite.origin = new Vector2(tex.Width / 2, tex.Height / 2);
            sprite.scale = 1f;

            text = new TextItem(World.fontManager["ScoreFont"], " ");
            text.position = position + new Vector2(0, -25);
            text.color = Color.Red;

            //Console.WriteLine("Character X:" + position.X + " Y:" + position.Y);
        }

        public void recalcPos()
        {
            this.position = origin + Game1.world.twoDToIso(new Point((int)(gridPos.X * cubeWidth), (int)(gridPos.Y * cubeHeight * .65f))).ToVector2();
            this.position.Y -= gridPos.Z * cubeHeight * .65F + nwTex.Height / 2;

            sprite.position = position;

            //text.Text = DrawOrder().ToString();
            text.position = position + new Vector2(0, -25);

            //darken();
        }

        public void Update(GameTime gameTime)
        {
            //recalcPos();
            if (textTime < gameTime.TotalGameTime)
            {
                text.Text = " ";
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
            if (text.Text != " ")
            {
                text.Draw(spriteBatch);
            }
        }

        public int DrawOrder()
        {
            return (int)(gridPos.X + gridPos.Y + gridPos.Z);
        }

        public void SetAlpha(float alpha)
        {
            sprite.alpha = alpha;
            //text.alpha = alpha;
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
