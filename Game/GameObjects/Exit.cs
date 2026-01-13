using System;

public class Exit : GameObject
{
    public Exit()
    {
        Symbol = 'E'; 
    }

    public override void Render()
    {
        
        Console.SetCursorPosition(Position.X, Position.Y);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write('E');
        Console.ResetColor();
    }
}