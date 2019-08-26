using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;


namespace やりたくない
{
    internal static class Input
    {
        public static inputArrange arrange = new inputArrange()
        {
            left = Keys.A,
            right = Keys.D,
            up = Keys.W,
            down = Keys.S,
            pause = Keys.Escape,
            jump = Keys.Space,
            boost = Keys.C,
            c_click = Buttons.RightStick,
            c_pause = Buttons.Start,
            c_jump = Buttons.A,
            c_boost = Buttons.RightTrigger
        };
        /// <summary>
        /// impulse to left/right
        /// keyboard is 100% of impulse, controller is the value specified by value
        /// </summary>
        public static float LeftRightImpulse;
        public static float UpDownImpulse;
        public static bool pause, jump, boost;
        private static KeyboardState currentKeyboardState, pastKeyboardState;
        private static MouseState currentMouseState, pastMouseState;
        const int usualControllerIndex = 0;
        private static int controllerIndex = usualControllerIndex;
        private static GamePadState currentControllerState, pastControllerState;
        public static Vector2 mouseLocation;
        public static Vector2 mouseWorldspace => mouseLocation + Camera.location;
        public static Rectangle mouseTip;
        public static Vector2 mouseDelta;
        public static bool mouseMoved, mouseMainButton, mouseSecondaryButton, mouseMiddleButton;
        public static bool mouseMainButtonReleased;
        //psuedo-mouse: controller-based mouse.  Is not the actual mouse, because why not have 2 mice...
        public static Vector2 pseudoMouseLocation = Main.center;
        public static Vector2 pseudoMouseWorlspace => pseudoMouseLocation + Camera.location;
        public static Rectangle pseudoMouseTip;
        public static Vector2 pseudoMouseDelta;
        public static float pseudoMouseSpeed = 10f;
        public static bool pseudoMouseClicked;
        public static bool pseudoMouseReleased;

        private static bool isControllerConnected;
        private static Vector2 pseudoMouseSaveLocation = Main.center;//save location is in center, so if a controller is connected it moves there.

        public static event yaritakunaiEventHandler controllerConnected = onControllerConnect;
        public static event yaritakunaiEventHandler controlledDisconnected = onControllerDisconnect;

        public static void init()
        {
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            currentControllerState = GamePad.GetState(controllerIndex);
            isControllerConnected = currentControllerState.IsConnected;
            if (isControllerConnected)
                controllerConnected();
            else
                controlledDisconnected();
        }
        public static void update()
        {

            pastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            pastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            pastControllerState = currentControllerState;
            currentControllerState = GamePad.GetState(controllerIndex);
            //Get impulses from controller. they will be 0 if no controller, in which case it is reset.
            LeftRightImpulse = currentControllerState.ThumbSticks.Left.X;
            UpDownImpulse = -currentControllerState.ThumbSticks.Left.Y;//negative because the controller has up/down be the opposite of the window's up/down. Sigh.

            //left & right
            if (currentKeyboardState.IsKeyDown(arrange.left))
                LeftRightImpulse = -1;
            if (currentKeyboardState.IsKeyDown(arrange.right))
                LeftRightImpulse = 1;
            //up down.
            if (currentKeyboardState.IsKeyDown(arrange.up))
                UpDownImpulse = -1;
            if (currentKeyboardState.IsKeyDown(arrange.down))
                UpDownImpulse = 1;
            //jump.
            jump = currentKeyboardState.IsKeyDown(arrange.jump)
                || currentControllerState.IsButtonDown(arrange.c_jump);
            //pause button.
            pause = (currentKeyboardState.IsKeyDown(arrange.pause) && pastKeyboardState.IsKeyUp(arrange.pause))
                || (currentControllerState.IsButtonDown(arrange.c_pause) && pastControllerState.IsButtonUp(arrange.c_pause));
            boost = currentKeyboardState.IsKeyDown(arrange.boost) || currentControllerState.IsButtonDown(arrange.c_boost);
            //mouse stuff.
            mouseDelta = new Vector2(currentMouseState.X - mouseLocation.X, currentMouseState.Y - mouseLocation.Y);
            mouseLocation = new Vector2(currentMouseState.X, currentMouseState.Y);
            mouseTip = new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1);
            mouseMoved = currentMouseState.X != pastMouseState.X || currentMouseState.Y != pastMouseState.Y;
            mouseMainButton = currentMouseState.LeftButton == ButtonState.Pressed && pastMouseState.LeftButton == ButtonState.Released;
            mouseMainButtonReleased = currentMouseState.LeftButton == ButtonState.Released && pastMouseState.LeftButton == ButtonState.Pressed;
            mouseMiddleButton = currentMouseState.MiddleButton == ButtonState.Pressed && pastMouseState.MiddleButton == ButtonState.Released;
            mouseSecondaryButton = currentMouseState.RightButton == ButtonState.Pressed && pastMouseState.RightButton == ButtonState.Released;

