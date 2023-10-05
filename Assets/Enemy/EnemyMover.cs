using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    // pass in some waypoints to follow
    // loop through list and print to console
    [SerializeField] List<Tile> path = new List<Tile>();
    [SerializeField] [Range(0f, 5f)] float speed = 1f;

    Enemy enemy;

    void OnEnable()
    {
        FindPath();
        ReturnToStart();
        // begin coroutine
        StartCoroutine(FollowPath());
    }

    void Start() {
        enemy = GetComponent<Enemy>();
    }

    void FindPath() {
        // clear path so we know it is always empty prior to being set up
        path.Clear();

        GameObject parent = GameObject.FindGameObjectWithTag("Path");

        // loop through each child of the GameObject Path and add it to the list 
        foreach (Transform child in parent.transform) {
            Tile tile = child.GetComponent<Tile>();
            if (tile != null) {
                path.Add(tile);
            }
        }
    }

    // place the enemy at start position of path
    void ReturnToStart() {
        transform.position = path[0].transform.position;
    }

    void FinishPath() {
        enemy.StealGold();
        gameObject.SetActive(false);
    }

    // loop through the list and print names
    IEnumerator FollowPath() {
        foreach(Tile tile in path) {
            // set up start and end position we want to move to
            Vector3 startPosition = transform.position;
            Vector3 endPosition = tile.transform.position;
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
