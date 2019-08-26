using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace やりたくない.PlayerControllers
{
    class PlayerAnimationController : AnimationController
    {
        public PlayerAnimationController(Player Controlled) : base(Controlled)
        {
            //it does a thing
        }
        public override void update(GameTime gameTime)
        {
            //update the direction facing based on PHYSICS
            //(eg it doesn't belong to the input (which is EXCELLENt compartmentalization)
            //but we still need a universal "game object" class that everything inherits (that is IPhysicsControllable, because everything in the game is physics-controlled.)
            //We can actually be certain in this instance that controlled is a player, for *obvious reasons*
            if ((controlled as IPhysicsControllable).HorizontalVelocity > 0)
            {
                play((uint)PlayerAnimation.right);
            }
            else if ((controlled as IPhysicsControllable).HorizontalVelocity < 0)
            {
                play((uint)PlayerAnimation.left);
            }

            base.update(gameTime);
        }
    }
}