            //pseudo mouse stuff, but only if there's a controller
            if (currentControllerState.IsConnected)
            {
                pseudoMouseDelta = currentControllerState.ThumbSticks.Right * pseudoMouseSpeed;
                pseudoMouseDelta.Y *= -1;//invert axis...


                //Check if any of the delta changes set the position over threshold, if so, reduce delta.
                if (pseudoMouseLocation.X + pseudoMouseDelta.X < 0)
                {
                    pseudoMouseDelta.X -= pseudoMouseLocation.X + pseudoMouseDelta.X;
                }
                else if (pseudoMouseLocation.X + pseudoMouseDelta.X > Main.zeroedWindow.Width - 2)
                {
                    pseudoMouseDelta.X -= pseudoMouseDelta.X + pseudoMouseLocation.X - Main.zeroedWindow.Width + 2;
                }

                if (pseudoMouseLocation.Y + pseudoMouseDelta.Y < 0)
                {
                    pseudoMouseDelta.Y -= pseudoMouseLocation.Y + pseudoMouseDelta.Y;
                }
                else if (pseudoMouseLocation.Y + pseudoMouseDelta.Y > Main.zeroedWindow.Height - 2)
                {
                    pseudoMouseDelta.Y -= pseudoMouseDelta.Y + pseudoMouseLocation.Y - Main.zeroedWindow.Height + 2;
                }

                //modify location based on delta.
                pseudoMouseLocation += pseudoMouseDelta;

                //update tip
                pseudoMouseTip = new Rectangle((int)pseudoMouseLocation.X, (int)pseudoMouseLocation.Y, 1, 1);
                //buttons
                pseudoMouseClicked = currentControllerState.IsButtonDown(arrange.c_click) && pastControllerState.IsButtonUp(arrange.c_click);
                pseudoMouseReleased = currentControllerState.IsButtonUp(arrange.c_click) && pastControllerState.IsButtonDown(arrange.c_click);
            }

            if (isControllerConnected && !currentControllerState.IsConnected)
            {
                isControllerConnected = false;
                controlledDisconnected();
            }
            if (!isControllerConnected && currentControllerState.IsConnected)
            {
                isControllerConnected = true;
                controllerConnected();
            }




        }

        private static void onControllerConnect()
        {
            //recall the saved location
            pseudoMouseLocation = pseudoMouseSaveLocation;
            pseudoMouseTip = new Rectangle((int)pseudoMouseLocation.X, (int)pseudoMouseLocation.Y, 1, 1);
            Main.isPsuedoCursorDrawn = true;

        }
        private static void onControllerDisconnect()
        {
            //save pseudo-mouse location and then purge the mouse.
            pseudoMouseSaveLocation = pseudoMouseLocation;
            pseudoMouseLocation = new Vector2(-1000, -1000);
            pseudoMouseTip = new Rectangle((int)pseudoMouseLocation.X, (int)pseudoMouseLocation.Y, 1, 1);
            pseudoMouseDelta = Vector2.Zero;
            Main.isPsuedoCursorDrawn = false;
        }

    }
    internal struct inputArrange
    {
        public Keys left, right, up, down, pause, jump, boost;
        public Buttons c_click, c_pause, c_jump, c_boost;

    }
}
