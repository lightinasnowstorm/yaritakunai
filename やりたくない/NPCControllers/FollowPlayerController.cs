using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace やりたくない.NPCControllers
{
    class FollowPlayerController : Controller
    {
        public FollowPlayerController() : base() { }

        public FollowPlayerController(NPC Controlled) : base(Controlled)
        {

        }
        Random aiRand = new Random();
        public override void update(GameTime gameTime)
        {
            float xdiffFromPlayer = Main.currentPlayer.Location.X - controlled.LocationXComponent;
            if (Math.Abs(xdiffFromPlayer) > 60)
                controlled.HorizontalVelocity = (int)(xdiffFromPlayer * .03f * aiRand.Next(30, 50) / 50f);
            else
                controlled.HorizontalVelocity = 0;
        }
    }
}
