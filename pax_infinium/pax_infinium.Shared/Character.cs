using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using pax_infinium.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
        public TimeSpan postMoveWaitTime;

        public int accuracyMod;

        public Sprite hitSprite;
        public TimeSpan hitTime;
        public EHitIcon hitType;

        public List<Object[]> timedEvents;
        public List<EHitIcon> hitList;

        public Move currentMove;
        public int moveCounter;

        public int[] genes;
        public EPersonality personality;
        public float personalityScore;


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
            statusText.position = position + new Vector2(0, -25);
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

            postMoveWaitTime = TimeSpan.MinValue;

            healthBacker = new Sprite(Game1.world.textureConverter.GenRectangle(60, 30, new Color(Color.Black, .75f)));
            healthBacker.position = statusText.position + new Vector2(-2, -2);

            accuracyMod = 0;

            hitTime = TimeSpan.MinValue;
            hitType = EHitIcon.None;

            timedEvents = new List<Object[]>();
            hitList = new List<EHitIcon>();

            currentMove = null;
            moveCounter = 0;
            genes = new int[7];
            personalityScore = 0;
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

            statusText.position = position + new Vector2(0, -25);

            healthBacker.position = statusText.position + new Vector2(-2, -2);

            if (hitSprite != null)
            {
                hitSprite.position = position;
            }

            //darken();
        }

        public void Update(GameTime gameTime)
        {
            handleTimedEvents(gameTime);
            handleMoveEvent(gameTime);
        }

        public void DoMove(GameTime gameTime)
        {
            Object[] obj;
            switch (currentMove.noneMoveBeforeMoveAfter)
            {
                case 0: // Don't Move
                    switch (moveCounter)
                    {
                        case 0: // None
                            currentMove.NothingAttackSpecial(this, Game1.world.level, gameTime);
                            Object[] objA = { gameTime.TotalGameTime + new TimeSpan(0, 0, 2), "DoMove" };
                            timedEvents.Add(objA);
                            break;
                        case 1: // Rotate
                            //Rotate(currentMove.rotDir);
                            //RotateBest(Game1.world.level);
                            Object[] objB = { gameTime.TotalGameTime + new TimeSpan(0, 0, 2), "DoMove" };
                            timedEvents.Add(objB);
                            break;
                        case 2: // EndTurn
                            currentMove.EndTurn(Game1.world.level, gameTime);
                            break;
                    }
                    break;
                case 1: // Move Before Action
                    switch (moveCounter)
                    {
                        case 0: // Move
                            Move(currentMove.movePos, Game1.world.level);
                            break;
                        case 1: // NothingAttackSpecial
                            currentMove.NothingAttackSpecial(this, Game1.world.level, gameTime);
                            Object[] objC = { gameTime.TotalGameTime + new TimeSpan(0, 0, 2), "DoMove" };
                            timedEvents.Add(objC);
                            break;
                        case 2: // Rotate
                            //Rotate(currentMove.rotDir);
                            //RotateBest(Game1.world.level);
                            Object[] objD = { gameTime.TotalGameTime + new TimeSpan(0, 0, 2), "DoMove" };
                            timedEvents.Add(objD);
                            break;
                        case 3: // EndTurn
                            currentMove.EndTurn(Game1.world.level, gameTime);
                            break;
                    }
                    break;
                case 2: // Move After Action
                    switch (moveCounter)
                    {
                        case 0: // NothingAttackSpecial
                            currentMove.NothingAttackSpecial(this, Game1.world.level, gameTime);
                            Object[] objE = { gameTime.TotalGameTime + new TimeSpan(0, 0, 2), "DoMove" };
                            timedEvents.Add(objE);
                            break;
                        case 1: // Move
                            Move(currentMove.movePos, Game1.world.level);
                            break;
                        case 2: // Rotate
                            //Rotate(currentMove.rotDir);
                            //RotateBest(Game1.world.level);
                            Object[] objF = { gameTime.TotalGameTime + new TimeSpan(0, 0, 2), "DoMove" };
                            timedEvents.Add(objF);
                            break;
                        case 3: // EndTurn
                            currentMove.EndTurn(Game1.world.level, gameTime);
                            break;
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            moveCounter++;
        }

        public void handleTimedEvents(GameTime gameTime)
        {
            if (timedEvents.Count > 0)
            {
                Object[][] tempList = new Object[timedEvents.Count][];
                timedEvents.CopyTo(tempList);
                foreach (Object[] entry in tempList)
                {
                    TimeSpan t = (TimeSpan)entry[0];
                    if (t < gameTime.TotalGameTime)
                    {
                        String code = (String)entry[1];
                        Type thisType = this.GetType();
                        MethodInfo theMethod = thisType.GetMethod(code);
                        theMethod.Invoke(this, new object[] { gameTime });
                        timedEvents.Remove(entry);
                    }
                }
            }
        }

        /*public void handlePostMoveWaitTime(GameTime gameTime) // Add Rotation pause handling -------------------------------------------------------TODO-----------------------------------------
        {
            Game1.world.currentMove.PostMove(Game1.world.level, gameTime);
        }*/

        public void clearHitType(GameTime gameTime)
        {
            hitType = EHitIcon.None;
            if (hitList.Count > 0)
            {
                EHitIcon temp = hitList.ToArray()[0];
                UpdateHitIcon(temp, gameTime);
                hitList.Remove(temp);
            }
        }

        public void clearText(GameTime gameTime)
        {
            setText(" ", Color.Red);
        }

        public void handleMoveEvent(GameTime gameTime)
        {
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
                        Rotate(newPos, true);
                    }
                    gridPos = newPos;
                    if (movePath.Count == 1 && team == 0)
                    {
                        Object[] obj = { gameTime.TotalGameTime + new TimeSpan(0, 0, 2), "DoMove" };
                        timedEvents.Add(obj);
                    }
                    movePath.RemoveAt(0);
                    recalcPos();
                    Game1.world.level.grid.onCharacterMoved(Game1.world.level);
                    moveTime = TimeSpan.MinValue;
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (hitType == EHitIcon.Magic)
            {
                hitSprite.Draw(spriteBatch);
            }
            sprite.Draw(spriteBatch);
            //statusText.Text = health.ToString();
            if (health.ToString() != statusText.Text)
            {
                statusText = new TextItem(World.fontManager["InfoFont"], health.ToString());
                statusText.scale = 1.5f;
                statusText.position = position + new Vector2(0, -25);
                if (team == 0)
                {
                    statusText.color = new Color(0, 1f, 0); //Color.Green;
                }
                else if (team == 1)
                {
                    statusText.color = new Color(1f, 0, 1f);// Color.Purple;
                }
                healthBacker = new Sprite(Game1.world.textureConverter.GenRectangle((int)(statusText.rectangle.Width * statusText.scale) + 10, 30, new Color(Color.Black, .75f)));
                healthBacker.position = statusText.position + new Vector2(-2, -2);
            }
            healthBacker.Draw(spriteBatch);
            statusText.Draw(spriteBatch);
            if (text.Text != " ")
            {
                text.Draw(spriteBatch);
            }
            if (hitType != EHitIcon.None && hitType != EHitIcon.Magic)
            {
                hitSprite.Draw(spriteBatch);
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
            if (hitSprite != null)
            {
                hitSprite.alpha = alpha;
            }
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

                int chance = 90 - character.evasion + angleModifier * 5 + accuracyMod;
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
                if (team == 0 && character.team == 1)
                {
                    Game1.world.level.damageDealtByAI += damage;
                }
                Console.WriteLine("Hit! " + character.name + " takes " + damage + " damage!");
                character.health -= damage;
                //character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                Object[] obj = { gameTime.TotalGameTime + new TimeSpan(0, 0, 5), "clearText" };
                character.timedEvents.Add(obj);
                character.setText("-" + damage + "HP", Color.Red);

                if (job == EJob.Hunter)
                {
                    character.UpdateHitIcon(EHitIcon.Arrow, gameTime);
                }
                else
                {
                    character.UpdateHitIcon(EHitIcon.Slash, gameTime);
                }

                Game1.world.level.recalcTeamHealthBar();

                if (character.health <= 0)
                {
                    Console.WriteLine(character.name + " has died!");
                    return character;
                }
                else
                {
                    Console.WriteLine(character.name + " health: " + character.health);
                    Game1.world.level.recalcStatusBars();
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Miss!");
                //character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                Object[] obj = { gameTime.TotalGameTime + new TimeSpan(0, 0, 5), "clearText" };
                character.timedEvents.Add(obj);
                character.setText("Miss!", Color.Green);
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

        public void Move(Vector3 pos, bool recalc)
        {
            gridPos = pos;
            if (recalc)
            {
                recalcPos();
            }
        }

        public int[] calculateMageSpecial(Character character)
        {
            int chance = 100 - character.evasion + accuracyMod;
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
                if (team == 0 && character.team == 1)
                {
                    Game1.world.level.damageDealtByAI += damage;
                }
                Console.WriteLine("Hit! " + character.name + " takes " + damage + " damage!");
                character.health -= damage;
                //character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                Object[] obj = { gameTime.TotalGameTime + new TimeSpan(0, 0, 5), "clearText" };
                character.timedEvents.Add(obj);
                character.setText("-" + damage + "HP", Color.Red);

                character.UpdateHitIcon(EHitIcon.Lightning, gameTime);

                Game1.world.level.recalcTeamHealthBar();

                if (character.health <= 0)
                {
                    Console.WriteLine(character.name + " has died!");
                    return character;
                }
                else
                {
                    Console.WriteLine(character.name + " health: " + character.health);
                    Game1.world.level.recalcStatusBars();
                }
            }
            else
            {
                Console.WriteLine("Miss!");
                //character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                Object[] obj = { gameTime.TotalGameTime + new TimeSpan(0, 0, 5), "clearText" };
                character.timedEvents.Add(obj);
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
            if (team == 0 && character.team == 0)
            {
                Game1.world.level.damageDealtByAI += health;
            }
            Console.WriteLine(character.name + " heals " + health + " points!");
            character.health += health;
            //character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
            Object[] obj = { gameTime.TotalGameTime + new TimeSpan(0, 0, 5), "clearText" };
            character.timedEvents.Add(obj);
            character.setText("+" + health + "HP", Color.Red);

            Game1.world.level.recalcTeamHealthBar();
            Game1.world.level.recalcStatusBars();

            character.UpdateHitIcon(EHitIcon.Heal, gameTime);
        }

        public int CalculateThiefSpecial(Character character)
        {
            int angleModifier = 0;
            angleModifier = getAngleModifier(character.direction);

            int chance = 45 - character.evasion + angleModifier * 5 + accuracyMod;

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
                //character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                Object[] obj = { gameTime.TotalGameTime + new TimeSpan(0, 0, 5), "clearText" };
                character.timedEvents.Add(obj);
                character.setText("Skipped!", Color.OrangeRed);

                character.UpdateHitIcon(EHitIcon.Skip, gameTime);

                return character;
            }
            else
            {
                Console.WriteLine("Miss!");
                //character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                Object[] obj = { gameTime.TotalGameTime + new TimeSpan(0, 0, 5), "clearText" };
                character.timedEvents.Add(obj);
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
            //textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
            Object[] obj = { gameTime.TotalGameTime + new TimeSpan(0, 0, 5), "clearText" };
            timedEvents.Add(obj);
            setText("-" + cost + "MP", Color.Blue);

            UpdateHitIcon(EHitIcon.Magic, gameTime);

            Game1.world.level.recalcStatusBars();
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
            //character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
            Object[] obj = { gameTime.TotalGameTime + new TimeSpan(0, 0, 5), "clearText" };
            character.timedEvents.Add(obj);
            character.setText("+" + defenseBoost + "WD", Color.Yellow);

            character.UpdateHitIcon(EHitIcon.Shield, gameTime);
        }

        public int CalculateHunterSpecial(Character character)
        {
            int chance = 75 - character.evasion + accuracyMod;

            return chance;
        }

        public void HunterSpecial(Character character)
        {
            int chance = CalculateHunterSpecial(character);
            if (chance >= World.Random.Next(1, 101))
            {
                int accuracyDrop = 5;
                character.accuracyMod -= accuracyDrop;
            }
        }

        public void HunterSpecial(Character character, GameTime gameTime)
        {
            int chance = CalculateHunterSpecial(character);
            Console.WriteLine("Chance to hit: " + chance + "%");
            if (chance >= World.Random.Next(1, 101))
            {
                int accuracyDrop = 5;
                Console.WriteLine(character.name + " loses " + accuracyDrop + " accuracy!");
                character.accuracyMod -= accuracyDrop;
                //character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                Object[] obj = { gameTime.TotalGameTime + new TimeSpan(0, 0, 5), "clearText" };
                character.timedEvents.Add(obj);
                character.setText("-" + accuracyDrop + "ACC", Color.Orange);

                character.UpdateHitIcon(EHitIcon.AccuracyDown, gameTime);
            }
            else
            {
                Console.WriteLine("Miss!");
                //character.textTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 5);
                Object[] obj = { gameTime.TotalGameTime + new TimeSpan(0, 0, 5), "clearText" };
                character.timedEvents.Add(obj);
                character.setText("Miss!", Color.Green);
            }
        }

        public void Rotate(Vector3 pos, bool changeTex)
        {
            float x = gridPos.X - pos.X;
            float y = gridPos.Y - pos.Y;

            if (Math.Abs(x) > Math.Abs(y))
            {
                if (x > 0)
                {
                    direction = EDirection.Northwest;
                }
                else
                {
                    direction = EDirection.Southeast;
                }
            }
            else
            {
                if (y > 0)
                {
                    direction = EDirection.Northeast;
                }
                else
                {
                    direction = EDirection.Southwest;
                }
            }

            if (changeTex)
            {
                Rotate(direction);
            }
        }

        public void Rotate(EDirection dir, bool changeTex=true)
        {
            direction = dir;
            if (changeTex)
            {
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
        }

        public void RotateBest(Level level, bool changeTex = true)
        {
            Cube nw = null;
            int nwScore = 0;
            Cube se = null;
            int seScore = 0;
            Cube ne = null;
            int neScore = 0;
            Cube sw = null;
            int swScore = 0;
            int topHeight = (int) Math.Min(level.grid.height - 1, gridPos.Z + 1);
            int bottomHeight = (int)Math.Max(0, gridPos.Z - 1);
            for(; topHeight >= 0; topHeight++)
            {
                if (nw == null) nw = level.grid.getCube((int)gridPos.X - 1, (int)gridPos.Y, topHeight);
                if (se == null) se = level.grid.getCube((int)gridPos.X + 1, (int)gridPos.Y, topHeight);
                if (ne == null) ne = level.grid.getCube((int)gridPos.X, (int)gridPos.Y - 1, topHeight);
                if (sw == null) sw = level.grid.getCube((int)gridPos.X - 1, (int)gridPos.Y + 1, topHeight);
            }

            nwScore += GetCubeScore(level, nw);
            seScore += GetCubeScore(level, se);
            neScore += GetCubeScore(level, ne);
            swScore += GetCubeScore(level, sw);
            int maxScore = Math.Max(Math.Max(nwScore, seScore), Math.Max(neScore, swScore));
            /*if (maxScore != 2) // if there is no enemy adjacent
            {
                Character closestEnemy = null;
                int closestEnemyDist = 0;
                foreach(Character c in level.grid.characters.list)
                {
                    if (c.team != team)
                    {
                        if (closestEnemy == null)
                        {
                            closestEnemy = c;
                            closestEnemyDist = Game1.world.cubeDist(gridPos, c.gridPos);
                        }
                        else
                        {
                            int tempEnemyDist = Game1.world.cubeDist(gridPos, c.gridPos);
                            if (tempEnemyDist < closestEnemyDist)
                            {
                                closestEnemy = c;
                                closestEnemyDist = tempEnemyDist;
                            }
                        }
                    }
                }

                Vector3 directionToClosestEnemy = gridPos - closestEnemy.gridPos;
                int xDiff = (int) directionToClosestEnemy.X;
                int yDiff = (int) directionToClosestEnemy.Y;

                if (Math.Abs(xDiff) > Math.Abs(yDiff))
                {
                    if (xDiff > 0)
                    {
                        nwScore += 1;
                    }
                    else
                    {
                        seScore += 1;
                    }
                }
                else
                {
                    if (yDiff > 0)
                    {
                        neScore += 1;
                    }
                    else
                    {
                        swScore += 1;
                    }

                }
            }

            maxScore = (int)Math.Max(Math.Max(nwScore, seScore), Math.Max(neScore, swScore));*/

            if (nwScore == maxScore) {
                Rotate(EDirection.Northwest, changeTex);
            }
            else if (neScore == maxScore) {
                Rotate(EDirection.Northeast, changeTex);
            }
            else if (swScore == maxScore) {
                Rotate(EDirection.Southwest, changeTex);
            }
            else {
                Rotate(EDirection.Southeast, changeTex);
            }

        }

        public int GetCubeScore(Level level, Cube c)
        {
            int result = 0;
            if (c != null)
            {
                if (level.grid.getCube((int)c.gridPos.X, (int)c.gridPos.Y, (int)c.gridPos.Z + 1) == null)
                {
                    result += 1;
                    Character temp = level.grid.CharacterAtPos(c.gridPos);
                    if (temp != null)
                    {
                        if (temp.team == team)
                        {
                            result -= 1;
                        }
                        else
                        {
                            result += 1;
                        }
                    }
                }
            }
            return result;
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
                            moves.Add(new Move(0, gridPos, 1, character.gridPos, character.name)); // Don't move, Attack character
                        }
                    }
                    if (CanCast(8))
                    {
                        foreach (Cube cu in level.grid.cubes)
                        {
                            if (InMagicRange(cu.gridPos) && level.grid.TopExposed(cu.gridPos))
                            {
                                if (job != EJob.Mage && job != EJob.Healer)
                                {
                                    Character target = level.grid.CharacterAtPos(cu.gridPos);
                                    if (target != null && (target.team != team || job == EJob.Soldier))
                                    {
                                        moves.Add(new Move(0, gridPos, 2, cu.gridPos, target.name)); // Don't move, use special on character
                                    }
                                }
                                else
                                {
                                    foreach (Character c in level.grid.characters.list)
                                    {
                                        if (c.team == team)
                                        {
                                            if (job == EJob.Healer && (cu.isAdjacent(c.gridPos) || cu.gridPos == c.gridPos))
                                            {
                                                moves.Add(new Move(0, gridPos, 2, cu.gridPos, c.name)); // Don't move, use special at cube
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (job == EJob.Mage && (cu.isAdjacent(c.gridPos) || cu.gridPos == c.gridPos))
                                            {
                                                moves.Add(new Move(0, gridPos, 2, cu.gridPos, c.name)); // Don't move, use special at cube
                                                break;
                                            }
                                        }
                                    }                                    
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
                            moves.Add(new Move(1, cube.gridPos, 1, character.gridPos, character.name)); // Move first, Attack character

                        }
                    }
                    if (CanCast(8))
                    {
                        foreach (Cube cu in level.grid.cubes)
                        {
                            if (Vector3.Distance(cu.gridPos, cube.gridPos) <= magicRange && level.grid.TopExposed(cu.gridPos))
                            {
                                if (job != EJob.Mage && job != EJob.Healer)
                                {
                                    Character target = level.grid.CharacterAtPos(cu.gridPos);
                                    if (target != null && target != this && (target.team != team || job == EJob.Soldier))
                                    {
                                        moves.Add(new Move(1, cube.gridPos, 2, cu.gridPos, target.name)); // Move first, use special on character
                                    }
                                    else if (job == EJob.Soldier && cu.gridPos == cube.gridPos)
                                    {
                                        moves.Add(new Move(1, cube.gridPos, 2, cu.gridPos, name)); // Move first, use special on self
                                    }
                                }
                                else
                                {
                                    foreach (Character c in level.grid.characters.list)
                                    { 
                                        if (c.team == team)
                                        {
                                            if (job == EJob.Healer && (cu.isAdjacent(c.gridPos) || cu.gridPos == c.gridPos))
                                            {
                                                moves.Add(new Move(1, cube.gridPos, 2, cu.gridPos, c.name)); // Move first, use special at cube
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (job == EJob.Mage && (cu.isAdjacent(c.gridPos) || cu.gridPos == c.gridPos))
                                            {
                                                moves.Add(new Move(1, cube.gridPos, 2, cu.gridPos, c.name)); // Move first, use special at cube
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //move after
                    foreach (Character character in level.grid.characters.list)
                    {
                        if (character.team != team && InWeaponRange(character.gridPos))
                        {
                            moves.Add(new Move(2, cube.gridPos, 1, character.gridPos, character.name)); // Move after, Attack character
                        }
                    }
                    if (CanCast(8))
                    {
                        foreach (Cube cu in level.grid.cubes)
                        {
                            if (InMagicRange(cu.gridPos) && level.grid.TopExposed(cu.gridPos))
                            {
                                if (job != EJob.Mage && job != EJob.Healer)
                                {
                                    Character target = level.grid.CharacterAtPos(cu.gridPos);
                                    if (target != null && (target.team != team || job == EJob.Soldier))
                                    {
                                        moves.Add(new Move(2, cube.gridPos, 2, cu.gridPos, target.name)); // Move after, use special on character
                                    }
                                }
                                else
                                {
                                    foreach (Character c in level.grid.characters.list)
                                    {
                                        if (c.team == team)
                                        {
                                            if (job == EJob.Healer && (cu.isAdjacent(c.gridPos) || cu.gridPos == c.gridPos))
                                            {
                                                moves.Add(new Move(2, cube.gridPos, 2, cu.gridPos, c.name)); // Move after, use special at cube
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (job == EJob.Mage && (cu.isAdjacent(c.gridPos) || cu.gridPos == c.gridPos))
                                            {
                                                moves.Add(new Move(2, cube.gridPos, 2, cu.gridPos, c.name)); // Move after, use special at cube
                                                break;
                                            }
                                        }
                                    }
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
            if (words[words.Length - 2] == "clone")
            {                
                String number = words[words.Length - 1];
                int num = Int32.Parse(number);
                num++;
                for (int i = 0; i < words.Length - 2; i++)
                {
                    clone.name += words[i] + " ";
                }
                clone.name +=  num;
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
            if (val == 3 || val == 1) // Face to Side
            {
                val = 1;
            }
            if (val == 2) // Face to Face
            {
                val = 0;
            }
            if (val == 0) // Face to Back
            {
                val = 2;
            }
            return val;
        }

        /*
            Northeast = 0,

            Southeast = 1,

            Southwest = 2,

            Northwest = 3,

            Face to Side
            NE - NW = -3
            NE- SE = -1

            NW - NE = 3
            NW - SW = 1

            SW - NW = -1
            SW - SE = 1

            SE - SW = -1
            SE - NE = 1

            Face to Face
            NE - SW = -2
            NW - SE = 2
            SE - NW = -2
            SW - NE = 2

            Face to Back
            x - x = 0

        */

        public void setText(String t, Color c)
        {
            text = new TextItem(World.fontManager["ScoreFont"], t);
            text.position = position + new Vector2(0, -80);
            text.color = c;
            text.scale = 1.5f;
        }

        public void UpdateHitIcon(EHitIcon hT, GameTime gameTime)
        {
            if (hitType != EHitIcon.None)
            {
                hitList.Add(hT);
            }
            else
            {
                hitType = hT;
                //hitTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 2);
                Object[] obj = { gameTime.TotalGameTime + new TimeSpan(0, 0, 2), "clearHitType" };
                timedEvents.Add(obj);
                switch (hitType)
                {
                    case EHitIcon.Slash:
                        hitSprite = new Sprite(World.textureManager["Slash"]);
                        break;
                    case EHitIcon.Heal:
                        hitSprite = new Sprite(World.textureManager["Heal"]);
                        break;
                    case EHitIcon.AccuracyDown:
                        hitSprite = new Sprite(World.textureManager["AccuracyDown"]);
                        break;
                    case EHitIcon.Arrow:
                        hitSprite = new Sprite(World.textureManager["Arrow"]);
                        break;
                    case EHitIcon.Lightning:
                        hitSprite = new Sprite(World.textureManager["Lightning"]);
                        break;
                    case EHitIcon.Magic:
                        hitSprite = new Sprite(World.textureManager["Magic"]);
                        break;
                    case EHitIcon.Shield:
                        hitSprite = new Sprite(World.textureManager["Shield"]);
                        break;
                    case EHitIcon.Skip:
                        hitSprite = new Sprite(World.textureManager["Skip"]);
                        break;
                    default:
                        break;
                }
                if (hitSprite != null)
                {
                    hitSprite.position = position;
                }
            }
        }

        public void SaveGene()
        {
            String path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String filePath = path + @"\Genes";
            switch ((int)job)
            {
                case 0: // Soldier
                    filePath += @"\Soldier";
                    break;
                case 1: // Hunter
                    filePath += @"\Hunter";
                    break;
                case 2: // Black Mage
                    filePath += @"\Mage";
                    break;
                case 3: // White Mage/Healer
                    filePath += @"\Healer";
                    break;
                case 4: // Thief
                    filePath += @"\Thief";
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
            switch ((int)personality)
            {
                case 1:
                    filePath += @"\Aggressive.txt";
                    break;
                case 2:
                    filePath += @"\Defensive.txt";
                    break;
                default:
                    filePath += @"\Default.txt";
                    break;
            }
            Console.WriteLine(filePath);
            Console.WriteLine("pScore: " + personalityScore);
            StreamWriter sw = new StreamWriter(filePath);
            sw.WriteLine(genes[0] + " " + genes[1] + " " + genes[2] + " " + genes[3] +
                " " + genes[4] + " " + genes[5] + " " + genes[6] + " " + personalityScore);
            sw.Close();
            StreamReader sr = new StreamReader(filePath);
            String line = "";
            if ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
            else
            {
                Console.WriteLine("Nope");
            }
        }
    }
}
