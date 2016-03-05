namespace MifuminSoft.funyak.Core
{
    public class Map
    {
        public double Width { get; protected set; }
        public double Height { get; protected set; }

        public Map(double width, double height)
        {
            Width = width;
            Height = height;
        }
    }
}
