using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = 5;
    [SerializeField] int currentHitPoints = 0;

    // Start is called before the first frame update
    void Start()
    {
        // set our current hit points to our maxHitpoints
        currentHitPoints = maxHitPoints;
    }

    void OnParticleCollision(GameObject other) {
        // process hit
        ProcessHit();
    }

    void ProcessHit() {
        currentHitPoints--;
        if (currentHitPoints <= 0) {
            Destroy(gameObject);
        }
    }
}
