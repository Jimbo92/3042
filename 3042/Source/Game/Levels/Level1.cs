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

        public Level1(ContentManager getContent, Rectangle getScreenSize)
        {
            BaseCode = new Level_Base(getContent, getScreenSize);
            //Code Bellow this


        }

        public void Update(GameTime getGameTime)
        {
            BaseCode.Update(getGameTime);
            //Code bellow this




        }

        public void Draw(SpriteBatch sB)
        {
            BaseCode.Draw(sB);
            //Code bellow this



        }


    }
}
