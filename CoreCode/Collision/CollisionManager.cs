using System.Collections.Generic;

namespace MifuminSoft.funyak.Collision
{
    public class CollisionManager
    {
        private List<RegionCollider> regionColliders = new List<RegionCollider>();
        private List<PointCollider> pointColliders = new List<PointCollider>();
        private List<PlateCollider> plateColliders = new List<PlateCollider>();
        private List<NeedleCollider> needleColliders = new List<NeedleCollider>();

        public void Add(RegionCollider collider) => regionColliders.Add(collider);
        public void Add(PointCollider collider) => pointColliders.Add(collider);
        public void Add(PlateCollider collider) => plateColliders.Add(collider);
        public void Add(NeedleCollider collider) => needleColliders.Add(collider);

        public void Remove(RegionCollider collider) => regionColliders.Remove(collider);
        public void Remove(PointCollider collider) => pointColliders.Remove(collider);
        public void Remove(PlateCollider collider) => plateColliders.Remove(collider);
        public void Remove(NeedleCollider collider) => needleColliders.Remove(collider);

        public void Collide()
        {
            foreach (var regionCollider in regionColliders)
            {
                foreach (var pointCollider in pointColliders)
                {
                    if (regionCollider.Owner != pointCollider.Owner && regionCollider.Contains(pointCollider))
                    {
                        regionCollider.Owner.OnCollided(regionCollider, pointCollider);
                        pointCollider.Owner.OnCollided(pointCollider, regionCollider);
                    }
                }
            }
            foreach (var plateCollider in plateColliders)
            {
                foreach (var needleCollider in needleColliders)
                {
                    if (plateCollider.Owner != needleCollider.Owner)
                    {
                        var collision = plateCollider.GetCollision(needleCollider);
                        if (collision.IsCollided)
                        {
                            plateCollider.Owner.OnCollided(plateCollider, needleCollider, collision.CrossPoint);
                            needleCollider.Owner.OnCollided(needleCollider, plateCollider, collision.CrossPoint);
                        }
                    }
                }
            }
        }
    }
}
