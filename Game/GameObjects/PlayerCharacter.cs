
using System;

public class PlayerCharacter : GameObject
{
    public ObservableProperty<int> Health = new ObservableProperty<int>(30);
    private string _healthGauge;
    
    public Tile[,] Field { get; set; }
    private Inventory _inventory;
    public bool IsActiveControl { get; private set; }

    public PlayerCharacter() => Init();

    public void Init()
    {
        Symbol = 'P';
        IsActiveControl = true;
        Health.AddListener(SetHealthGauge);
        _healthGauge = "■■■■■■■■■■";
        _inventory = new Inventory(this);
    }

    public void Update()
    {
        if (InputManager.GetKey(ConsoleKey.I))
        {
            HandleControl();
        }
        
        if (InputManager.GetKey(ConsoleKey.UpArrow))
        {
            Move(Vector.Up);
            _inventory.SelectUp();
        }

        if (InputManager.GetKey(ConsoleKey.DownArrow))
        {
            Move(Vector.Down);
            _inventory.SelectDown();
        }

        if (InputManager.GetKey(ConsoleKey.LeftArrow))
        {
            Move(Vector.Left);
        }

        if (InputManager.GetKey(ConsoleKey.RightArrow))
        {
            Move(Vector.Right);
        }

        if (InputManager.GetKey(ConsoleKey.Enter))
        {
            _inventory.Select();
        }

        if (InputManager.GetKey(ConsoleKey.T))
        {
            Health.Value--;
        }
    }

    public void HandleControl()
    {
        _inventory.IsActive = !_inventory.IsActive;
        IsActiveControl = !_inventory.IsActive;
        Debug.LogWarning($"{_inventory._itemMenu.CurrentIndex}");
    }

    private void Move(Vector direction)
    {
        if (Field == null) return; 

        Vector nextPos = Position + direction;

        if (nextPos.X < 0 || nextPos.X >= Field.GetLength(1) || nextPos.Y < 0 || nextPos.Y >= Field.GetLength(0)) return;

        GameObject nextTileObject = Field[nextPos.Y, nextPos.X].OnTileObject;

        if (nextTileObject != null)
        {
            if (nextTileObject is IInteractable)
            {
                (nextTileObject as IInteractable).Interact(this);
            }

            if (nextTileObject is Wall) return;
        }

        if (!IsActiveControl) return;

        Field[Position.Y, Position.X].OnTileObject = null;
        Field[nextPos.Y, nextPos.X].OnTileObject = this;
        Position = nextPos;
    }

    public override void Render()
    {
        DrawHealthGauge();
        _inventory.Render();

        Console.SetCursorPosition(Position.X, Position.Y);
        "P".Print(ConsoleColor.Cyan);
    }

    public void AddItem(Item item)
    {
        _inventory.Add(item);
    }

    public void DrawHealthGauge()
    {
        Console.SetCursorPosition(0, 0);
        Console.Write("HP: ");
        _healthGauge.Print(ConsoleColor.Red);
    }

    public void SetHealthGauge(int health)
    {
        _healthGauge = "";
        for (int i = 0; i < health; i++)
          {
            _healthGauge += "■";
          }
        }

    public void Heal(int value)
    {
        Health.Value += value;
    }
}