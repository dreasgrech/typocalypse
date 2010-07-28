using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Typocalypse
{
    public class Enemy:GameObject
    {
        private readonly Player player;
        private readonly float speed;
        private readonly Texture2D objectTexture;
        public string Text { get; set; }

        public event EventHandler Dead;

        public bool IsActive { get; set; }
        private EnemyTextBox textbox;
        private SoundEffect enemy_dead;

        public EnemyTextBox TextBox
        {
            get
            {
                return textbox;
            }
            set
            {
                textbox = value;
                Game.Components.Add(textbox);
            }
        }

        public Enemy(Game game, Vector2 initialPosition, float initialAngle, float zOrder, string imageAssetName, float speed, string text, Player player) : base(game, initialPosition, initialAngle, zOrder)
        {
            Text = text;
            this.player = player;
            this.speed = speed;
            objectTexture = game.Content.Load<Texture2D>(imageAssetName);
            IsActive = true;
        }


        public override int Width
        {
            get { return objectTexture.Width; }
        }

        public override int Height
        {
            get { return objectTexture.Height; }
        }

        public override void Initialize()
        {
            enemy_dead = Game.Content.Load<SoundEffect>("monster-dies");
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            if (!IsActive)
            {
                return;
            }

            Angle = GetRelativeAngle(player.Position);
            Position += GetTranslatedTransform(1, speed);
            if (player.RelativeBounds.Intersects(RelativeBounds))
            {
                player.Die();
            }
            TextBox.Location = new Vector2(Position.X, Position.Y + 30);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(objectTexture, Position, ObjectBounds, Color.White, Angle, Center, 1f, SpriteEffects.None, ZOrder);
            base.Draw(gameTime);
        }

        public void Die()
        {
            enemy_dead.Play();
            Game.Components.Remove(this);
            if (Dead != null)
            {
                Dead(this, EventArgs.Empty);
            }
            Game.Components.Remove(TextBox);
        }

        public override string ToString()
        {
            return TextBox.Text;
        }
    }
}
