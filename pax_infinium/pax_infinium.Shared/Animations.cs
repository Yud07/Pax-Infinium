using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace pax_infinium
{
    public class Animations
    {
        public SpriteSheetInfo spriteSheetInfo;
        internal Dictionary<string, SpriteSheet> spriteSheets;
        public bool active;
        public long elapsedTime;
        public int currentFrame;
        public Rectangle sourceRect;
        internal SpriteSheet currentSpriteSheet;
        private string currentAnimationName;
        public string CurrentAnimationName
        {
            get
            {
                return currentAnimationName;
            }
            set
            {
                if (value == null)
                {
                    currentAnimationName = null;
                    currentSpriteSheet = null;
                    active = false;
                }
                else
                {
                    if (spriteSheets.ContainsKey(value))
                    {
                        currentAnimationName = value;
                        currentSpriteSheet = spriteSheets[currentAnimationName];
                        ResetAnimation();
                    }
                    else
                    {
                        throw new Exception("That animation does not exist.");
                    }
                }
            }
        }

        public SpriteSheet CurrentAnimation
        {
            get
            {
                return currentSpriteSheet;
            }
        }

        public SpriteSheet this[string key]
        {
            set
            {
                if (spriteSheets.ContainsKey(key))
                {
                    spriteSheets[key] = value;
                }
                else
                {
                    spriteSheets.Add(key, value);
                }
            }
        }

        public Animations(SpriteSheetInfo spriteSheetInfo)
        {
            this.spriteSheetInfo = spriteSheetInfo;
            spriteSheets = new Dictionary<string, SpriteSheet>();
        }

        private void ResetAnimation()
        {
            active = true;
            if (spriteSheets.Count != 0)
            {
                spriteSheetInfo = CurrentAnimation.info;
            }
            sourceRect = new Rectangle(0, 0, spriteSheetInfo.FrameWidth, spriteSheetInfo.FrameHeight);
            elapsedTime = 0;
            currentFrame = 0;
        }

        public SpriteSheet AddSpriteSheet(Texture2D spriteSheet, int frameCount, long frameTime, bool loop)
        {
            return new SpriteSheet(spriteSheet,
                spriteSheetInfo,
                frameCount,
                frameCount,
                1,
                SpriteSheet.Directions.LeftToRight,
                TimeSpan.FromMilliseconds(frameTime).Ticks,
                loop);
        }

        public SpriteSheet AddSpriteSheet(Texture2D spriteSheet, int frameCount, int columns, int rows, SpriteSheet.Directions direction, long frameTime, bool loop)
        {
            return new SpriteSheet(spriteSheet,
                spriteSheetInfo,
                frameCount,
                columns,
                rows,
                direction,
                TimeSpan.FromMilliseconds(frameTime).Ticks,
                loop);
        }

        public SpriteSheet AddSpriteSheet(Texture2D spriteSheet, SpriteSheetInfo info, int frameCount, int columns, int rows, SpriteSheet.Directions direction, long frameTime, bool loop)
        {
            return new SpriteSheet(spriteSheet,
                info,
                frameCount,
                columns,
                rows,
                direction,
                TimeSpan.FromMilliseconds(frameTime).Ticks,
                loop);
        }

        public void SetFrameAction(string animation, int frameNumber, Action action)
        {
            if (spriteSheets.ContainsKey(animation))
            {
                spriteSheets[animation].frameActions[frameNumber].Add(action);
            }
            else
            {
                throw new Exception("That animation does not exist.");
            }
        }
    }
}
