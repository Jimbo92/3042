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
    class AnimSprite
    {
        public Texture2D Texture;
        public int Rows;
        public int Columns;
        public int Width;
        public int Height;
        public int currentFrame;

        private int totalFrames;

        public AnimSprite(ContentManager getContent, string getTexture, int getWidth, int getHeight, int getRows, int getColumns)
        {
            Texture = getContent.Load<Texture2D>(getTexture);
            Width = getWidth;
            Height = getHeight;
            Rows = getRows;
            Columns = getColumns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
        }

        public void UpdateAnimation()
        {
            currentFrame++;
            if (currentFrame == totalFrames)
                currentFrame = 0;
        }

        public void Draw(SpriteBatch sB, Vector2 getLocation)
        {
            int sourceWidth = Texture.Width / Columns;
            int sourceHeight = Texture.Height / Rows;

            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(sourceWidth * column, sourceHeight * row, sourceWidth, sourceHeight);
            Rectangle destinationRectangle = new Rectangle((int)getLocation.X, (int)getLocation.Y, Width, Height);

            sB.Draw(Texture,
                destinationRectangle,
                sourceRectangle,
                Color.White,
                0,
                new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2),
                SpriteEffects.None,
                0);
        }

    }
}
