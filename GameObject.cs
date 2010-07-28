using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SB = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace Typocalypse
{
    public abstract class GameObject: DrawableGameComponent
    {
        private Rectangle screenBox;

        public Vector2 Position { get; set; }

        /// <summary>
        /// Angle in radians
        /// </summary>
        public float Angle { get; set; }
        public float ZOrder { get; set; }

        public SpriteBatch SpriteBatch { get; set; }
        public abstract int Width { get; }
        public abstract int Height { get; }

        public float X
        {
            get
            {
                return Position.X;
            }
            set
            {
                Position = new Vector2(value,Position.Y);
            }
        }

        public float Y
        {
            get
            {
                return Position.Y;
            }
            set
            {
                Position = new Vector2(Position.X,value);
            }
        }

        protected Vector2 Center
        {
            get
            {
                return new Vector2(Width/2 - 3, Height/2);
            }
        }

        public Rectangle RelativeBounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }

        protected Rectangle ObjectBounds
        {
            get
            {
                return new Rectangle(0, 0, Width, Height);
            }
        }

        protected GameObject(Game game, Vector2 initialPosition, float initialAngle, float zOrder) : base(game)
        {
            Position = initialPosition;
            Angle = initialAngle;
            SpriteBatch = (SB) Game.Services.GetService(typeof (SB));
            ZOrder = zOrder;
            screenBox = new Rectangle(0,0,Game.GraphicsDevice.PresentationParameters.BackBufferWidth,Game.GraphicsDevice.PresentationParameters.BackBufferHeight);
        }

        protected bool IsInScreen()
        {
            return screenBox.Intersects(RelativeBounds);
        }
        
        protected Vector2 GetTranslatedTransform(int dir, float speed) // dir: 1 for forward, -1 for backwards
        {
            Matrix rotation = Matrix.CreateRotationZ(Angle);
            return new Vector2(dir * (rotation.M12 * speed), -dir * (rotation.M11 * speed));
        }

        public float GetRelativeAngle(Vector2 other)
        {
            float distX = other.X - X, distY = other.Y - Y;
            return -MathHelper.WrapAngle((float) (Math.Atan2(distX, distY) - Math.PI));
        }
    }
}
