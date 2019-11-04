namespace MifuminSoft.funyak.Collision
{
    public interface IColliderPositionUpdater
    {
        void Update(ColliderBase collider, double oldLeft, double oldTop, double oldRight, double oldBottom, double newLeft, double newTop, double newRight, double newBottom);
    }
}
