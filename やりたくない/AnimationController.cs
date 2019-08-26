using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace やりたくない
{
    interface IAnimationControllable
    {
        /// <summary>
        /// runtime calculation of current frame from current animation and frame inside that
        /// </summary>
        uint CurrentFrame { get; }
        /// <summary>
        /// the set of animations that it has
        /// </summary>
        Animation[] Animations { get; }
        /// <summary>
        /// current animation
        /// </summary>
        uint CurrentAnimation { get; set; }
        /// <summary>
        /// rotation of the current object
        /// </summary>
        float Rotation { get; set; }
        SpriteEffects Effects { get; set; }
        Vector2 Scale { get; set; }
        float ScaleX { get; set; }
        float ScaleY { get; set; }

    }
    class AnimationController
    {
        public AnimationController(IAnimationControllable Controlled)
        {
            controlled = Controlled;
        }
        protected IAnimationControllable controlled;
        public void play(uint animationNumber)
        {
            controlled.CurrentAnimation = animationNumber;
        }
        public void pause()
        {
            controlled.Animations[controlled.CurrentAnimation].pause();
        }
        public virtual void update(GameTime gameTime)
        {
            controlled.Animations[controlled.CurrentAnimation].AnimateTick();
            //set effects
            controlled.Effects = controlled.Animations[controlled.CurrentAnimation].effects;
        }
    }
}
