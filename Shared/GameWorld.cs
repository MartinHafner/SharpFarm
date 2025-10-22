namespace SharpFarm.Shared;
using System.Collections.Generic;

public enum CellType { Empty, Crop, Rock, Water }

public class Cell {
    public CellType Type { get; set; } = CellType.Empty;
    public bool IsPlanted { get; set; }
    public bool IsHarvested { get; set; }
}

public class GameWorld
{
    public static GameWorld Instance { get; } = new GameWorld();

    public int Width { get; } = 10;
    public int Height { get; } = 10;
    public Cell[,] Grid { get; }
    public List<Drone> Drones { get; } = new List<Drone>();

    public GameWorld()
    {
        Grid = new Cell[Width, Height];
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                Grid[x, y] = new Cell();
    }

    public void SpawnDrone(int x, int y) => Drones.Add(new Drone { X = x, Y = y });
}

    public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;
}
