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
        public List<Background_effects> StarList = new List<Background_effects>();
        public List<Background_effects> NebulaList = new List<Background_effects>();
        public List<Background_effects> PlanetList = new List<Background_effects>();

        private ContentManager Content;
        private int[] Timer = new int[3];
        private Random RandStarPos = new Random();
        private int RandStarPosNum;
        private Rectangle ScreenSize;

        public Background(ContentManager getContent, Rectangle getScreenSize)
        {
            Content = getContent;
            ScreenSize = getScreenSize;

            AddPlanet();
        }

        public void Update()
        {
            foreach (Background_effects star in StarList)
            {
                star.Update(0.05f);
                if (star.Position.Y >= 800)
                {
                    star.isAlive = false;
                }

            }
            foreach (Background_effects Nebula in NebulaList)
                Nebula.Update();
            foreach (Background_effects Planet in PlanetList)
            {
                Planet.Update();
                if (Planet.Position.Y >= ScreenSize.Y * 2)
                {
                    Planet.isAlive = false;
                }
            }

            if (StarList.Count >= 200)
            {
                StarList.RemoveAt(0);
            }

            Timer[1]++;
            if (Timer[1] >= 2000)
            {
                //AddNebula();
                AddPlanet();
                Timer[1] = 0;
            }

            Timer[0]++;
            if (Timer[0] >= 1.5f)
            {
                RandStarPosNum = RandStarPos.Next(701);
                AddStar(RandStarPosNum, RandStarPosNum);
                Timer[0] = 0;
            }
        }

        private void AddStar(float getPosX, float getDirX)
        {
            Random Rand = new Random();
            int RandNum = Rand.Next(6, 11);
            Background_effects star = new Background_effects(Content, "graphics/starss", RandNum, RandNum, 1, 2);
            star.Position = new Vector2(getPosX, -100);
            star.Direction = new Vector2(getDirX, 800) - star.Position;
            star.Direction.Normalize();
            Random Rand2 = new Random();
            float RandNum2 = Rand2.Next(2, 5);
            star.Speed = RandNum2;

            StarList.Add(star);
        }

        private void AddNebula()
        {
            Background_effects Nebula = new Background_effects(Content, "graphics/background_nebula", ScreenSize.X / 2, ScreenSize.Y / 2, 4, 4);
            Random Rand2 = new Random();
            int RandNum2 = Rand2.Next(-ScreenSize.X / 2, ScreenSize.X * 2 / 2);
            Nebula.Position = new Vector2(RandNum2, -ScreenSize.Y / 2);
            Nebula.Direction = new Vector2(RandNum2, ScreenSize.Y * 2) - Nebula.Position;
            Nebula.Direction.Normalize();
            Nebula.Speed = 3f;
            Random Rand = new Random();
            int RandNum = Rand.Next(1, 17);
            Nebula.SpriteAnim.currentFrame = RandNum;

            NebulaList.Add(Nebula);
        }
        private void AddPlanet()
        {
            Random Rand = new Random();
            int RandNum = Rand.Next(100, 400);
            Background_effects Planet = new Background_effects(Content, "graphics/background_planet3", RandNum, RandNum, 1, 3);
            Random Rand2 = new Random();
            int RandNum2 = Rand2.Next(-ScreenSize.X + ScreenSize.X, ScreenSize.X);
            Planet.Position = new Vector2(RandNum2, -ScreenSize.Y / 2);
            Planet.Direction = new Vector2(RandNum2, ScreenSize.Y * 2) - Planet.Position;
            Planet.Direction.Normalize();
            Planet.Speed = 1.5f;
            Random Rand3 = new Random();
            int RandNum3 = Rand3.Next(0, 3);
            Planet.SpriteAnim.currentFrame = RandNum3;

            PlanetList.Add(Planet);
        }


        public void Draw(SpriteBatch sB)
        {
            foreach (Background_effects Nebula in NebulaList)
                Nebula.Draw(sB);

            foreach (Background_effects star in StarList)
                star.Draw(sB);

            foreach (Background_effects Planet in PlanetList)
                Planet.Draw(sB);
        }
    }
}
