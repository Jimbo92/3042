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
        public float AltBarAmount = 100;


        private Rectangle ScreenSize;
        private ContentManager Content;
        private BasicSprite HealthBar;
        private BasicSprite AltBar;
        private BasicSprite[] BarBackground = new BasicSprite[2];
        private Player player;
        private Font AltReady;
        private int[] AltReadyTimer = new int[2];
        private SoundEffect AltReadySFX;
        private SoundEffectInstance AltReadyInsSFX;

        public GUI(ContentManager getContent, Rectangle getScreenSize)
        {
            ScreenSize = getScreenSize;
            Content = getContent;

            FontRegular = getContent.Load<SpriteFont>("fonts/font_regular");

            //Health Bar
            HealthBar = new BasicSprite(getContent, "graphics/healthbar", 30, (int)PlayerHealth);
            //Alt Bar
            AltBar = new BasicSprite(getContent, "graphics/altbar", 30, (int)AltBarAmount);
            AltReady = new Font(getContent);
            AltReadySFX = Content.Load<SoundEffect>("sound/altready");
            AltReadyInsSFX = AltReadySFX.CreateInstance();
            AltReadyInsSFX.Volume = 0.05f;
            //Bar Background
            for (int i = 0; i < 2; i++)
                BarBackground[i] = new BasicSprite(Content, "graphics/barbackground", HealthBar.Width + 10, HealthBar.Height + 10);

            //Lives
            AddLives(new Vector2(130, ScreenSize.Y - 80));
            AddLives(new Vector2(90, ScreenSize.Y - 80));
            AddLives(new Vector2(50, ScreenSize.Y - 80));
        }

        public void Update(Player getPlayer)
        {
            player = getPlayer;

            HealthAndAlt();
        }

        private void HealthAndAlt()
        {
            HealthBar.Height = (int)PlayerHealth;
            AltBar.Height = (int)AltBarAmount;

            if (AltBarAmount >= 100)
            {
                player.isAltFire = true;
                AltBarAmount = 100;
            }
            else
            {
                AltBarAmount += 0.2f;
                player.isAltFire = false;
            }

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
                AddLives(new Vector2(130, ScreenSize.Y - 80));
                AddLives(new Vector2(90, ScreenSize.Y - 80));
                AddLives(new Vector2(50, ScreenSize.Y - 80));
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
            //BarBackgrounds
            BarBackground[0].Draw(sB, new Vector2(40 - 30 / 2, ScreenSize.Y - 30 - 100 / 2));
            BarBackground[1].Draw(sB, new Vector2(ScreenSize.X - 9 - 30 / 2, ScreenSize.Y - 30 - 100 / 2));
            //Health Bar
            HealthBar.Draw(sB, new Vector2(39, ScreenSize.Y - 20), new Vector2(0, 0), MathHelper.ToRadians(180));
            //Alt Bar
            AltBar.Draw(sB, new Vector2(ScreenSize.X - 10, ScreenSize.Y - 20), new Vector2(0, 0), MathHelper.ToRadians(180));
            if (AltBarAmount >= 100)
            {
                AltReadyTimer[0]++;
                if (AltReadyTimer[0] <= 35)
                {
                    AltReadyInsSFX.Play();
                    AltReadyTimer[1]++;
                    if (AltReadyTimer[1] >= 3)
                    {
                        AltReady.Draw(sB, "R\nE\nA\nD\nY", new Vector2(ScreenSize.X - 25, ScreenSize.Y - 70), 0.3f, Color.DarkBlue);
                        AltReadyTimer[1] = 0;
                    }
                }
                else
                    AltReadyTimer[0] = 36;
            }
            else 
                AltReadyTimer[0] = 0;

            //Lives
            foreach (Lives life in _Lives)
            {
                life.Draw(sB);
            }
        }


    }
}
