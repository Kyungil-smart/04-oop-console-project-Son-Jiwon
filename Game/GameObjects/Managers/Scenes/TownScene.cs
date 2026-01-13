using System;

public class TownScene : Scene
{
    private Tile[,] _field = new Tile[10, 20];
    private PlayerCharacter _player;
    private Enemy _enemy;
    private Vector _exitPos;

    public TownScene(PlayerCharacter player) => Init(player);

    public void Init(PlayerCharacter player)
    {
        _player = player;

        for (int y = 0; y < _field.GetLength(0); y++)
        {
            for (int x = 0; x < _field.GetLength(1); x++)
            {
                Vector pos = new Vector(x, y);
                _field[y, x] = new Tile(pos);
            }
        }
    }


    public override void Enter()
    {
        for (int y = 0; y < _field.GetLength(0); y++)
        {
            for (int x = 0; x < _field.GetLength(1); x++)
            {
                _field[y, x].OnTileObject = null;
            }
        }

        _player.Field = _field;
        _player.Position = new Vector(1, 2);
        _field[_player.Position.Y, _player.Position.X].OnTileObject = _player;

        Random rand = new Random();
        int mapHeight = _field.GetLength(0);
        int mapWidth = _field.GetLength(1);


        for (int i = 0; i < 15; i++)
        {

            int randomX = rand.Next(0, mapWidth);
            int randomY = rand.Next(2, mapHeight);


            if (randomX == 1 && randomY == 2) continue;

            if (_field[randomY, randomX].OnTileObject == null)
            {
                _field[randomY, randomX].OnTileObject = new Wall();
            }
        }


        for (int i = 0; i < 5; i++)
        {
            int randomX = rand.Next(0, mapWidth);
            int randomY = rand.Next(2, mapHeight);

            if (_field[randomY, randomX].OnTileObject == null)
            {
                _field[randomY, randomX].OnTileObject = new Potion { Name = $"포션" };
            }
        }

        int exitX = mapWidth - 1;
        int exitY = rand.Next(2, mapHeight);

        Exit newExit = new Exit();
        newExit.Position = new Vector(exitX, exitY);
        _field[exitY, exitX].OnTileObject = newExit;

        _exitPos = newExit.Position;

        _enemy = new Enemy(new Vector(15, 5), _field); 
        _field[5, 15].OnTileObject = _enemy;

    }

    public override void Update()
    {
        Vector startPos = _player.Position;
        _player.Update();
        _enemy.Update(_player);

        if (_player.Position.X == _enemy.Position.X && _player.Position.Y == _enemy.Position.Y)
        {
            ShowResultScreen(" 적에게 잡혔습니다! ", ConsoleColor.Red);
            return;
        }

        if (startPos.X != _player.Position.X || startPos.Y != _player.Position.Y)
        {
            _player.Health.Value -= 1;
        }

        if (_player.Health.Value <= 0)
        {
            ShowResultScreen(" GAME OVER... ", ConsoleColor.Red);
            return;
        }

        if (_player.Position.X == _exitPos.X && _player.Position.Y == _exitPos.Y)
        {
            ShowResultScreen("★ STAGE CLEAR! ★", ConsoleColor.Yellow);
        }
    }

    private void ShowResultScreen(string message, ConsoleColor color)
    {
        Console.Clear();
        Console.SetCursorPosition(10, 5);
        message.Print(color);
        Console.SetCursorPosition(10, 7);
        "다시 시작하시겠습니까? (Y / N)".Print();

        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Y)
            {
                _player.Health.Value = 30; 

                Console.Clear();
                Enter(); 
                return;
            }
            else if (keyInfo.Key == ConsoleKey.N)
            {
                Environment.Exit(0);
            }
        }
    }
    public override void Render()
    {
        PrintField();

        for (int y = 0; y < _field.GetLength(0); y++)
        {
            for (int x = 0; x < _field.GetLength(1); x++)
            {
                GameObject obj = _field[y, x].OnTileObject;

                if (obj != null && !(obj is PlayerCharacter))
                {
                    obj.Render();
                }
            }
        }
        _enemy.Render();
        _player.Render();
    }


    public override void Exit()
    {
        _field[_player.Position.Y, _player.Position.X].OnTileObject = null;
        _player.Field = null;
    }

    private void PrintField()
    {
        for (int y = 0; y < _field.GetLength(0); y++)
        {
            for (int x = 0; x < _field.GetLength(1); x++)
            {
                _field[y, x].Print();
            }
            Console.WriteLine();
        }
    }

}