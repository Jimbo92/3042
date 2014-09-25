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
    class MenuButton
    {
        public AnimSprite SpriteAnim;
        public Rectangle MousePos;
        public Rectangle CollisionBox;
        public bool isReleased = false;

        private Font ButtonText;
        private SoundEffect[] ButtonSounds = new SoundEffect[2];
        private SoundEffectInstance[] ButtonSoundsIns = new SoundEffectInstance[2];
        private int[] SFXTimer = new int[2];

        public MenuButton(ContentManager getContent, int getWidth, int getHeight)
        {
            SpriteAnim = new AnimSprite(getContent, "graphics/buttonss", getWidth, getHeight, 1, 3);
            ButtonText = new Font(getContent);
            ButtonSounds[0] = getContent.Load<SoundEffect>("sound/MenuSelectionRollover");
            ButtonSounds[1] = getContent.Load<SoundEffect>("sound/MenuSelectionClick");
        }

        public void Update()
        {
            for (int i = 0; i < 2; i++)
            {
                ButtonSoundsIns[i] = ButtonSounds[i].CreateInstance();
                ButtonSoundsIns[i].Volume = 0.3f;
            }

            ButtonSoundsIns[1].Pitch = -0.2f;
            ButtonSoundsIns[1].Volume = 0.1f;

            if (CheckCollision.Collision(MousePos, CollisionBox))
            {
                SFXTimer[0]++;
                if (SFXTimer[0] <= 1)
                    ButtonSoundsIns[0].Play();
                else
                    SFXTimer[0] = 2;

                if (Input.ClickPress(Input.EClicks.LEFT))
                {
                    SFXTimer[1]++;
                    if (SFXTimer[1] <= 1)
                        ButtonSoundsIns[1].Play();
                    else
                        SFXTimer[1] = 2;

                    SpriteAnim.currentFrame = 2;
                }
                else
                {
                    SFXTimer[1] = 0;
                    SpriteAnim.currentFrame = 1;
                }

                if (Input.ClickReleased(Input.EClicks.LEFT))
                    isReleased = true;
                else
                    isReleased = false;
            }
            else
            {
                SFXTimer[0] = 0;
                SpriteAnim.currentFrame = 0;
            }
        }

        public void Draw(SpriteBatch sB, string getText, float getTextSize, Vector2 getPos)
        {
            MousePos = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 0, 0);
            CollisionBox = new Rectangle((int)getPos.X - SpriteAnim.Width / 2, (int)getPos.Y - SpriteAnim.Height / 2, SpriteAnim.Width, SpriteAnim.Height);

            SpriteAnim.Draw(sB, getPos);

            ButtonText.Draw(sB, getText, getPos, getTextSize, Color.White);
        }

    }
}
