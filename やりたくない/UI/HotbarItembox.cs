using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace やりたくない.UI
{
    class HotbarItembox : Itembox
    {
        public HotbarItembox(Holder<Item> holder) : base(holder, true)
        {

        }
    }
}
