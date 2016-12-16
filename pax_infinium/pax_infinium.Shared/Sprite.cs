using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace pax_infinium
{
    public class Sprite : SpriteBase
    {
        public Texture2D tex;
        public Rectangle drawRect;
        public Matrix spriteTransform;
        public Animations animations;
        private bool isAnimated;

        public Sprite(Texture2D loadedTex)
        {
            tex = loadedTex;
            drawRect = new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y), 0, 0);
            rectangle = new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y), tex.Width, tex.Height);
            origin = new Vector2(tex.Width / 2, tex.Height / 2);
            isAnimated = false;
        }


        public Sprite(GraphicsDeviceManager graphics)
        {
            tex = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            tex.SetData(new[] { color });
            rectangle = new Rectangle((int)position.X, (int)position.Y, tex.Width, tex.Height);
            drawRect = new Rectangle((int)position.X, (int)position.Y, 0, 0);
            origin = new Vector2(tex.Width / 2, tex.Height / 2);
            isAnimated = false;
        }

        public Sprite(Texture2D baseTex, GraphicsDeviceManager graphics, SpriteSheetInfo spriteSheetInfo) : this(graphics, spriteSheetInfo)
        {
            tex = baseTex;
        }

        public Sprite(GraphicsDeviceManager graphics, SpriteSheetInfo spriteSheetInfo)
        {
            isAnimated = true;
            drawRect = new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y), 0, 0);
            animations = new Animations(spriteSheetInfo);
            tex = new Texture2D(graphics.GraphicsDevice, animations.spriteSheetInfo.FrameWidth, animations.spriteSheetInfo.FrameHeight);
            if (!string.IsNullOrEmpty(animations.CurrentAnimationName))
            {
                animations.currentSpriteSheet = animations.spriteSheets.First().Value;
            }
            rectangle = new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y), tex.Width, tex.Height);
            origin = new Vector2(tex.Width / 2, tex.Height / 2);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (isAnimated)
            {
                UpdateAnimation(gameTime);
            }
            position += velocity;
            drawRect.X = (int)position.X;
            drawRect.Y = (int)position.Y;
            rectangle = new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y), tex.Width, tex.Height);
            spriteTransform = Matrix.CreateTranslation(new Vector3(-origin, 0.0f)) *
                Matrix.CreateScale(scale) * Matrix.CreateRotationZ(rotation) *
                Matrix.CreateTranslation(new Vector3(position, 0.0f));
            rectangle = CalculateBoundingRectangle(new Rectangle(0, 0, tex.Width, tex.Height), spriteTransform);
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            if (animations.active)
            {
                animations.elapsedTime += gameTime.ElapsedGameTime.Ticks;
                if (animations.elapsedTime > animations.currentSpriteSheet.frameTime)
                {
                    long framesMoved = animations.elapsedTime / animations.currentSpriteSheet.frameTime;
                    for (int i = 0; i < framesMoved; i++)
                    {
                        animations.currentFrame++;
                        if (animations.currentFrame == animations.currentSpriteSheet.frameCount)
                        {
                            if (!animations.currentSpriteSheet.loop)
                            {
                                animations.active = false;
                                animations.elapsedTime = 0;
                                animations.currentFrame = 0;
                                break;
                            }
                            else
                            {
                                animations.currentFrame = 0;
                            }
                        }
                        if (animations.currentFrame >= 0 && animations.currentFrame < animations.currentSpriteSheet.frameActions.Count)
                        {
                            foreach (Action action in animations.currentSpriteSheet.frameActions[animations.currentFrame])
                            {
                                action.Invoke();
                            }
                        }
                    }
                }
                animations.elapsedTime = animations.elapsedTime % animations.currentSpriteSheet.frameTime;
            }
            if (animations.CurrentAnimationName != null)
            {
                UpdateTexture();
            }
        }

        private void UpdateTexture()
        {
            Rectangle oldSourceRect = animations.sourceRect;
            if (animations.currentSpriteSheet.direction == SpriteSheet.Directions.LeftToRight)
            {
                animations.sourceRect = new Rectangle(
                    (animations.currentFrame % animations.currentSpriteSheet.columns) * animations.spriteSheetInfo.FrameWidth,
                    (animations.currentFrame / animations.currentSpriteSheet.columns) * animations.spriteSheetInfo.FrameHeight,
                    animations.spriteSheetInfo.FrameWidth,
                    animations.spriteSheetInfo.FrameHeight);
            }
            else if (animations.currentSpriteSheet.direction == SpriteSheet.Directions.TopToBottom)
            {
                animations.sourceRect = new Rectangle(
                    (animations.currentFrame / animations.currentSpriteSheet.rows) * animations.spriteSheetInfo.FrameWidth,
                    (animations.currentFrame % animations.currentSpriteSheet.rows) * animations.spriteSheetInfo.FrameHeight,
                    animations.spriteSheetInfo.FrameWidth,
                    animations.spriteSheetInfo.FrameHeight);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isAnimated && animations.active)
            {
                DrawAnimation(spriteBatch);
            }
            else
            {
                spriteBatch.Draw(tex, position, null, color * alpha, MathHelper.ToRadians(rotation), origin, scale, SpriteEffects.None, 0);
            }
        }

        public void DrawAnimation(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(animations.CurrentAnimation.tex, position, animations.sourceRect, color * alpha, MathHelper.ToRadians(rotation), origin, scale, SpriteEffects.None, 0);
        }

        public void DrawRect(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, drawRect, null, color * alpha, MathHelper.ToRadians(rotation), origin, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Used for pixel perfect collision with scaled/rotated sprites.
        /// </summary>
        /// <param name="rectangle">The current bounding rectangle</param>
        /// <param name="transform">The current sprite transform matrix</param>
        /// <returns>Returns a new bounding rectangle</returns>
        private Rectangle CalculateBoundingRectangle(Rectangle rectangle, Matrix transform)
        {
            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int)Math.Round(min.X), (int)Math.Round(min.Y),
                                 (int)Math.Round(max.X - min.X), (int)Math.Round(max.Y - min.Y));
        }
    }
}
