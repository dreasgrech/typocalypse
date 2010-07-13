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
using Typocalypse.Trie;

namespace Typocalypse
{
    public class EnemyInputManager 
    {

        private static Random rand = new Random();
        private Trie<Enemy> trie;
        private List<String> texts;
        private Game game;
        private List<Enemy> currentlyMatched;

        public EnemyInputManager(List<String> texts, Game game)
        {
            this.trie = new Trie<Enemy>();
            this.texts = texts;
            this.game = game;
            currentlyMatched = new List<Enemy>();
        }

        private static double BiasedRandom(double bias)
        {
            return (Math.Exp(bias * rand.NextDouble()) - 1) / (Math.Exp(bias) - 1);
        }

        public EnemyTextBox RegisterEnemy(Enemy enemy, double bias)
        {
            if (bias == 0)
            {
                bias += 0.1;
            }
            String text = texts[(int)(texts.Count * BiasedRandom(bias))];
            EnemyTextBox textBox = new EnemyTextBox(game, text);
            trie.Put(text, enemy);

            return textBox;
        }

        public Enemy InputKey(char chr)
        {
            var matchResult = trie.Matcher.NextMatch(chr);
            if (!matchResult)
            {
                currentlyMatched.ForEach(e => e.TextBox.ResetMatchedPrefixLength());
                currentlyMatched.Clear();
                trie.Matcher.ResetMatch();
            } else
            {
                var newlyMatched = trie.Matcher.GetPrefixMatches();
                currentlyMatched.Except(newlyMatched).ToList().ForEach(e => e.TextBox.ResetMatchedPrefixLength());
                newlyMatched.ForEach(e => e.TextBox.MatchedPrefixLength(trie.Matcher.GetPrefix().Length));
                currentlyMatched = newlyMatched;
                var exactMatch = trie.Matcher.GetExactMatch();
                if (exactMatch != null)
                {
                    trie.Matcher.ResetMatch();
                    currentlyMatched.ForEach(e => e.TextBox.ResetMatchedPrefixLength());
                    currentlyMatched.Clear();
                }
                return exactMatch;
            }
            return null;
        }

    }
}