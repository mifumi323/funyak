namespace MifuminSoft.funyak.Core.Input
{
    public interface IInput
    {
        double X { get; }
        double Y { get; }

        bool IsPressed(int key);
        bool IsPushed(int key);
        bool IsReleased(int key);

        bool Update();
    }

    public enum Keys : int
    {
        Left = 1,
        Up = 2,
        Right = 3,
        Down = 4,
        Jump = 5,
        Attack = 6,
        Smile = 7,
    }
}
