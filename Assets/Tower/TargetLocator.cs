using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] ParticleSystem projectileParticles;
    [SerializeField] float range = 15f;
    Transform target;

    // Update is called once per frame
    void Update()
    {
        FindClosestTarget();
        AimWeapon();
    }

    // grab every enemy in scene
    // compare distances to see which is closest
    // FUTURE EDIT - add safeguards so this function isn't always running
    // if there are a lot of enemies then this foreach loop will become expensive
    // Ex) run if enemy goes out of range or is destroyed
    void FindClosestTarget() {
        
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies) {
            // param1 is current position of ballista, 
            // param2 is enemy position
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if (targetDistance < maxDistance) {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }

        // update the new target for this ballista
        target = closestTarget;
    }

    void AimWeapon() {
        // check if enemy is within range of this ballista
        float targetDistance = Vector3.Distance(transform.position, target.position);
        weapon.LookAt(target);
        if (targetDistance < range) {
            Attack(true);
        } else {
            Attack(false);
        }
    }

    void Attack(bool isActive) {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
    }
}
