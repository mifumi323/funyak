namespace MifuminSoft.funyak.MapObject
{
    public sealed class RealizeCollisionArgs
    {
        private readonly Map map;

        public RealizeCollisionArgs(Map map) => this.map = map;

        public double MapWidth => map.Width;
        public double MapHeight => map.Height;
    }
}
