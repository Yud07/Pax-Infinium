﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using pax_infinium.Enum;
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
        public EJob job;
        public EDirection direction;
        public TextItem statusText;
        public int maxMP;
        public Texture2D faceLeft;
        public Texture2D faceRight;

        public Action attackAction;
        public Action specialAction;

        public int startingHealth;
        public int startingMP;

        public List<Vector3> movePath;
        public TimeSpan moveTime;

        public Sprite healthBacker;
        public Sprite textBacker;


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
            this.direction = EDirection.Northeast;
            this.faceLeft = faceL;
            this.faceRight = faceR;

            Texture2D tex;
            if (direction == EDirection.Northwest)
                tex = nwTex;
            else if (direction == EDirection.Northeast)
                tex = neTex;
            else if (direction == EDirection.Southwest)
                tex = swTex;
            else
                tex = seTex;

            sprite = new Sprite(tex, graphics, spriteSheetInfo);
            sprite.position = position;
            sprite.origin = new Vector2(tex.Width / 2, tex.Height / 2);
            sprite.scale = 1f;

            setText(" ", Color.Red);

            statusText = new TextItem(World.fontManager["InfoFont"], " ");
            statusText.scale = 1.5f;
            statusText.position = position + new Vector2(-25, -25);
            if (team == 0)
            {
                statusText.color = new Color(0, 1f, 0); //Color.Green;
            }
            else if (team == 1)
            {
                statusText.color = new Color(1f, 0, 1f);// Color.Purple;
            }

            movePath = new List<Vector3>();
            moveTime = TimeSpan.MinValue;

            healthBacker = new Sprite(Game1.world.textureConverter.GenRectangle(60, 30, new Color(Color.Black, .75f)));
            healthBacker.position = statusText.position + new Vector2(22, -2);

            //Console.WriteLine("Character X:" + position.X + " Y:" + position.Y);
        }

        public void recalcPos()
        {
            int width = 11;// Game1.world.level.grid.width + 1;
            int depth = 11;// Game1.world.level.grid.depth + 1;
            int height = 6;// Game1.world.level.grid.height + 1;
            float scale = (100f - width / 2 - depth / 2 - height / 2 + gridPos.Z / 2 + gridPos.Y / 2 + gridPos.X / 2) / (100f - width / 2 - depth / 2 - height / 2);
            sprite.scale = scale;
            statusText.scale = 1.5f * scale;


            this.position = origin + Game1.world.twoDToIso(new Point((int)(gridPos.X * cubeWidth * scale), (int)(gridPos.Y * cubeHeight * scale * .65f))).ToVector2();
            this.position.Y -= gridPos.Z * cubeHeight * scale * .65F + nwTex.Height * scale / 2;

            sprite.position = position;

            //text.Text = DrawOrder().ToString();
            text.position = position + new Vector2(0, -60);

            statusText.position = position + new Vector2(-25, -25);

            healthBacker.position = statusText.position + new Vector2(22, -2);

            //darken();
        }

        public void Update(GameTime gameTime)
        {
            //recalcPos();
            if (textTime < gameTime.TotalGameTime)
            {
                setText(" ", Color.Red);
            }

            if (movePath.Count > 0)
            {
                if (moveTime == TimeSpan.MinValue)
                {
                    moveTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 1);
                }
                else if (moveTime < gameTime.TotalGameTime)
                {
                    Vector3 newPos = movePath.ToArray()[0];
                    if (newPos != gridPos)
                    {
                        Rotate(newPos);
                    }
                    gridPos = newPos;
                    movePath.RemoveAt(0);
                    recalcPos();
                    moveTime = TimeSpan.MinValue;
                }
                
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
            healthBacker.Draw(spriteBatch);
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
            healthBacker.alpha = alpha * .75f;
            statusText.alpha = alpha;
            text.alpha = alpha;
        }

        public void onCharacterMoved(Level level)
        {
            Character character = level.grid.characters.list[0];
            if (DrawOrder() > character.DrawOrder() && Vector2.Distance(position, character.position) < 10)
            {
                SetAlpha(.25f);
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
            SetAlpha(1f);
            if (c.gridPos != gridPos && DrawOrder() > c.DrawOrder() && Vector2.Distance(position, c.position) < 60)// && gridPos.Z > c.gridPos.Z)
            {
                SetAlpha(.5f);
            }
        }

        public bool isAdjacent(Vector3 v, int zTolerance=1)
        {
            return Game1.world.linearCubeDist(gridPos, v) == 1 && Math.Abs(gridPos.Z - v.Z) <= zTolerance;
        }

        public int[] calculateAttack(Character character)
        {
            bool canAttack;

            canAttack = character != this;

            if (job != EJob.Hunter)
            {
                canAttack = canAttack && character.isAdjacent(gridPos);
            }
            else
            {
                canAttack = canAttack && Vector3.Distance(character.gridPos, gridPos) <= weaponRange;// Game1.world.cubeDist(gridPos, character.gridPos) <= weaponRange;
            }

            if (canAttack)
            {
                int angleModifier = 0;
                angleModifier = getAngleModifier(character.direction);

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
                character.setText("-" + damage + "HP", Color.Red);

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
                setText("Miss!", Color.Green);
                return null;
            }
        }

        public bool inMoveRange(Vector3 pos, Level level)
        {
            return Game1.world.linearCubeDist(gridPos, pos) < move &&
                    level.grid.TopExposed(pos) &&
                    Math.Abs(pos.Z - gridPos.Z) <= jump;
        }

        public void Move(Vector3 pos, Level level)
        {
            int i = 0;
            foreach (Vector3 vect in level.validMoveSpaces)
            {
                if (vect == pos)
                {
                    foreach(Vector3 v in level.validMovePaths.ToArray()[i])
                    {
                        movePath.Add(v);
                    }
                    movePath.Add(pos);
                    break;
                }
                i++;
            }
            /*gridPos = pos;
            recalcPos();*/
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
                character.setText("-" + damage + "HP", Color.Red);

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
                character.setText("Miss!", Color.Green);
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
            character.setText("+" + health + "HP", Color.Red);
        }

        public int CalculateThiefSpecial(Character character)
        {
            int angleModifier = 0;
            angleModifier = getAngleModifier(character.direction);

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
                character.setText("Skipped!", Color.OrangeRed);
                return character;
            }
            else
            {
                Console.WriteLine("Miss!");
                character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                character.setText("Miss!", Color.Green);
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
            setText("-" + cost + "MP", Color.Blue);
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
            character.setText("+" + defenseBoost + "WD", Color.Yellow);
        }

        public void Rotate(Vector3 pos)
        {
            float x = gridPos.X - pos.X;
            float y = gridPos.Y - pos.Y;

            if (Math.Abs(x) > Math.Abs(y))
            {
                if (x > 0)
                {
                    direction = EDirection.Northwest;
                    sprite.tex = nwTex;
                }
                else
                {
                    direction = EDirection.Southeast;
                    sprite.tex = seTex;
                }
            }
            else
            {
                if (y > 0)
                {
                    direction = EDirection.Northeast;
                    sprite.tex = neTex;
                }
                else
                {
                    direction = EDirection.Southwest;
                    sprite.tex = swTex;
                }
            }
        }

        public void Rotate(EDirection dir)
        {
            direction = dir;
            if (dir == EDirection.Northwest)
            {
                sprite.tex = nwTex;
            }
            else if (dir == EDirection.Southeast)
            {
                sprite.tex = seTex;
            }
            else if (dir == EDirection.Northeast)
            {
                sprite.tex = neTex;
            }
            else if (dir == EDirection.Southwest)
            {
                sprite.tex = swTex;
            }
        }

        public bool InMagicRange(Vector3 pos)
        {
            //return Game1.world.cubeDist(pos, gridPos) <= magicRange;
            return Vector3.Distance(pos, gridPos) <= magicRange;
        }

        public bool InWeaponRange(Vector3 pos)
        {
            //return Game1.world.cubeDist(pos, gridPos) <= weaponRange;
            return Vector3.Distance(pos, gridPos) <= weaponRange;
        }

        public List<Cube> GetMovePositions(Level level)
        {
            List<Cube> results = new List<Cube>();
            foreach (Cube cube in level.grid.cubes)
            {
                if (((level.validMoveSpaces.Contains(cube.gridPos) || cube.gridPos == gridPos) && level.grid.TopExposed(cube.gridPos)))
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
                        if (character.team != team && InWeaponRange(character.gridPos))
                        {
                            moves.Add(new Move(0, gridPos, 1, character.gridPos)); // Don't move, Attack character
                        }
                    }
                    if (CanCast(8) && job != EJob.Hunter)
                    {
                        foreach (Cube cu in level.grid.cubes)
                        {
                            if (InMagicRange(cu.gridPos) && level.grid.TopExposed(cu.gridPos))
                            {                                
                                if (job != EJob.Mage || job != EJob.Healer)
                                {
                                    Character target = level.grid.CharacterAtPos(cu.gridPos);
                                    if (target != null && (target.team != team || job == EJob.Soldier))
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
                        if (character.team != team && Game1.world.cubeDist(character.gridPos, cube.gridPos) <= weaponRange)
                        {
                            moves.Add(new Move(1, cube.gridPos, 1, character.gridPos)); // Move first, Attack character
                        }
                    }
                    if (CanCast(8) && job != EJob.Hunter)
                    {
                        foreach (Cube cu in level.grid.cubes)
                        {
                            if (Game1.world.cubeDist(cu.gridPos, cube.gridPos) <= magicRange && level.grid.TopExposed(cu.gridPos))
                            {
                                if (job != EJob.Mage || job != EJob.Healer)
                                {
                                    Character target = level.grid.CharacterAtPos(cu.gridPos);
                                    if ((target != null && target != this && (target.team != team || job == EJob.Soldier)) || (job == EJob.Soldier && cu.gridPos == cube.gridPos))
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
                        if (character.team != team && InWeaponRange(character.gridPos))
                        {
                            moves.Add(new Move(2, cube.gridPos, 1, character.gridPos)); // Move after, Attack character
                        }
                    }
                    if (CanCast(8) && job != EJob.Hunter)
                    {
                        foreach (Cube cu in level.grid.cubes)
                        {
                            if (InMagicRange(cu.gridPos) && level.grid.TopExposed(cu.gridPos))
                            {
                               if (job != EJob.Mage || job != EJob.Healer)
                                {
                                    Character target = level.grid.CharacterAtPos(cu.gridPos);
                                    if (target != null && (target.team != team || job == EJob.Soldier))
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

        int getAngleModifier(EDirection dir)
        {
            int val = Math.Abs(direction - dir);
            if (val == 3)
            {
                val = 1;
            }
            if (val == 2)
            {
                val = 0;
            }
            if (val == 0)
            {
                val = 2;
            }
            return val;
        }

        public void setText(String t, Color c)
        {
            text = new TextItem(World.fontManager["ScoreFont"], t);
            text.position = position + new Vector2(0, -80);
            text.color = c;
            text.scale = 1.5f;
        }
    }
}
