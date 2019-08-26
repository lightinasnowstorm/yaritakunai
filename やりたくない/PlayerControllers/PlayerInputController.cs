using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace やりたくない.PlayerControllers
{
    class PlayerInputController : Controller
    {
        public static bool inputBlockFromTextbox;
        public Player internalPlayer
        {
            set { controlled = value; }
        }
        public PlayerInputController() : base(null)
        {

        }
        public PlayerInputController(IControllable p) : base(p)
        {
        }

        const int speed = 5;
        public override void update(GameTime gameTime)
        {
            if (inputBlockFromTextbox)
            {
                controlled.Velocity = Vector2.Zero;
                return;
            }
            controlled.HorizontalVelocity = (int)(speed * Input.LeftRightImpulse);
            controlled.VerticalVelocity = 0;
            if (Input.jump)
                controlled.VerticalVelocity = -20;
            if (Input.boost)
                controlled.HorizontalVelocity *= 10;
        }
    }
}
