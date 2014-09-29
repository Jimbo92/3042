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
    class Menu
    {
        public MenuButton[] Buttons = new MenuButton[3];

        private ContentManager Content;
        private Rectangle ScreenSize;
        private BasicSprite MenuBackgroundImage;
        private Font Title;

        public Menu(ContentManager getContent, Rectangle getScreenSize)
        {
            Content = getContent;
            ScreenSize = getScreenSize;

            Title = new Font(getContent);
            MenuBackgroundImage = new BasicSprite(getContent, "graphics/menubackground", ScreenSize.Width, ScreenSize.Height);
            for (int i = 0; i < 3; i++)
            {
                Buttons[i] = new MenuButton(Content, 164, 32);
            }
        }

        public void Update(Game1 getGame1)
        {
            for (int i = 0; i < 3; i++)
            {
                Buttons[i].Update();
            }

            //Buttons
            if (CheckCollision.Collision(Buttons[0].MousePos, Buttons[0].CollisionBox))
                if (Input.ClickReleased(Input.EClicks.LEFT))
                    GameMode.Mode = GameMode.EGameMode.LEVELSELECT;
            if (CheckCollision.Collision(Buttons[1].MousePos, Buttons[1].CollisionBox))
                if (Input.ClickReleased(Input.EClicks.LEFT))
                    GameMode.Mode = GameMode.EGameMode.OPTIONS;
            if (CheckCollision.Collision(Buttons[2].MousePos, Buttons[2].CollisionBox))
                if (Input.ClickReleased(Input.EClicks.LEFT))
                    getGame1.Exit();

        }

        public void Draw(SpriteBatch sB)
        {
            MenuBackgroundImage.Draw(sB, new Vector2(ScreenSize.X / 2, ScreenSize.Y / 2));
            Title.Draw(sB, "3042", new Vector2(ScreenSize.X / 4, ScreenSize.Y / 2), 2f, Color.White);

            //Draw Buttons Last
            Buttons[0].Draw(sB, "Play", 0.5f, new Vector2(ScreenSize.X / 5, ScreenSize.Y - 250));
            Buttons[1].Draw(sB, "Options", 0.5f, new Vector2(ScreenSize.X / 5, ScreenSize.Y - 200));
            Buttons[2].Draw(sB, "Quit", 0.5f, new Vector2(ScreenSize.X / 5, ScreenSize.Y - 150));
        }

    }
}
