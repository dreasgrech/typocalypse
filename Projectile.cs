using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Typocalypse
{
    class Projectile:GameObject
    {
        private Texture2D objectTexture;
        private float speed;

        public Projectile(Game game, Vector2 initialPosition, float initialAngle, float zOrder, string imageAssetName, float speed) : base(game, initialPosition, initialAngle, zOrder)
        {
            objectTexture = game.Content.Load<Texture2D>(imageAssetName);
            this.speed = speed;
        }

        public override int Width
        {
            get { return objectTexture.Width; }
        }

        public override int Height
        {
            get { return objectTexture.Height; }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(objectTexture, Position, ObjectBounds, Color.White, Angle, Center, 1f, SpriteEffects.None, ZOrder);
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Position += GetTranslatedTransform(1, speed);
            if (!IsInScreen())
            {
                Game.Components.Remove(this);
            }

            for (int i = 0; i < EnemyManager.enemies.Count; i++)
            {
                var enemy = EnemyManager.enemies[i];
                if (enemy.RelativeBounds.Intersects(RelativeBounds))
                {
                    enemy.Die();
                    Game.Components.Remove(this);
                }
            }
        }
    }
}
