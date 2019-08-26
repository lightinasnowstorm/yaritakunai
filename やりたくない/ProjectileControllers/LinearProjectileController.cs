using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace やりたくない.ProjectileControllers
{
    class LinearProjectileController : Controller
    {
        int xVelocity;
        int yVelocity;
        public LinearProjectileController(int XVelocity, int YVelocity) : this(null, XVelocity, YVelocity) { }
        public LinearProjectileController(IControllable controlled, int XVelocity, int YVelocity) : base(controlled)
        {
            xVelocity = XVelocity;
            yVelocity = YVelocity;
        }

        public override void update(GameTime gameTime)
        {
            controlled.HorizontalVelocity = xVelocity;
            controlled.VerticalVelocity = yVelocity;
        }
    }
}
