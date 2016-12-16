using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        public Grid grid;

        public Texture2D oneByOne;

        public World(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            rooms = new PermanantStates<Room>();
            rooms.AddState("game", new Room(graphics));
        }

        public void Update(GameTime gameTime)
        {
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
                    tempPt.Y = (int)(y + Math.Sqrt((height / 2.0) * (height / 2.0)) - 1);
                }
                else
                {
                    tempPt.Y = y;
                }
            }
            return (tempPt);
        }
    }
}
