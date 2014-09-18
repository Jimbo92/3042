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
    class Level_Base
    {
        //Varibles
        public Player player;


        public Level_Base(ContentManager getContent, Rectangle getScreenSize)
        {
            player = new Player(getContent, getScreenSize);
        }

        public void Update(GameTime getGameTime)
        {
            player.Update(getGameTime);
        }

        public void Draw(SpriteBatch sB)
        {
            player.Draw(sB);
        }


    }
}
