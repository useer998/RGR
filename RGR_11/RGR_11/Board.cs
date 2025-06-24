// Board.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BattleshipGame
{
    public enum CellState
    {
        Empty,
        Ship,
        Hit,
        Miss
    }

    public enum GamePhase
    {
        Placing,
        InProgress,
        GameOver
    }

    public class Ship
    {
        public List<Point> Coordinates { get; }
        private HashSet<Point> hits;

        public Ship(IEnumerable<Point> coords)
        {
            Coordinates = new List<Point>(coords);
            hits = new HashSet<Point>();
        }

        public bool Contains(Point p)
        {
            return Coordinates.Contains(p);
        }

        public bool RegisterHit(Point p)
        {
            if (Contains(p) && !hits.Contains(p))
            {
                hits.Add(p);
                return true;
            }
            return false;
        }

        public bool IsSunk()
        {
            return hits.Count == Coordinates.Count;
        }
    }

    public class Board
    {
        public int Size { get; }
        public CellState[,] Grid { get; }
        private List<Ship> ships;

        public Board(int size)
        {
            Size = size;
            Grid = new CellState[size, size];
            ships = new List<Ship>();
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    Grid[x, y] = CellState.Empty;
        }

        public bool PlaceShip(int x, int y, int length, bool horizontal)
        {
            if (length <= 0)
                return false;

            var coords = new List<Point>();
            for (int i = 0; i < length; i++)
            {
                int cx = x + (horizontal ? i : 0);
                int cy = y + (horizontal ? 0 : i);
                if (cx < 0 || cx >= Size || cy < 0 || cy >= Size)
                    return false;
                coords.Add(new Point(cx, cy));
            }

            foreach (var pt in coords)
            {
                if (Grid[pt.X, pt.Y] != CellState.Empty)
                    return false;
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        int nx = pt.X + dx;
                        int ny = pt.Y + dy;
                        if (nx >= 0 && nx < Size && ny >= 0 && ny < Size)
                        {
                            if (Grid[nx, ny] == CellState.Ship)
                                return false;
                        }
                    }
                }
            }

            foreach (var pt in coords)
            {
                Grid[pt.X, pt.Y] = CellState.Ship;
            }
            ships.Add(new Ship(coords));
            return true;
        }

        public (bool hit, Ship sunkShip) ProcessShot(int x, int y)
        {
            if (x < 0 || x >= Size || y < 0 || y >= Size)
                return (false, null);
            if (Grid[x, y] == CellState.Hit || Grid[x, y] == CellState.Miss)
                return (false, null);

            Point p = new Point(x, y);
            foreach (var ship in ships)
            {
                if (ship.Contains(p))
                {
                    ship.RegisterHit(p);
                    Grid[x, y] = CellState.Hit;
                    if (ship.IsSunk())
                        return (true, ship);
                    return (true, null);
                }
            }

            Grid[x, y] = CellState.Miss;
            return (false, null);
        }

        public bool AllShipsSunk()
        {
            return ships.All(s => s.IsSunk());
        }
    }
}
