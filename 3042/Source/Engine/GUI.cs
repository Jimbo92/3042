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
        public Font ScoreFont;
        public Font HealthFont;
        public Font WepLvlFont;
        public int Score;
        public Lives[] _Lives = new Lives[3];
        public int PlayerLives = 3;
        public bool ResetLives = false;
        public float PlayerHealth = 100;
        public float AltBarAmount = 100;
        public string WepLvl;

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

            ScoreFont = new Font(getContent);
            HealthFont = new Font(getContent);
            WepLvlFont = new Font(getContent);

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

            for (int i = 0; i < 3; i++)
            {
                _Lives[i] = new Lives(getContent);
            }

        }

        public void Update(Player getPlayer)
        {
            player = getPlayer;

            HealthAndAlt();

            if (GameMode.Mode == GameMode.EGameMode.GAMEOVER || GameMode.Mode == GameMode.EGameMode.MENU)
                Reset();
        }

        private void Reset()
        {
            Score = 0;
            PlayerLives = 3;
            player.isReset = true;
            player.ImmuneTimer = 0;
            player._weaponType = Player.EWeaponType.BASIC;
            PlayerHealth = 100;
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
                player._weaponType = Player.EWeaponType.BASIC;
                PlayerLives--;
                PlayerHealth = 100;
            }

            if (PlayerLives >= 4)
                PlayerLives = 3;

            if (PlayerLives <= 0)
                GameMode.Mode = GameMode.EGameMode.GAMEOVER;
        }

        public void Draw(SpriteBatch sB)
        {
            //Score
            ScoreFont.Draw(sB, "Score " + Score.ToString(), new Vector2(ScreenSize.X / 2, ScreenSize.Y - 25), 0.5f, Color.White);
            //BarBackgrounds
            BarBackground[0].Draw(sB, new Vector2(40 - 30 / 2, ScreenSize.Y - 15 - 100 / 2));
            BarBackground[1].Draw(sB, new Vector2(ScreenSize.X - 9 - 30 / 2, ScreenSize.Y - 15 - 100 / 2));
            //Health Bar
            HealthBar.Draw(sB, new Vector2(40, ScreenSize.Y - 15), new Vector2(0, 0), MathHelper.ToRadians(180));
            HealthFont.Draw(sB, "HP", new Vector2(25, ScreenSize.Y - 30), 0.3f, Color.White);
            //Alt Bar
            AltBar.Draw(sB, new Vector2(ScreenSize.X - 9, ScreenSize.Y - 15), new Vector2(0, 0), MathHelper.ToRadians(180));
            if (AltBarAmount >= 100)
            {
                AltReadyTimer[0]++;
                if (AltReadyTimer[0] <= 35)
                {
                    AltReadyInsSFX.Play();
                    AltReadyTimer[1]++;
                    if (AltReadyTimer[1] >= 3)
                    {
                        AltReady.Draw(sB, "R\nE\nA\nD\nY", new Vector2(ScreenSize.X - 25, ScreenSize.Y - 65), 0.3f, Color.DarkBlue);
                        AltReadyTimer[1] = 0;
                    }
                }
                else
                    AltReadyTimer[0] = 36;
            }
            else 
                AltReadyTimer[0] = 0;
            //Weapon Level Font
            WepLvlFont.Draw(sB, "Weapon\n" + WepLvl, new Vector2(ScreenSize.X - 85, ScreenSize.Y - 25), 0.3f, Color.White);

            //Lives
            if (PlayerLives >= 1)
            _Lives[0].Draw(sB, new Vector2(60, ScreenSize.Y - 40));
            if (PlayerLives >= 2)
            _Lives[1].Draw(sB, new Vector2(60, ScreenSize.Y - 70));
            if (PlayerLives >= 3)
            _Lives[2].Draw(sB, new Vector2(60, ScreenSize.Y - 100));
        }


    }
}
