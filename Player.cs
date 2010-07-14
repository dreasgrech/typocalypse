using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Typocalypse
{
    public class Player: GameObject
    {
        private Texture2D objectTexture;
        Vector2 scoreDisplayPosition = new Vector2(50, 30);

        private readonly SoundEffect playerDies;
        public int Score { get; set; }
        private readonly SpriteFont scoreDisplay;

        public EnemyManager EnemyManager{ get; set;}
        public bool IsAlive { get; set; }

        public Player(Game game, Vector2 initialPosition, float initialAngle, float zOrder, string imageAssetName) : base(game, initialPosition, initialAngle, zOrder)
        {
            objectTexture = game.Content.Load<Texture2D>(imageAssetName);
            scoreDisplay = game.Content.Load<SpriteFont>("ScoreFont");
            playerDies = game.Content.Load<SoundEffect>("player-dies");
            IsAlive = true;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SpriteBatch.Draw(objectTexture, Position, ObjectBounds, Color.White, Angle, Center, 1f, SpriteEffects.None, ZOrder);
            SpriteBatch.DrawString(scoreDisplay,"Score: " + Score,scoreDisplayPosition,Color.Red, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 1f);
        }

        public override int Width
        {
            get { return objectTexture.Width; }
        }

        public override int Height
        {
            get { return objectTexture.Height; }
        }

        public void Shoot(Enemy enemy)
        {
            Angle = GetRelativeAngle(new Vector2(enemy.X, enemy.Y));
            var p = new Projectile(Game, Position, Angle, 0.5f, "projectile", 23);
            Game.Components.Add(p);
        }

        public void EnemyKilled(int score)
        {
            Score += 10;
        }

        public void Move(int direction, float speed)
        {
            Position += GetTranslatedTransform(direction, speed);
        }

        public void Rotate(float direction)
        {
            Angle += MathHelper.ToRadians(direction);
        }

        public void Die()
        {
            IsAlive = false;
            EnemyManager.PlayerIsHit();
            playerDies.Play();
            MediaPlayer.Stop();
            Game.Components.Remove(this);
        }

    }
}