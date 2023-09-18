using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    // pass in some waypoints to follow
    // loop through list and print to console
    [SerializeField] List<Waypoint> path = new List<Waypoint>();
    [SerializeField] float waitTime = 1f;

    void Start()
    {
        StartCoroutine(FollowPath());
    }

    // loop through the list and print names
    IEnumerator FollowPath() {
        foreach(Waypoint waypoint in path) {
            transform.position = waypoint.transform.position;
            
            // causes delay
            yield return new WaitForSeconds(waitTime);
        }
    }
}
