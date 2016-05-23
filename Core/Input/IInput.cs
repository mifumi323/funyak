namespace MifuminSoft.funyak.Core.Input
{
    public interface IInput
    {
        double X { get; }
        double Y { get; }

        bool IsPressed(Keys key);
        bool IsPushed(Keys key);
        bool IsReleased(Keys key);

        void Update();
    }

    public enum Keys : int
    {
        Left = 0,
        Up = 1,
        Right = 2,
        Down = 3,
        Jump = 4,
        Attack = 5,
        Smile = 6,
    }
}
