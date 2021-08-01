using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }

    [SerializeField] Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();
    Queue<Node> nodesToExplore = new Queue<Node>();

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };

    GridManager gridManager;

    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null)
        {
            grid = gridManager.Grid;
            startNode = grid[startCoordinates];
            destinationNode = grid[destinationCoordinates];
        }
    }

    void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int coords)
    {
        gridManager.ResetNodes();
        BreadthFirstSearch(coords);
        return BuildPath();
    }

    void ExploreNeighbors()
    {
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborCoords = currentSearchNode.coordinates + direction;
            if (grid.ContainsKey(neighborCoords))
            {
                Node neighbor = grid[neighborCoords];
                if (!reached.ContainsKey(neighborCoords) && neighbor.isWalkable)
                {
                    neighbor.connectedTo = currentSearchNode;
                    reached.Add(neighborCoords, neighbor);
                    nodesToExplore.Enqueue(neighbor);
                }
            }
        }
    }

    void BreadthFirstSearch(Vector2Int coords)
    {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        nodesToExplore.Clear();
        reached.Clear();

        bool destFound = false;
        nodesToExplore.Enqueue(grid[coords]);
        reached.Add(coords, grid[coords]);

        while (nodesToExplore.Count > 0 && !destFound)
        {
            currentSearchNode = nodesToExplore.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbors();
            if (currentSearchNode.coordinates == destinationCoordinates)
            {
                destFound = true;
            }

        }
    }

    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;
        path.Add(currentNode);
        currentNode.inPath = true;

        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.inPath = true;
        }

        path.Reverse();

        return path;
    }

    public bool WillBlockPath(Vector2Int coords)
    {
        if (grid.ContainsKey(coords))
        {
            bool prevState = grid[coords].isWalkable;
            grid[coords].isWalkable = false;
            List<Node> newPath = GetNewPath();
            grid[coords].isWalkable = prevState;

            if (newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }

        return false;
    }

    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }

}
