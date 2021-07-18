using System.Collections.Generic;

namespace MifuminSoft.funyak.Collision
{
    public class ColliderCollection
    {
        private readonly List<RegionCollider> regionColliders = new List<RegionCollider>();
        private readonly List<PointCollider> pointColliders = new List<PointCollider>();
        private readonly List<PlateCollider> plateColliders = new List<PlateCollider>();
        private readonly List<NeedleCollider> needleColliders = new List<NeedleCollider>();

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
                    if (regionCollider.Owner != pointCollider.Owner)
                    {
                        if ((regionCollider.RegionInfo.Flags & pointCollider.Reactivities) != RegionAttributeFlag.None)
                        {
                            if (regionCollider.Contains(pointCollider, out var collision))
                            {
                                regionCollider.OnCollided?.Invoke(ref collision);
                                pointCollider.OnCollided?.Invoke(ref collision);
                            }
                        }
                    }
                }
            }
            foreach (var plateCollider in plateColliders)
            {
                foreach (var needleCollider in needleColliders)
                {
                    if (plateCollider.Owner != needleCollider.Owner)
                    {
                        if ((plateCollider.PlateInfo.Flags & needleCollider.Reactivities) != PlateAttributeFlag.None)
                        {
                            if (plateCollider.IsCollided(needleCollider, out var collision))
                            {
                                plateCollider.OnCollided?.Invoke(ref collision);
                                needleCollider.OnCollided?.Invoke(ref collision);
                            }
                        }
                    }
                }
            }
        }
    }
}
