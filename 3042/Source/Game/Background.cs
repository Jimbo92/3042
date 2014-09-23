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
    class Background
    {
        public List<Star> StarList = new List<Star>();

        private ContentManager Content;
        private int Timer;
        private Random RandStarPos = new Random();
        private int RandStarPosNum;

        public Background(ContentManager getContent)
        {
            Content = getContent;
        }

        public void Update()
        {
            foreach (Star star in StarList)
            {
                star.Update();
                if (star.Position.Y >= 800)
                {
                    star.isAlive = false;
                }
            }

            Timer++;
            if (Timer >= 1)
            {
                RandStarPosNum = RandStarPos.Next(701);
                AddStar(RandStarPosNum, RandStarPosNum);
                Timer = 0;
            }
        }

        private void AddStar(float getPosX, float getDirX)
        {
            Star star = new Star(Content);
            star.Position = new Vector2(getPosX, -100);
            star.Direction = new Vector2(getDirX, 800) - star.Position;
            star.Direction.Normalize();
            star.Speed = 5f;

            StarList.Add(star);
        }


        public void Draw(SpriteBatch sB)
        {
            foreach (Star star in StarList)
            {
                star.Draw(sB);
            }
        }
    }
}
