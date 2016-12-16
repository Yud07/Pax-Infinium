using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace pax_infinium
{
    public class Camera
    {
        private Matrix transform;
        internal Matrix Transform
        {
            get
            {
                UpdateTransform();
                return transform;
            }
        }

        private Matrix inverseTransform;
        internal Matrix InverseTransform
        {
            get
            {
                return inverseTransform;
            }
        }

        public Vector2 Pan { get; private set; }
        public float Zoom { get; private set; }
        public float Rotation { get; private set; }
        public Viewport viewport;

        public enum CameraFocus
        {
            TopLeft,
            Center
        }

        private CameraFocus focus;
        public CameraFocus Focus
        {
            get
            {
                return focus;
            }
            set
            {
                focus = value;
            }
        }

        public Camera(Viewport viewport, CameraFocus focus)
        {
            this.viewport = viewport;
            Pan = Vector2.Zero;
            Zoom = 1.0f;
            Rotation = 0;
            Focus = focus;
        }

        private void UpdateTransform()
        {
            if (focus == CameraFocus.Center)
            {
                transform = Matrix.CreateTranslation(new Vector3(-Pan.X, -Pan.Y, 0)) *
                    Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation)) *
                    Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                    Matrix.CreateTranslation(new Vector3(viewport.Width * 0.5f, viewport.Height * 0.5f, 0));
                viewport.X = (int)Math.Round(Pan.X - viewport.Width / 2);
                viewport.Y = (int)Math.Round(Pan.Y - viewport.Height / 2);
            }
            else if (focus == CameraFocus.TopLeft)
            {
                transform = Matrix.CreateTranslation(new Vector3(-Pan.X, -Pan.Y, 0)) *
                    Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation)) *
                    Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));
                viewport.X = (int)Math.Round(Pan.X);
                viewport.Y = (int)Math.Round(Pan.Y);
            }
            inverseTransform = Matrix.Invert(transform);
        }

        public void Update()
        {
            UpdateTransform();
        }
    }
}
