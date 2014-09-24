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
    class Font
    {
        public SpriteFont Regular;

        public Font(ContentManager getContent)
        {
            Regular = getContent.Load<SpriteFont>("fonts/font_regular");
        }


        public void Draw(SpriteBatch sB, string getString, Vector2 getPos, float getSize, Color getColour)
        {
            Vector2 FontSize = Regular.MeasureString(getString);

            sB.DrawString(Regular,
                getString,
                getPos,
                getColour,
                0,
                FontSize / 2,
                getSize,
                SpriteEffects.None,
                0);
        }
        public void Draw(SpriteBatch sB, string getString, Vector2 getPos, float getSize, Vector2 getOrigin, Color getColour)
        {
            sB.DrawString(Regular,
                getString,
                getPos,
                getColour,
                0,
                getOrigin,
                getSize,
                SpriteEffects.None,
                0);
        }

    }
}
