﻿using System;
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
    class BasicSprite
    {
        public Texture2D Texture;
        public int Width;
        public int Height;
        public Vector2 Position;

        public BasicSprite(ContentManager getContent, string getTexture, int getWidth, int getHeight)
        {
            Texture = getContent.Load<Texture2D>(getTexture);

            Width = getWidth;
            Height = getHeight;
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch sB)
        {
            Rectangle _destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

            sB.Draw(Texture,
                _destinationRectangle,
                null,
                Color.White,
                0,
                new Vector2(_destinationRectangle.Width / 2, _destinationRectangle.Height / 2),
                SpriteEffects.None,
                0);
                
        }

    }
}