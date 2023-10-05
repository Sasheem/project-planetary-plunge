using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    [SerializeField] Vector2Int destinationCoordinates;
    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();       // keep track of explored nodes

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    void Awake() {
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null) {
            grid = gridManager.Grid;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        startNode = gridManager.Grid[startCoordinates];
        destinationNode = gridManager.Grid[destinationCoordinates];

        BreadthFirstSearch();
        BuildPath();
    }

    void ExploreNeighbors() {
        // 1 create an empty list called 'neighhbors'
        List<Node> neighbors = new List<Node>();
        // 2 loop through all for directions
        foreach (Vector2Int direction in directions) {
            // 3 calculate the coordinates of the node in that direction from our currentSearchNode
            Vector2Int neighborCoods = currentSearchNode.coordinates + direction;

            // 4 check if the neighbor's coordinates exist in the grid
            if (grid.ContainsKey(neighborCoods)) {
                // 5 if it does exist in the grid, add it to our neighbors list
                neighbors.Add(grid[neighborCoods]);
            }
        }

        // loop through neighbors
        foreach (Node neighbor in neighbors) {
            // check if Dictionary of reached nodes has this current neighbor
            // check if neighbor is walkable
            if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable) {
                // creates the connection between nodes to build tree
                neighbor.connectedTo = currentSearchNode;
                // keep track of what we've reached
                reached.Add(neighbor.coordinates, neighbor);
                // keep track to potentially add to final path?
                frontier.Enqueue(neighbor);
            }
        }
    }

    void BreadthFirstSearch () {
        bool isRunning = true;

        frontier.Enqueue(startNode);
        reached.Add(startCoordinates, startNode);

        while(frontier.Count > 0 && isRunning) {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbors();
            if (currentSearchNode.coordinates == destinationCoordinates) {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath() {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null) {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        path.Reverse();

        return path;
    }
}
