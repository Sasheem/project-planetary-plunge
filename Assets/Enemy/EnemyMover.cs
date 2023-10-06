using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] [Range(0f, 5f)] float speed = 1f;

    List<Node> path = new List<Node>();

    Enemy enemy;

    GridManager gridManager;
    Pathfinder pathfinder;

    void OnEnable()
    {
        FindPath();
        ReturnToStart();
        // begin coroutine
        StartCoroutine(FollowPath());
    }

    void Awake() {
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    void FindPath() {
        // clear path so we know it is always empty prior to being set up
        path.Clear();
        path = pathfinder.GetNewPath();
    }

    // place the enemy at start position of path
    void ReturnToStart() {
        transform.position = gridManager.GetPositionFromCoordinates(pathfinder.StartCoordinates);
    }

    void FinishPath() {
        enemy.StealGold();
        gameObject.SetActive(false);
    }

    // loop through the list and print names
    IEnumerator FollowPath() {
        for(int i = 0; i < path.Count; i++) {
            // set up start and end position we want to move to
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercent = 0f;

            // always facing the waypoint enemy is heading to
            transform.LookAt(endPosition);

            // while not at end position
            while(travelPercent < 1f) {
                // update travel percent
                travelPercent += Time.deltaTime * speed;
                // move position of enemy
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                // yield back to update function until end of frame has completed
                yield return new WaitForEndOfFrame();
            }
        }
        FinishPath();
    }
}
