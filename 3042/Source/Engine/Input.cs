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
    static class Input
    {
        static public KeyboardState PreviousKeyboard;
        static public KeyboardState CurrentKeyboard;

        static public bool KeyboardPressed(Keys key)
        {
            if (CurrentKeyboard.IsKeyDown(key) && PreviousKeyboard.IsKeyUp(key))
                return true;
            else
                return false;
        }
        static public bool KeyboardReleased(Keys key)
        {
            if (CurrentKeyboard.IsKeyUp(key) && PreviousKeyboard.IsKeyDown(key))
                return true;
            else
                return false;
        }
        static public bool KeyboardPress(Keys key)
        {
            if (CurrentKeyboard.IsKeyDown(key))
                return true;
            else
                return false;
        }
        static public bool KeyboardRelease(Keys key)
        {
            if (CurrentKeyboard.IsKeyUp(key))
                return true;
            else
                return false;
        }

        static public MouseState PreviousMouse;
        static public MouseState CurrentMouse;

        public enum EClicks
        {
            LEFT,
            MIDDLE,
            RIGHT
        }

        static public bool ClickPressed(EClicks click)
        {
            switch (click)
            {
                case EClicks.LEFT: return CurrentMouse.LeftButton == ButtonState.Pressed & PreviousMouse.LeftButton == ButtonState.Released;
                case EClicks.MIDDLE: return CurrentMouse.MiddleButton == ButtonState.Pressed & PreviousMouse.MiddleButton == ButtonState.Released;
                case EClicks.RIGHT: return CurrentMouse.RightButton == ButtonState.Pressed & PreviousMouse.RightButton == ButtonState.Released;
            }
            return false;
        }
        static public bool ClickPress(EClicks click)
        {
            switch (click)
            {
                case EClicks.LEFT: return CurrentMouse.LeftButton == ButtonState.Pressed;
                case EClicks.MIDDLE: return CurrentMouse.MiddleButton == ButtonState.Pressed;
                case EClicks.RIGHT: return CurrentMouse.RightButton == ButtonState.Pressed;
            }
            return false;
        }
        static public bool ClickReleased(EClicks click)
        {
            switch (click)
            {
                case EClicks.LEFT: return CurrentMouse.LeftButton == ButtonState.Released & PreviousMouse.LeftButton == ButtonState.Pressed;
                case EClicks.MIDDLE: return CurrentMouse.MiddleButton == ButtonState.Released & PreviousMouse.MiddleButton == ButtonState.Pressed;
                case EClicks.RIGHT: return CurrentMouse.RightButton == ButtonState.Released & PreviousMouse.RightButton == ButtonState.Pressed;
            }
            return false;
        }
        static public bool ClickRelease(EClicks click)
        {
            switch (click)
            {
                case EClicks.LEFT: return CurrentMouse.LeftButton == ButtonState.Released;
                case EClicks.MIDDLE: return CurrentMouse.MiddleButton == ButtonState.Released;
                case EClicks.RIGHT: return CurrentMouse.RightButton == ButtonState.Released;
            }
            return false;
        }

        static public void Begin()
        {
            CurrentKeyboard = Keyboard.GetState();
            CurrentMouse = Mouse.GetState();
        }

        static public void End()
        {
            PreviousKeyboard = CurrentKeyboard;
            PreviousMouse = CurrentMouse;
        }
    }
}
