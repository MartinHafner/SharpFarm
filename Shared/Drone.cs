namespace SharpFarm.Shared;

public class Drone {
    public int X { get; private set; }
    public int Y { get; private set; }
    public string Name { get; set; }

    public Drone(int x, int y, string name = "Drone") {
        X = x; Y = y; Name = name;
    }

    public void Move(string dir, GameWorld world) {
        var (dx, dy) = dir switch {
            "up" => (0, -1),
            "down" => (0, 1),
            "left" => (-1, 0),
            "right" => (1, 0),
            _ => (0, 0)
        };

        int nx = X + dx, ny = Y + dy;
        if (world.InBounds(nx, ny)) { X = nx; Y = ny; }
    }

    public Cell Sense(GameWorld world) => world.Grid[X, Y];

    public void Plant(GameWorld world) {
        var cell = world.Grid[X, Y];
        if (cell.Type == CellType.Empty && !cell.IsPlanted)
            cell.IsPlanted = true;
    }

    public void Harvest(GameWorld world) {
        var cell = world.Grid[X, Y];
        if (cell.IsPlanted)
            cell.IsHarvested = true;
    }
}
