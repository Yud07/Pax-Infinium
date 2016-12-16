using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace pax_infinium
{
    public struct SpriteSheetInfo
    {
        public int FrameWidth { get; private set; }
        public int FrameHeight { get; private set; }

        public SpriteSheetInfo(int frameWidth, int frameHeight)
        {
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
        }
    }

    public class SpriteSheet
    {
        public enum Directions
        {
            LeftToRight,
            TopToBottom
        }

        public Texture2D tex;
        public SpriteSheetInfo info;
        public int frameCount;
        public int columns;
        public int rows;
        public Directions direction;
        public long frameTime;
        public bool loop;
        internal List<List<Action>> frameActions;

        public SpriteSheet(Texture2D loadedTex,
            SpriteSheetInfo info,
            int frameCount,
            int columns,
            int rows,
            Directions direction,
            long frameTime,
            bool loop)
        {
            tex = loadedTex;
            this.info = info;
            this.frameCount = frameCount;
            this.columns = columns;
            this.rows = rows;
            this.direction = direction;
            this.frameTime = frameTime;
            this.loop = loop;
            frameActions = new List<List<Action>>();
            for (int i = 0; i < frameCount; i++)
            {
                frameActions.Add(new List<Action>());
            }
        }
    }
}
