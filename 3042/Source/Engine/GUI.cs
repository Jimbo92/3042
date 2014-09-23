using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace _3042
{
    class GUI
    {
        public SpriteFont FontRegular;
        public int Score;
        public List<Lives> _Lives = new List<Lives>();
        public int PlayerLives = 3;
        public bool ResetLives = false;
        public float PlayerHealth = 100;


        private Rectangle ScreenSize;
        private ContentManager Content;
        private BasicSprite HealthBar;
        private Player player;

        public GUI(ContentManager getContent, Rectangle getScreenSize)
        {
            ScreenSize = getScreenSize;
            Content = getContent;

            FontRegular = getContent.Load<SpriteFont>("fonts/font_regular");

            //Health Bar
            HealthBar = new BasicSprite(getContent, "graphics/healthbar", 20, (int)PlayerHealth);

            //Lives
            AddLives(new Vector2(50, ScreenSize.Y - 80));
            AddLives(new Vector2(90, ScreenSize.Y - 80));
            AddLives(new Vector2(130, ScreenSize.Y - 80));
        }

        public void Update(Player getPlayer)
        {
            player = getPlayer;

            HealthAndLives();
        }

        private void HealthAndLives()
        {
            HealthBar.Height = (int)PlayerHealth;
            if (PlayerHealth <= 0)
            {
                player.isReset = true;
                player.ImmuneTimer = 0;

                PlayerLives--;
                _Lives.RemoveAt(0);
                PlayerHealth = 100;
            }
            if (PlayerLives <= 0)
            {
                ResetLives = true;
                PlayerLives = 3;
            }
            else
                ResetLives = false;

            if (ResetLives)
            {
                AddLives(new Vector2(50, ScreenSize.Y - 80));
                AddLives(new Vector2(90, ScreenSize.Y - 80));
                AddLives(new Vector2(130, ScreenSize.Y - 80));
            }
        }
        private void AddLives(Vector2 getPosition)
        {
            Lives life = new Lives();
            life.LoadContent(Content);
            life.Position = getPosition;

            _Lives.Add(life);
        }

        public void Draw(SpriteBatch sB)
        {
            //Score
            sB.DrawString(FontRegular,
                "Score " + Score.ToString(),
                new Vector2(50, ScreenSize.Y - 50),
                Color.White,
                0,
                new Vector2(),
                0.5f,
                SpriteEffects.None,
                0);

            //Health Bar
            HealthBar.Draw(sB, new Vector2(35, ScreenSize.Y - 20), new Vector2(0, 0), MathHelper.ToRadians(180));

            //Lives
            foreach (Lives life in _Lives)
            {
                life.Draw(sB);
            }
        }


    }
}
