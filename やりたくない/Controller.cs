using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace やりたくない
{
    interface IControllable
    {
        Vector2 Location { get; set; }
        float LocationXComponent { get; set; }
        float LocationYComponent { get; set; }
        float HorizontalVelocity { get; set; }
        float VerticalVelocity { get; set; }
        Vector2 Velocity { get; set; }

        float Rotation { get; set; }
    }
    /// <summary>
    /// A controller is an entity that controls a unit. (NPC, projectile, player)
    /// </summary>
    abstract class Controller
    {
        public Controller(IControllable Controlled)
        {
            controlled = Controlled;
        }
        public Controller() : this(null) { }
        public IControllable Controlled
        {
            set { controlled = value; }
        }
        protected IControllable controlled;
        public abstract void update(GameTime gameTime);
    }
    class NoController : Controller
    {
        public new IControllable Controlled
        {
            set { }
        }
        public static NoController noController = new NoController(null);
        public NoController(IControllable controlled) : base(controlled) { }
        public override void update(GameTime gameTime) { }
    }
}
