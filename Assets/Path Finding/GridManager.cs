using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    [Tooltip("should unity editor snap settings")]
    [SerializeField] int unityGridSize = 10;
    public int UnityGridSize { get { return unityGridSize; } }

    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid { get { return grid; } }

    void Awake()
    {
        CreateGrid();
    }

    public Node GetNode(Vector2Int key)
    {
        if (!grid.ContainsKey(key)) return null;
        return grid[key];
    }

    public void BlockNode(Vector2Int coords)
    {
        if (grid.ContainsKey(coords))
        {
            grid[coords].isWalkable = false;
        }
    }

    public void ResetNodes()
    {
        foreach(KeyValuePair<Vector2Int, Node> entry in grid)
        {
            entry.Value.connectedTo = null;
            entry.Value.isExplored = false;
            entry.Value.inPath = false;
        }
    }

    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();
        coordinates.x = Mathf.RoundToInt(position.x / unityGridSize);
        coordinates.y = Mathf.RoundToInt(position.z / unityGridSize);
        return coordinates;
    }

    public Vector3 GetPositionFromCoordinates(Vector2Int coords)
    {
        Vector3 position = new Vector3();
        position.x = coords.x * unityGridSize;
        position.z = coords.y * unityGridSize;
        return position;
    }

    void CreateGrid()
    {
        for(int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new Node(coordinates, true));
            }
        }
    }
}
