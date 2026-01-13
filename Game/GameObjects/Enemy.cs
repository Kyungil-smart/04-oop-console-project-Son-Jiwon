using System;

public class Enemy : GameObject
{
    private Random _rand = new Random();
    public Tile[,] Field { get; set; }
    private int _moveDelay = 0;

    public Enemy(Vector startPos, Tile[,] field)
    {
        Symbol = 'X';
        Position = startPos;
        Field = field;
    }

    public void Update(PlayerCharacter player)
    {
        
        if (++_moveDelay < 3) return;
        _moveDelay = 0;

        Vector[] directions = { Vector.Up, Vector.Down, Vector.Left, Vector.Right };
        Vector moveDir = directions[_rand.Next(4)];
        Vector nextPos = Position + moveDir;

       
        if (nextPos.X < 0 || nextPos.X >= Field.GetLength(1) || nextPos.Y < 0 || nextPos.Y >= Field.GetLength(0)) return;
        if (Field[nextPos.Y, nextPos.X].OnTileObject is Wall) return;

       
        Field[Position.Y, Position.X].OnTileObject = null;
        Position = nextPos;

        
        if (Position.X == player.Position.X && Position.Y == player.Position.Y)
        {
            player.Health.Value = 0; 
            return;
        }

        Field[Position.Y, Position.X].OnTileObject = this;
    }

    public override void Render()
    {
        Console.SetCursorPosition(Position.X, Position.Y);
        "X".Print(ConsoleColor.Red); 
    }
}