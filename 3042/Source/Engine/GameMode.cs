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
    static class GameMode
    {
        public enum EGameMode
        {
            MENU,
            LEVELSELECT,
            OPTIONS,
            GAMEOVER
        }
        static public EGameMode Mode = EGameMode.MENU;

        public enum EControls
        {
            Keyboard,
            Mouse
        }
        static public EControls Controls = EControls.Mouse;

        public enum EUniMusic
        {
            Mute,
            Unmute
        }
        static public EUniMusic UniMusic = EUniMusic.Unmute;

        static public SoundEffectInstance UniversalMusic;
    }
}
