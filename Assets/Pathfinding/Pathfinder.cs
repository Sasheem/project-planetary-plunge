using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }
    [SerializeField] Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }

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
            startNode = grid[startCoordinates];
            destinationNode = grid[destinationCoordinates];
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath() {
        gridManager.ResetNodes();
        BreadthFirstSearch();
        return BuildPath();
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

    // search for a path
    void BreadthFirstSearch () {
        // setting both to true here so they are walkable 
        // for our pathfinding but not placeable for towers
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;
        
        // clear existing path
        frontier.Clear();
        reached.Clear();

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

    // construct the path from BFS method above
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

    // check whether placing a tower will block a node (or enemy path?)
    public bool WillBlockPath(Vector2Int coordinates) {
        // make change to the node at the coords
        // check if bfs can find a path through it
        // if it can, return true
        // if it cant, return false
        if (grid.ContainsKey(coordinates)) {
            bool previousState = grid[coordinates].isWalkable;

            grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            // Means the path will be blocked because it wasn't able to get past the single node
            // when it was trying to build a path
            if(newPath.Count <= 1) {
                // get a new path with the old state of the node we were playing with
                GetNewPath();
                return true;
            }
            
        }

        // if we couldn't find our gridManager than we haven't blocked a path AND
        // if greater than 1 than that means it got away from it's starting node
        // thus finding a valid path
        return false;
    }
}
