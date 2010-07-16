using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Typocalypse
{
    public class EnemyTextBox : DrawableGameComponent
    {

        /// <summary>
        /// Text to display in text box.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Location of text box on the screen.
        /// </summary>
        public Vector2 Location { get; set; }

        /// <summary>
        /// Dimension of text box on the screen.
        /// </summary>
        public Vector2 Dimension { get; private set; }

        private SpriteBatch spriteBatch;

        private SpriteFont matchedFont, unmatchedFont;

        private int matchedPrefixLength;

        /// <summary>
        /// Create a text box.
        /// </summary>
        /// <param name="game">The Game that the game component should be attached to.</param>
        /// <param name="text">The text to display in the text box.</param>
        /// <param name="location">The location of the text box on the screen.</param>
        public EnemyTextBox(Game game, string text)
            : base(game)
        {
            this.Text = text;
            this.Location = new Vector2(-50);
            this.matchedPrefixLength = 0;
            this.spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            this.matchedFont = game.Content.Load<SpriteFont>("MatchedFont");
            this.unmatchedFont = game.Content.Load<SpriteFont>("UnmatchedFont");
            updateDimensions();
        }

        private void updateDimensions()
        {
            Dimension = new Vector2(matchedFont.MeasureString(Text).X+2, matchedFont.MeasureString(Text).Y);
        }

        /// <summary>
        /// Get the length of the matched prefix of the text.
        /// </summary>
        /// <returns>The length.</returns>
        public int GetMatchedPrefixLength()
        {
            return matchedPrefixLength;
        }

        /// <summary>
        /// Increment the length of the matched prefix of the text.
        /// </summary>
        public void MatchedPrefixLength(int prefixLength)
        {
            if (prefixLength < Text.Length)
            {
                matchedPrefixLength = prefixLength;
                updateDimensions();
            }
        }

        /// <summary>
        /// Reset the length of the matched prefix of the text to zero.
        /// </summary>
        public void ResetMatchedPrefixLength()
        {
            matchedPrefixLength = 0;
        }

        public override void Draw(GameTime gameTime)
        {
            string matchedString   = Text.Substring(0, matchedPrefixLength);
            string unmatchedString = Text.Substring(matchedPrefixLength);

            Vector2 unmatchedLocation = new Vector2(Location.X + matchedFont.MeasureString(matchedString).X, Location.Y);

            spriteBatch.DrawString(matchedFont, matchedString, Location, Color.Red, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(unmatchedFont, unmatchedString, unmatchedLocation, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.5f);

            base.Draw(gameTime);
        }

    }
}