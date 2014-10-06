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

        private int[] EnemySpawnTimers = new int[10];

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

            if (GameTimePast >= 1 && GameTimePast <= 100)
            {
                BaseCode.RandAsteroidWave(0, 200);
            }
            if (GameTimePast >= 10 && GameTimePast <= 15)
            {
                BaseCode.SmallEnemyWave(2, 50, Enemy.EEnemyType.Warp_JumpLeftRightDown, ScreenSize.X - 50, 50);
                BaseCode.BigEnemyWave(3, 100, Enemy.EEnemyType.Warp_JumpRightLeftDown, 50, 100);
            }
            if (GameTimePast >= 23 && GameTimePast <= 35)
            {
                BaseCode.RandAsteroidWave(1, 10);
            }
            if (GameTimePast >= 40 && GameTimePast <= 50)
            {
                BaseCode.BigEnemyWave(2, 100, Enemy.EEnemyType.Warp_CurvUpLeft, ScreenSize.X - 100, 600);
                BaseCode.SmallEnemyWave(3, 35, Enemy.EEnemyType.Warp_JumpRightLeftDown, 50, 100);
            }
            if (GameTimePast >= 55 && GameTimePast <= 65)
            {
                BaseCode.BigEnemyWave(2, 50, Enemy.EEnemyType.Warp_CurvLeft, ScreenSize.X - 100, 50);
                BaseCode.SmallEnemyWave(3, 35, Enemy.EEnemyType.Warp_CurvRight, 50, 100);
            }
            if (GameTimePast >= 70 && GameTimePast <= 80)
            {
                BaseCode.BigEnemyWave(2, 45, Enemy.EEnemyType.Warp_CurvUpLeft, ScreenSize.X - 100, 600);
                BaseCode.BigEnemyWave(3, 50, Enemy.EEnemyType.Warp_CurvUpRight, 50, 550);
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
