using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;
    [SerializeField] bool isPlaceable;
    public bool IsPlaceable { get { return isPlaceable; } }
    GridManager gridManager;
    Pathfinder pathfinder;
    Vector2Int coordinates = new Vector2Int();

    void Awake() {
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    void Start() {
        if (gridManager != null) {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            if (!isPlaceable) {
                gridManager.BlockNode(coordinates);
            }
        }
    }

    void OnMouseDown() {
        // checking if the node is walkable and won't block the path
        if (gridManager.GetNode(coordinates).isWalkable && !pathfinder.WillBlockPath(coordinates)) {
            bool isCreated = towerPrefab.CreateTower(towerPrefab, transform.position);
            // only block node if tower creation was successful
            if (isCreated) { 
                gridManager.BlockNode(coordinates);
                pathfinder.NotifyReceivers();
            }
        }
    }
}
