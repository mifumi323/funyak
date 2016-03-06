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
}
