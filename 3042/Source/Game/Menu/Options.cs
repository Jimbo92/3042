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
    class Options
    {
        public MenuButton[] Buttons = new MenuButton[3];
        private BasicSprite OptionsBackgroundImage;
        private Rectangle ScreenSize;
        private Font[] Fonts = new Font[3];
        private string MusicChoice;

        public Options(ContentManager getContent, Rectangle getScreenSize)
        {
            ScreenSize = getScreenSize;

            OptionsBackgroundImage = new BasicSprite(getContent, "graphics/optionsbackground", ScreenSize.Width, ScreenSize.Height);
            
            for (int i = 0; i < 3; i++)
            {
                Buttons[i] = new MenuButton(getContent, 164, 32);
                Fonts[i] = new Font(getContent);
            }
        }

        public void Update()
        {
            for (int i = 0; i < 3; i++)
                Buttons[i].Update();

            //Buttons
            if (CheckCollision.Collision(Buttons[0].MousePos, Buttons[0].CollisionBox))
                if (Input.ClickReleased(Input.EClicks.LEFT))
                {
                    if (GameMode.UniMusic == GameMode.EUniMusic.Mute)
                        GameMode.UniMusic = GameMode.EUniMusic.Unmute;
                    else if (GameMode.UniMusic == GameMode.EUniMusic.Unmute)
                        GameMode.UniMusic = GameMode.EUniMusic.Mute;
                }
            if (CheckCollision.Collision(Buttons[1].MousePos, Buttons[1].CollisionBox))
                if (Input.ClickReleased(Input.EClicks.LEFT))
                {
                    if (GameMode.Controls == GameMode.EControls.Mouse)
                        GameMode.Controls = GameMode.EControls.Keyboard;
                    else if (GameMode.Controls == GameMode.EControls.Keyboard)
                        GameMode.Controls = GameMode.EControls.Mouse;
                }
            if (CheckCollision.Collision(Buttons[2].MousePos, Buttons[2].CollisionBox))
                if (Input.ClickReleased(Input.EClicks.LEFT))
                    GameMode.Mode = GameMode.EGameMode.MENU;

        }

        public void Draw(SpriteBatch sB)
        {
            OptionsBackgroundImage.Draw(sB, new Vector2(ScreenSize.X / 2, ScreenSize.Y / 2));
            //Fonts
            Fonts[0].Draw(sB, "Options", new Vector2(ScreenSize.X / 2, 50), 1f, Color.White);
            Fonts[1].Draw(sB, "Music", new Vector2(ScreenSize.X / 4, 150), .7f, Color.White);
            Fonts[2].Draw(sB, "Controls", new Vector2(ScreenSize.X / 4 * 3, 150), .7f, Color.White);

            switch (GameMode.UniMusic)
            {
                case GameMode.EUniMusic.Mute: MusicChoice = "Unmute"; break;
                case GameMode.EUniMusic.Unmute: MusicChoice = "Mute"; break;
            }

            //Draw Buttons Last
            Buttons[0].Draw(sB, MusicChoice, 0.5f, new Vector2(ScreenSize.X / 4, 200));
            Buttons[1].Draw(sB, GameMode.Controls.ToString(), 0.5f, new Vector2(ScreenSize.X / 4 * 3, 200));
            Buttons[2].Draw(sB, "Back", 0.5f, new Vector2(100, ScreenSize.Y - 50));
        }
    }
}
