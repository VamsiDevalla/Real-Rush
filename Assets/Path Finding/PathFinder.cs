using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    [SerializeField] Vector2Int destinationCoordinates;

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
        if (gridManager != null) grid = gridManager.Grid;
    }

    void Start()
    {
        startNode = gridManager.Grid[startCoordinates];
        destinationNode = gridManager.Grid[destinationCoordinates];
        BreadthFirstSearch();
        BuildPath();
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

    void BreadthFirstSearch()
    {
        bool destFound = false;
        nodesToExplore.Enqueue(startNode);
        reached.Add(startCoordinates, startNode);

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

}
