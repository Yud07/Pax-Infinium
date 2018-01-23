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
        public TextItem statusText;
        public int maxMP;
        public Texture2D faceLeft;
        public Texture2D faceRight;


        public Character(string name, int team, Vector2 origin, Texture2D nwTex, Texture2D neTex, Texture2D swTex, Texture2D seTex, Texture2D faceL, Texture2D faceR, GraphicsDeviceManager graphics, SpriteSheetInfo spriteSheetInfo)
        {
            this.name = name;
            this.nwTex = nwTex;
            this.neTex = neTex;
            this.swTex = swTex;
            this.seTex = seTex;
            this.origin = origin;
            this.gridPos = Vector3.Zero;
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
            this.direction = "ne";
            this.faceLeft = faceL;
            this.faceRight = faceR;

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
            text.position = position + new Vector2(0, -60);
            text.color = Color.Red;

            statusText = new TextItem(World.fontManager["InfoFont"], " ");
            statusText.scale = 1.5f;
            statusText.position = position + new Vector2(-25, -25);
            if (team == 0)
            {
                statusText.color = Color.Blue;
            }
            else if (team == 1)
            {
                statusText.color = Color.Red;
            }

            //Console.WriteLine("Character X:" + position.X + " Y:" + position.Y);
        }

        public void recalcPos()
        {
            this.position = origin + Game1.world.twoDToIso(new Point((int)(gridPos.X * cubeWidth), (int)(gridPos.Y * cubeHeight * .65f))).ToVector2();
            this.position.Y -= gridPos.Z * cubeHeight * .65F + nwTex.Height / 2;

            sprite.position = position;

            //text.Text = DrawOrder().ToString();
            text.position = position + new Vector2(0, -60);

            statusText.position = position + new Vector2(-25, -25);

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
            statusText.Text = health.ToString();
            statusText.Draw(spriteBatch);
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

        public void onCharacterMoved(Level level)
        {
            SetAlpha(1f);
            foreach (Character character in level.grid.characters.list)
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

        public void onHighlightMoved(Cube c)
        {
            if (gridPos != c.gridPos && DrawOrder() > c.DrawOrder() && Math.Abs(Game1.world.cubeDist(gridPos, c.gridPos)) < 5)
            {
                SetAlpha(.5f);
            }
        }

        public bool isAdjacent(Vector3 v)
        {
            Vector3 diff = gridPos - v;


            return Game1.world.cubeDist(gridPos, v) == 1 && (diff.X == 0 || diff.Y == 0);
        }

        public int[] calculateAttack(Character character)
        {
            bool canAttack;

            canAttack = character != this;

            if (job != 1)
            {
                canAttack = canAttack && character.isAdjacent(gridPos);
            }
            else
            {
                canAttack = canAttack && Game1.world.cubeDist(gridPos, character.gridPos) <= weaponRange;
            }

            if (canAttack)
            {
                int angleModifier = 0;
                if (character.direction.Contains(direction[0].ToString()))
                    angleModifier++;
                if (character.direction.Contains(direction[1].ToString()))
                    angleModifier++;

                int chance = 90 - character.evasion + angleModifier * 5;
                int damage = (int)((WAttack + WAttack * angleModifier / 2 - (character.WDefense / 2)) * .5);
                return new int[] { chance, damage };
            }
            else
            {
                return new int[] { 0, 0};
            }
        }

        public Character attack(Character character)
        {
            int[] chanceDamage = calculateAttack(character);
            int chance = chanceDamage[0];
            int damage = chanceDamage[1];

            //Console.WriteLine("Chance to hit: " + chance + "%");
            if (chance >= World.Random.Next(1, 101))
            {
                //Console.WriteLine("Hit! " + character.name + " takes " + damage + " damage!");
                character.health -= damage;
                
                if (character.health <= 0)
                {
                    //Console.WriteLine(character.name + " has died!");
                    return character;
                }
                else
                {
                    //Console.WriteLine(character.name + " health: " + character.health);
                    return null;
                }
            }
            else
            {
                //Console.WriteLine("Miss!");
                return null;
            }
        }

        public Character attack(Character character, GameTime gameTime)
        {
            int[] chanceDamage = calculateAttack(character);
            int chance = chanceDamage[0];
            int damage = chanceDamage[1];

            Console.WriteLine("Chance to hit: " + chance + "%");
            if (chance >= World.Random.Next(1, 101))
            {
                Console.WriteLine("Hit! " + character.name + " takes " + damage + " damage!");
                character.health -= damage;
                character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                character.text.Text = "-" + damage;
                character.text.color = Color.OrangeRed;

                if (character.health <= 0)
                {
                    Console.WriteLine(character.name + " has died!");
                    return character;
                }
                else
                {
                    Console.WriteLine(character.name + " health: " + character.health);
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Miss!");
                character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                character.text.Text = "Miss";
                character.text.color = Color.Green;
                return null;
            }
        }

        public bool inMoveRange(Vector3 pos, Level level)
        {
            return Game1.world.cubeDist(gridPos, pos) < move &&
                    level.grid.topOfColumn(pos) == pos.Z &&
                    Math.Abs(pos.Z - gridPos.Z) <= jump;
        }

        public void Move(Vector3 pos)
        {
            gridPos = pos;
            recalcPos();
        }

        public int[] calculateMageSpecial(Character character)
        {
            int chance = 100 - character.evasion;
            int spellModifier = 0;
            int damage = (int)((MAttack + spellModifier - (character.MDefense / 2)) * .75);
            return new int[] { chance, damage };
        }

        public Character MageSpecial(Character character)
        {
            int[] chanceDamage;
            chanceDamage = calculateMageSpecial(character);
            int chance = chanceDamage[0];
            if (chance >= World.Random.Next(1, 101))
            {
                int damage = chanceDamage[1];
                //Console.WriteLine("Hit! " + character.name + " takes " + damage + " damage!");
                character.health -= damage;
                
                if (character.health <= 0)
                {
                    //Console.WriteLine(character.name + " has died!");
                    return character;
                }
                else
                {
                    //Console.WriteLine(character.name + " health: " + character.health);
                }
            }
            else
            {
                //Console.WriteLine("Miss!");
            }
            return null;
        }

        public Character MageSpecial(Character character, GameTime gameTime)
        {
            int[] chanceDamage;
            chanceDamage = calculateMageSpecial(character);
            int chance = chanceDamage[0];
            if (chance >= World.Random.Next(1, 101))
            {
                int damage = chanceDamage[1];
                Console.WriteLine("Hit! " + character.name + " takes " + damage + " damage!");
                character.health -= damage;
                character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                character.text.Text = "-" + damage;
                character.text.color = Color.OrangeRed;

                if (character.health <= 0)
                {
                    Console.WriteLine(character.name + " has died!");
                    return character;
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
            return null;
        }

        public void HealerSpecial(Character character)
        {
            int health = 15;
            //Console.WriteLine(character.name + " heals " + health + " points!");
            character.health += health;
        }

        public void HealerSpecial(Character character, GameTime gameTime)
        {
            int health = 15;
            Console.WriteLine(character.name + " heals " + health + " points!");
            character.health += health;
            character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
            character.text.Text = "+" + health;
            character.text.color = Color.Green;
        }

        public int CalculateThiefSpecial(Character character)
        {
            int angleModifier = 0;
            if (character.direction.Contains(direction[0].ToString()))
                angleModifier++;
            if (character.direction.Contains(direction[1].ToString()))
                angleModifier++;

            int chance = 45 - character.evasion + angleModifier * 5;

            return chance;
        }

        public Character ThiefSpecial(Character character)
        {
            int chance = CalculateThiefSpecial(character);
            //Console.WriteLine("Chance to hit: " + chance + "%");
            if (chance >= World.Random.Next(1, 101))
            {

                //Console.WriteLine(character.name + " had their next turn stolen!");
                return character;
            }
            else
            {
                //Console.WriteLine("Miss!");
                return null;
            }
        }

        public Character ThiefSpecial(Character character, GameTime gameTime)
        {
            int chance = CalculateThiefSpecial(character);
            Console.WriteLine("Chance to hit: " + chance + "%");
            if (chance >= World.Random.Next(1, 101))
            {
                
                Console.WriteLine(character.name + " had their next turn stolen!");
                character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                character.text.Text = "Skipped!";
                character.text.color = Color.OrangeRed;
                return character;
            }
            else
            {
                Console.WriteLine("Miss!");
                character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                character.text.Text = "Miss";
                character.text.color = Color.Green;
                return null;
            }
        }

        public bool CanCast(int cost)
        {
            return mp >= cost;
        }

        public void payForCast(int cost)
        {
            mp -= cost;
        }

        public void payForCast(int cost, GameTime gameTime)
        {
            mp -= cost;
            textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
            text.Text = "-" + cost;
            text.color = Color.LightBlue;
        }

        public void SoldierSpecial(Character character)
        {
            int defenseBoost = 20;
            //Console.WriteLine(character.name + " gains " + defenseBoost + " melee defense!");
            character.MDefense += defenseBoost;
        }

        public void SoldierSpecial(Character character, GameTime gameTime)
        {
            int defenseBoost = 20;
            Console.WriteLine(character.name + " gains " + defenseBoost + " melee defense!");
            character.MDefense += defenseBoost;
            character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
            character.text.Text = "+" + defenseBoost;
            character.text.color = Color.Yellow;
        }

        public void Rotate(Vector3 pos)
        {
            float x = gridPos.X - pos.X;
            float y = gridPos.Y - pos.Y;

            if (Math.Abs(x) > Math.Abs(y))
            {
                if (x > 0)
                {
                    direction = "nw";
                    sprite.tex = nwTex;
                }
                else
                {
                    direction = "se";
                    sprite.tex = seTex;
                }
            }
            else
            {
                if (y > 0)
                {
                    direction = "ne";
                    sprite.tex = neTex;
                }
                else
                {
                    direction = "sw";
                    sprite.tex = swTex;
                }
            }
        }

        public void Rotate(String dir)
        {
            direction = dir;
            if (dir == "nw")
            {
                sprite.tex = nwTex;
            }
            else if (dir == "se")
            {
                sprite.tex = seTex;
            }
            else if (dir == "ne")
            {
                sprite.tex = neTex;
            }
            else if (dir == "sw")
            {
                sprite.tex = swTex;
            }
        }

        public bool InMagicRange(Vector3 pos)
        {
            return Game1.world.cubeDist(pos, gridPos) <= magicRange;
        }

        public bool InWeaponRange(Vector3 pos)
        {
            return Game1.world.cubeDist(pos, gridPos) <= weaponRange;
        }

        public List<Cube> GetMovePositions(Level level)
        {
            List<Cube> results = new List<Cube>();
            foreach (Cube cube in level.grid.cubes)
            {
                if (((inMoveRange(cube.gridPos, level) && level.grid.isVacant(cube.gridPos)) || cube.gridPos == gridPos) && level.grid.topOfColumn(cube.gridPos) == cube.gridPos.Z)
                {
                    results.Add(cube);
                }
            }
            return results;
        }

        public List<Move> GetMoves(Level level)
        {
            /*
                int noneMoveBeforeMoveAfter; // 0-2
                Vector3 movePos; // irrelevant if no movement
                int nothingAttackSpecial; // 0-2
                Vector3 attackSpecialPos; // irrelevant if nothingAttackSpecial is 0

                public Move(Level level, int noneMoveBeforeMoveAfter, Vector3 movement, int nothingAttackSpecial, Vector3 attackSpecialPos)
            */
            List<Move> moves = new List<Move>();
            List<Cube> movePositions = GetMovePositions(level);
            foreach (Cube cube in movePositions)
            {
                if (cube.gridPos == gridPos)
                {
                    moves.Add(new Move(0, gridPos, 0, Vector3.Zero)); // Do nothing at all
                    foreach (Character character in level.grid.characters.list)
                    {
                        if (character != this && InWeaponRange(character.gridPos))
                        {
                            moves.Add(new Move(0, gridPos, 1, character.gridPos)); // Don't move, Attack character
                        }
                    }
                    if (CanCast(8) && job != 1)
                    {
                        foreach (Cube cu in level.grid.cubes)
                        {
                            if (InMagicRange(cu.gridPos) && level.grid.topOfColumn(cu.gridPos) == cu.gridPos.Z)
                            {                                
                                if (job != 2 || job != 3)
                                {
                                    Character target = level.grid.CharacterAtPos(cu.gridPos);
                                    if (target != null && (target.team != team || job == 0))
                                    {
                                        moves.Add(new Move(0, gridPos, 2, cu.gridPos)); // Don't move, use special on character
                                    }
                                }
                                else
                                {
                                    moves.Add(new Move(0, gridPos, 2, cu.gridPos)); // Don't move, use special at cube
                                }
                            }
                        }
                    }
                }
                else
                {
                    moves.Add(new Move(1, cube.gridPos, 0, Vector3.Zero)); // Move before to cube, do nothing
                    moves.Add(new Move(2, cube.gridPos, 0, Vector3.Zero)); // Move after to cube, do nothing

                    //move before
                    foreach (Character character in level.grid.characters.list)
                    {
                        if (character != this && Game1.world.cubeDist(character.gridPos, cube.gridPos) <= weaponRange)
                        {
                            moves.Add(new Move(1, cube.gridPos, 1, character.gridPos)); // Move first, Attack character
                        }
                    }
                    if (CanCast(8) && job != 1)
                    {
                        foreach (Cube cu in level.grid.cubes)
                        {
                            if (Game1.world.cubeDist(cu.gridPos, cube.gridPos) <= magicRange && level.grid.topOfColumn(cu.gridPos) == cu.gridPos.Z)
                            {
                                if (job != 2 || job != 3)
                                {
                                    Character target = level.grid.CharacterAtPos(cu.gridPos);
                                    if ((target != null && target != this && (target.team != team || job == 0)) || (job == 0 && cu.gridPos == cube.gridPos))
                                    {
                                        moves.Add(new Move(1, cube.gridPos, 2, cu.gridPos)); // Move first, use special on character
                                    }
                                }
                                else
                                {
                                    moves.Add(new Move(1, cube.gridPos, 2, cu.gridPos)); // Move first, use special at cube
                                }
                            }
                        }
                    }

                    //move after
                    foreach (Character character in level.grid.characters.list)
                    {
                        if (character != this && InWeaponRange(character.gridPos))
                        {
                            moves.Add(new Move(2, cube.gridPos, 1, character.gridPos)); // Move after, Attack character
                        }
                    }
                    if (CanCast(8) && job != 1)
                    {
                        foreach (Cube cu in level.grid.cubes)
                        {
                            if (InMagicRange(cu.gridPos) && level.grid.topOfColumn(cu.gridPos) == cu.gridPos.Z)
                            {
                               if (job != 2 || job != 3)
                                {
                                    Character target = level.grid.CharacterAtPos(cu.gridPos);
                                    if (target != null && (target.team != team || job == 0))
                                    {
                                        moves.Add(new Move(2, cube.gridPos, 2, cu.gridPos)); // Move after, use special on character
                                    }
                                }
                                else
                                {
                                    moves.Add(new Move(2, cube.gridPos, 2, cu.gridPos)); // Move after, use special at cube
                                }
                            }
                        }
                    }
                }
            }

            return moves;
        }

        public object Clone()
        {
            Character clone = (Character)this.MemberwiseClone();
            String[] words = clone.name.Split(' ');
            if (words.Length > 2)
            {
                String number = words[words.Length - 1];
                int num = Int32.Parse(number);
                num++;
                clone.name = words[0] + " " + words[1] + " clone " + num;
            }
            else
            {
                clone.name += " clone 1";
            }
            return clone;
        }
    }
}
