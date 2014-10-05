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
    class Level1
    {
        public Level_Base BaseCode;
        public Font AsteroidWarning;

        private int GameTicks;
        private Rectangle ScreenSize;
        private int AsteroidWarningTimer;
        private SoundEffect WarningSFX;
        private SoundEffectInstance WarningSFXIns;
        private int GameTimer;
        private int GameTimePast;
        private Font DebugGameTime;

        //Music
        private SoundEffect BackgroundMusicSong;
        private SoundEffectInstance BackgroundMusicSongIns;

        public Level1(ContentManager getContent, Rectangle getScreenSize)
        {
            ScreenSize = getScreenSize;

            BaseCode = new Level_Base(getContent, getScreenSize);
            //Code Bellow this

            AsteroidWarning = new Font(getContent);
            WarningSFX = getContent.Load<SoundEffect>("sound/warning");
            WarningSFXIns = WarningSFX.CreateInstance();
            WarningSFXIns.Volume = 0.1f;
            WarningSFXIns.Pitch = 0.5f;

            BackgroundMusicSong = getContent.Load<SoundEffect>("sound/Backgroundmusic3");
            GameMode.UniversalMusic = BackgroundMusicSongIns = BackgroundMusicSong.CreateInstance();
            BackgroundMusicSongIns.IsLooped = true;
            BackgroundMusicSongIns.Volume = 0.1f;

            DebugGameTime = new Font(getContent);

        }

        public void Update(GameTime getGameTime)
        {
            BaseCode.Update(getGameTime, ScreenSize);
            GameTicks = getGameTime.TotalGameTime.Seconds;

            if (GameMode.Mode == GameMode.EGameMode.GAMEOVER || GameMode.Mode == GameMode.EGameMode.MENU)
                GameTimePast = 0;

            GameTimer++;
            if (GameTimer >= 50)
            {
                GameTimePast++;
                GameTimer = 0;
            }

            if (GameMode.UniMusic == GameMode.EUniMusic.Mute || GameMode.Mode == GameMode.EGameMode.MENU || GameMode.Mode == GameMode.EGameMode.GAMEOVER)
                BackgroundMusicSongIns.Stop();
            else if (GameMode.UniMusic == GameMode.EUniMusic.Unmute)
                BackgroundMusicSongIns.Play();
            //Code bellow this

            if (GameTimePast >= 1 && GameTimePast <= 20)
            {
                BaseCode.RandAsteroidWave(200);
            }
            if (GameTimePast >= 23 && GameTimePast <= 35)
            {
                BaseCode.RandAsteroidWave(10);
            }

            if (GameTimePast >= 10 && GameTimePast <= 15)
            {
                BaseCode.SmallEnemyCurvLeftWave(25);
                BaseCode.SmallEnemyCurvRightWave(25);
            }
            if (GameTimePast >= 40 && GameTimePast <= 50)
            {
                BaseCode.SmallEnemyCurvRightWave(50);
                BaseCode.SmallEnemyCurvUpLeftWave(50);
            }

        }

        public void Draw(SpriteBatch sB)
        {
            BaseCode.Draw(sB);
            //Code Bellow this

            DebugGameTime.Draw(sB, "Game Time " + GameTimePast.ToString(), new Vector2(100, 50), 0.3f, Color.Red);

            if (GameTimePast >= 20 && GameTimePast <= 20.5)
            {
                WarningSFXIns.Play();

                AsteroidWarningTimer++;
                if (AsteroidWarningTimer >= 3)
                {
                    AsteroidWarning.Draw(sB, "Incoming Asteroid Field!",
                        new Vector2(ScreenSize.X / 2, ScreenSize.Y / 3),
                        .5f,
                        Color.White);

                    AsteroidWarningTimer = 0;
                }
            }
        }


    }
}
