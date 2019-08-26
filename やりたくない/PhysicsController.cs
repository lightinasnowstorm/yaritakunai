using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace やりたくない
{
    interface IPhysicsControllable
    {
        /// <summary>
        /// velocity is s'
        /// also important
        /// negative is left
        /// positive is right
        /// </summary>
        float HorizontalVelocity { get; set; }
        /// <summary>
        /// like velocity but for updown
        /// down is +
        /// up is -
        /// 
        /// because that's how computers do it
        /// </summary>
        float VerticalVelocity { get; set; }

        Vector2 Velocity { get; set; }
        /// <summary>
        /// the location of the object
        /// </summary>
        Vector2 Location { get; set; }
        /// <summary>
        /// the size of the object's hitbox
        /// relative to the location of the object
        /// </summary>
        Rectangle Hitbox { get; }
        /// <summary>
        /// hitbox in relation to the world
        /// </summary>
        Rectangle ActualHitbox { get; }
        //other stuff which I copied from IControllable
        float LocationXComponent { get; set; }
        float LocationYComponent { get; set; }
        bool falls { get; }
        bool terrainColliding { get; }
        void hitGround(float verticalVelocity);
    }
    /// <summary>
    /// class to control physics of an object
    /// </summary>
    class PhysicsController
    {
        IPhysicsControllable controlled;
        int gravVelocity = 0;
        const int gravitationalConstant = 1;//Pixels/Update^2
        const double _60fpsdtms = 16.6667;//1000 / 60; //16.6667 is dt at 60fps

        public PhysicsController(IPhysicsControllable Controlled)
        {
            controlled = Controlled;
        }

        public void update(GameTime gameTime)
        {
            float deltaTime = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / _60fpsdtms);


            //velocity and hitbox checking I guess


            //this does a quadratic fall behaviour
            if (controlled.falls)
            {
                controlled.VerticalVelocity += gravVelocity;
                gravVelocity += gravitationalConstant;
            }
            //apply dt after gravity
            controlled.Velocity *= deltaTime;

            //If the controlled object can collide with terrain, we check if it does.
            if (controlled.terrainColliding)
            {
                checkVelocityCausesCollisionsAndCorrect();


            }

            //move controlled object by velocity
            controlled.Location += controlled.Velocity;

        }

        public void checkVelocityCausesCollisionsAndCorrect()
        {
            //Calculate the location of the first block in all directions, if it exists.
            //TODO: generic hit-the-ground
            //*collision
            //instead of this

            /* A few approaches:
             * 1)Check for the closest block in all directions, and prevent clipping through that.
             * 2)Check the ending position for whether or not it is inside a block, as well as
             *    ->Branching based on total velocity, e.g. check end and halfway there, each of those halfway points, etc.
             * 3)Make a line from the beginning to ending, and check if within a certain distance or so there are blocks.
             * 4)Make a line from each corner of the hitbox, and make sure either that the lines hit nothing or that there's nothing between them.
             */

            //A supplement to that:
            //If the player is *inside* a block, push them up and out or not?

            //I am choosing to use the branching rectangles approach.
            //find the rectangle for the hitbox
            Rectangle thingimabob = controlled.ActualHitbox;


            //The current problem is that the rectangle is able to go inside blocks. that causes large amounts of problems
            bool startsInsideBlock = Main.MainWorld.isSolidBlockInRectangle(controlled.ActualHitbox);
            //Ostensibly, this would be based on the magnitude of the velocity
            float pieces = 8;
            float percentDelta = 1f / pieces;
            for (int numPieces = 0; numPieces < pieces; numPieces++)
            {
                //move rectangle by a certain distance.
                thingimabob.Offset(percentDelta * controlled.Velocity.X, percentDelta * controlled.Velocity.Y);
                //check if that has blocks in it.
                if (Main.MainWorld.isSolidBlockInRectangle(thingimabob))
                {
                    //problem: Despite component blaming and proper anti-block-falling-into mechanisms, blocks can still be fallen into.

                    //component blaming.
                    bool blameX = false;
                    bool blameY = false;
                    Rectangle lastOffsetRectangle = controlled.ActualHitbox;
                    lastOffsetRectangle.Offset(numPieces * percentDelta * controlled.Velocity);
                    //theoretically this rectangle should not be colliding with any blocks.

                    //make rectangles corresponding to the X and Y components of the last change vector
                    Rectangle thisOffsetYRectangle = lastOffsetRectangle;
                    Rectangle thisOffsetXRectangle = lastOffsetRectangle;
                    thisOffsetYRectangle.Offset(0, percentDelta * controlled.VerticalVelocity);
                    thisOffsetXRectangle.Offset(percentDelta * controlled.HorizontalVelocity, 0);

                    //despite having the same rectangle it blames only x
                    blameX = Main.MainWorld.isSolidBlockInRectangle(thisOffsetXRectangle);
                    blameY = Main.MainWorld.isSolidBlockInRectangle(thisOffsetYRectangle);

                    //not good
                    blameX &= !startsInsideBlock;
                    //blameY &= !startsInsideBlock && controlled.VerticalVelocity < 0;

                    //Two approaches here to stopping the velocity: previous-block and previous-frame
                   //Try mix and match for directions?

                    if (blameX && blameY)
                    {
                        //stop them both
                        controlled.Velocity = controlled.Velocity * percentDelta * numPieces;
                        gravVelocity = 0;


                    }
                    else if (blameX)
                    {
                        controlled.HorizontalVelocity = controlled.HorizontalVelocity * percentDelta * numPieces;
                    }
                    else if (blameY)
                    {
                        gravVelocity = 0;
                        controlled.VerticalVelocity = controlled.VerticalVelocity * percentDelta * numPieces;
                    }
                    else
                    {
                        gravVelocity = 0;
                        controlled.Velocity = controlled.Velocity * percentDelta * numPieces;
                    }
                    //if it is inside a block after this, move up



                    //unset gravity if Y is going down. Only. Or else colliding with a ceiling resets gravity, and that's not good physics.
                    //controlled.VerticalVelocity = (float)Math.Floor(velocity.Y * percentDelta * numPieces);
                    //controlled.HorizontalVelocity = (float)Math.Floor(velocity.X * percentDelta * numPieces);
                    //don't need to continue the loop if we know we hit.

                    //set to the nearest block-size for the edge of the rectangle that is colliding?
                    //sounds sensible.

                    //controlled.Velocity = controlled.Velocity * percentDelta * numPieces;
                    break;
                }


            }
            //if it doesn't collide, don't need to modify the velocity vector
        }
    }
}
