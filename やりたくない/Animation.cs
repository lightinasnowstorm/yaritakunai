using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace やりたくない
{
    class Animation
    {
        public Animation(uint StartFrame, uint EndFrame, bool Loops = false, SpriteEffects Effects = SpriteEffects.None)
        {
            startFrame = StartFrame;
            endFrame = EndFrame;
            effects = Effects;
            loops = Loops;
        }
        public Animation(uint StartFrame, uint EndFrame, SpriteEffects Effects = SpriteEffects.None, bool Loops = false)
        {
            startFrame = StartFrame;
            endFrame = EndFrame;
            effects = Effects;
            loops = Loops;
        }
        /// <summary>
        /// SpriteEffects applied to the animation.
        /// </summary>
        public SpriteEffects effects;
        /// <summary>
        /// which frame in spritesheet it starts on
        /// </summary>
        public uint startFrame;
        /// <summary>
        /// frame on spritesheet it ends on
        /// </summary>
        public uint endFrame;
        /// <summary>
        /// the current frame of the animation
        /// </summary>
        public uint currentFrame;
        /// <summary>
        /// whether or not the animation loops
        /// </summary>
        public bool loops;
        public bool paused = true;
        public void pause()
        {
            paused = true;
        }
        public void play()
        {
            paused = false;
        }
        public void start()
        {
            paused = false;
            currentFrame = startFrame;
        }
        /// <summary>
        /// increases the frame and loops if necessary.
        /// Animates one tick.
        /// </summary>
        public void AnimateTick()
        {
            if (!paused)
            {
                if (currentFrame < endFrame)
                    currentFrame++;
                else if (currentFrame == endFrame && loops)
                    currentFrame = startFrame;
            }

        }
    }
}
