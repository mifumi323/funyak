using System;
using System.Collections.Generic;
using System.Text;

namespace MifuminSoft.funyak.Collision
{
    public interface IColliderUpdatePositionListener
    {
        void ColliderPositionUpdated(ColliderBase collider);
    }
}
