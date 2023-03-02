using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong_inari.engine
{
    interface IGameObject : IDisposable
    {
        string Name { get; }
        (int x, int y) Position { get; set; }
    }
}
