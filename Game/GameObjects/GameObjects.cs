using System;

public abstract class GameObject
{
    public char Symbol { get; set; }
    public Vector Position { get; set; }

    public virtual void Render()
    { 
        Console.SetCursorPosition(Position.X, Position.Y);
        Console.Write(Symbol);
    }
}