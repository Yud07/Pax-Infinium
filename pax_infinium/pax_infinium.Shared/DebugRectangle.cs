using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace pax_infinium
{
    public class DebugRectangle
    {
        private Line top;
        private Line right;
        private Line bottom;
        private Line left;

        public DebugRectangle(GraphicsDeviceManager graphics)
        {
            top = new Line(graphics);
            right = new Line(graphics);
            bottom = new Line(graphics);
            left = new Line(graphics);
        }

        public void SetColor(Color color)
        {
            top.color = color;
            right.color = color;
            bottom.color = color;
            left.color = color;
        }

        public void UpdateRectangle(Rectangle rect)
        {
            top.point1 = new Vector2(rect.Left, rect.Top);
            top.point2 = new Vector2(rect.Right, rect.Top);
            right.point1 = new Vector2(rect.Right, rect.Top);
            right.point2 = new Vector2(rect.Right, rect.Bottom);
            bottom.point1 = new Vector2(rect.Right, rect.Bottom);
            bottom.point2 = new Vector2(rect.Left, rect.Bottom);
            left.point1 = new Vector2(rect.Left, rect.Bottom);
            left.point2 = new Vector2(rect.Left, rect.Top);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            top.Draw(spriteBatch);
            right.Draw(spriteBatch);
            bottom.Draw(spriteBatch);
            left.Draw(spriteBatch);
        }
    }
}
