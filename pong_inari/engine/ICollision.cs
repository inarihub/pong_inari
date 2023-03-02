using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong_inari.engine
{
    interface ICollision
    {
        Task WasCollided();
    }
}
