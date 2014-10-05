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
    class GameOver
    {
        public BasicSprite Background;
        public MenuButton[] buttons = new MenuButton[2];
        public Font Title;

        private Rectangle ScreenSize;

        public GameOver(ContentManager getContent, Rectangle getScreenSize)
        {
            ScreenSize = getScreenSize;

            Background = new BasicSprite(getContent, "graphics/gameoverbackground", 700, 700);

            Title = new Font(getContent);

            for (int i = 0; i < buttons.Length; i++)
                buttons[i] = new MenuButton(getContent, 128, 32);
        }

        public void Update()
        {
            if (Input.KeyboardPressed(Keys.Escape))
                GameMode.Mode = GameMode.EGameMode.MENU;

            for (int i = 0; i < buttons.Length; i++)
                buttons[i].Update();

             
            if (CheckCollision.Collision(buttons[0].MousePos, buttons[0].CollisionBox))
                if (Input.ClickReleased(Input.EClicks.LEFT))
                    GameMode.Mode = GameMode.EGameMode.MENU;
            if (CheckCollision.Collision(buttons[1].MousePos, buttons[1].CollisionBox))
                if (Input.ClickReleased(Input.EClicks.LEFT))
                    GameMode.Mode = GameMode.EGameMode.LEVELSELECT;
        }

        public void Draw(SpriteBatch sB)
        {
            Background.Draw(sB, new Vector2(ScreenSize.X / 2, ScreenSize.Y / 2));

            buttons[0].Draw(sB, "Menu", 0.5f, new Vector2(ScreenSize.X / 6, ScreenSize.Y - 100));
            buttons[1].Draw(sB, "Retry", 0.5f, new Vector2(ScreenSize.X / 6, ScreenSize.Y - 150));

            Title.Draw(sB, "GAME OVER", new Vector2(ScreenSize.X / 2, ScreenSize.Y / 7), 1.5f, Color.White);
        }
    }
}
