using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        private readonly SpriteBatch spriteBatch;

        private readonly SpriteFont matchedFont, unmatchedFont;

        private int matchedPrefixLength;

        /// <summary>
        /// Create a text box.
        /// </summary>
        /// <param name="game">The Game that the game component should be attached to.</param>
        /// <param name="text">The text to display in the text box.</param>
        public EnemyTextBox(Game game, string text) : base(game)
        {
            Text = text;
            Location = new Vector2(-50);
            matchedPrefixLength = 0;
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            matchedFont = game.Content.Load<SpriteFont>("MatchedFont");
            unmatchedFont = game.Content.Load<SpriteFont>("UnmatchedFont");
            UpdateDimensions();
        }

        private void UpdateDimensions()
        {
            Dimension = new Vector2(matchedFont.MeasureString(Text).X + 2, matchedFont.MeasureString(Text).Y);
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
                UpdateDimensions();
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
            string matchedString = Text.Substring(0, matchedPrefixLength);
            string unmatchedString = Text.Substring(matchedPrefixLength);

            Vector2 unmatchedLocation = new Vector2(Location.X + matchedFont.MeasureString(matchedString).X, Location.Y);

            spriteBatch.DrawString(matchedFont, matchedString, Location, Color.Red, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(unmatchedFont, unmatchedString, unmatchedLocation, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.5f);

            base.Draw(gameTime);
        }

    }
}