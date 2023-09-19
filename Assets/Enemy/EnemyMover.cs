using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    // pass in some waypoints to follow
    // loop through list and print to console
    [SerializeField] List<Waypoint> path = new List<Waypoint>();
    [SerializeField] [Range(0f, 5f)] float speed = 1f;

    void Start()
    {
        // begin coroutine
        StartCoroutine(FollowPath());
    }

    // loop through the list and print names
    IEnumerator FollowPath() {
        foreach(Waypoint waypoint in path) {
            // set up start and end position we want to move to
            Vector3 startPosition = transform.position;
            Vector3 endPosition = waypoint.transform.position;
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
    }
}
