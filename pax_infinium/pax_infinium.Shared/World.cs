using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MCTS.Interfaces;
using MCTS;
using System.Linq;

namespace pax_infinium
{
    public class World
    {
        public static Random Random { get; private set; }
        public static Color Red
        {
            get
            {
                return new Color(219, 107, 94);
            }
        }

        public static Color Yellow
        {
            get
            {
                return new Color(224, 227, 87);
            }
        }

        public static Color Green
        {
            get
            {
                return new Color(109, 221, 101);
            }
        }

        public static Color Blue
        {
            get
            {
                return new Color(75, 209, 239);
            }
        }

        public static Color Purple
        {
            get
            {
                return new Color(176, 93, 232);
            }
        }

        static World()
        {
            Random = new Random();
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static ContentManager<Texture2D> textureManager;
        public static ContentManager<SpriteFont> fontManager;

        public TextureConverter textureConverter;

        public PermanantStates<Room> rooms;
        public Level level;

        public Texture2D oneByOne;

        public int state;

        public bool triggerAIBool;

        public World(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            rooms = new PermanantStates<Room>();
            rooms.AddState("game", new Room(graphics));
            state = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (triggerAIBool && level.drewThoughtBubble)
            {
                triggerAI(level, gameTime);
            }
            rooms.CurrentState.Update(gameTime);
        }

        public void BeginDraw()
        {
            BeginDraw(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                null, null, null, null,
                rooms.CurrentState.cameras.CurrentState.Transform);
        }

        public void BeginDraw(SpriteSortMode spriteSortMode, BlendState blendState)
        {
            BeginDraw(spriteSortMode, blendState,
                null, null, null, null,
                rooms.CurrentState.cameras.CurrentState.Transform);
        }

        public void BeginDraw(SpriteSortMode sortMode, BlendState blendState,
            SamplerState samplerState, DepthStencilState depthStencilState,
            RasterizerState rasterizerState, Effect effect, Matrix transformMatrix)
        {
            spriteBatch.Begin(sortMode, blendState, samplerState,
                depthStencilState, rasterizerState, effect, transformMatrix);
        }

        public void Draw()
        {
            rooms.CurrentState.Draw(spriteBatch);
        }

        public void Draw(Action<SpriteBatch> drawMethod)
        {
            drawMethod.Invoke(spriteBatch);
        }

        public void EndDraw()
        {
            spriteBatch.End();
        }

        public Point isoTo2D(Point pt){
            Point tempPt = new Point(0, 0);
            tempPt.X = (2 * pt.Y + pt.X) / 2;
            tempPt.Y = (2 * pt.Y - pt.X) / 2;
            return(tempPt);
        }
        public Point twoDToIso(Point pt){
            Point tempPt = new Point(0,0);
            tempPt.X = (pt.X - pt.Y);
            tempPt.Y = (pt.X + pt.Y)/2;
            return(tempPt);
        }

        public Point twoDToIso2(Point pt, int height, int width, bool mirrored=false)
        {
            Point tempPt = new Point(0, 0);
            int y;
            if (mirrored)
            {
                tempPt.X = -pt.X;
            }
            else
            {
                tempPt.X = pt.X;
            }
            y = pt.X / 2 - pt.Y + height;
            if (pt.X != 0)
            {
                tempPt.Y = y;//(int)(y - .75 * Math.Sqrt((height / 2.0) * (height / 2.0))) + 9; // FIX THIS
            }
            else
            {
                if (mirrored)
                {
                    tempPt.Y = (int)(y + Math.Sqrt((height / 2.0) * (height / 2.0)));
                }
                else
                {
                    tempPt.Y = y;
                }
            }
            return (tempPt);
        }

        public int cubeDist(Vector3 a, Vector3 b)
        {
            return (int) Vector3.Distance(a, b);//(Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z));
        }

        public Point rotate(Point p, double angle, Point origin)
        {
            double s = Math.Sin(angle);
            double c = Math.Cos(angle);
            //Console.WriteLine("sin: " + s + " cos: " + c);

            // translate point back to origin:
            p.X -= origin.X;
            p.Y -= origin.Y;

            // rotate point
            float xnew = p.X * (float)c - p.Y * (float)s;
            float ynew = p.X * (float)s + p.Y * (float)c;

            // translate point back:
            return new Point((int)xnew + origin.X, (int)ynew + origin.Y);
        }

        public void triggerAI(Level level, GameTime gameTime)
        {
            Action<string> print = s => Console.WriteLine(s);
            print(level.ToString());
            IMove move = UCT.ComputeSingleThreadedUCT(level, 1000, true, print, 0.7F, 10);
            print(move.Name);
            level.DoMove(move, gameTime); // Add boolean so that this animates and only prints this move
            foreach(Character c in level.grid.characters.list)
            {
                c.recalcPos();
            }
            level.grid.onCharacterMoved(level);

            level.setupTurnOrderIcons();

            Grid grid = level.grid;
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
                    grid.rotate(true, level);
                }
                else if (distanceToWest == min)
                {
                    //Console.WriteLine("west");
                    grid.rotate(false, level);
                }
                else
                {
                    //Console.WriteLine("north");
                    grid.rotate(true, level);
                    grid.rotate(true, level);
                }
            }

            level.thoughtBubble.position = Vector2.Zero;
            level.drewThoughtBubble = false;
            triggerAIBool = false;

            if (player.team == 0)
            {
                level.thoughtBubble.position = player.position;
                level.thoughtBubble.position.Y -= 50;
                triggerAIBool = true;
                //Game1.world.triggerAI(level, gameTime);
            }
        }

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}
