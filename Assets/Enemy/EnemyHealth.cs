using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = 5;

    [Tooltip("Adds amount to maxHitPoints when enemy dies.")]
    [SerializeField] int difficultyRamp = 1;
    
    int currentHitPoints = 0;

    Enemy enemy;

    // Start is called before the first frame update
    void OnEnable()
    {
        // set our current hit points to our maxHitpoints
        currentHitPoints = maxHitPoints;
    }

    void Start() {
        enemy = GetComponent<Enemy>();
    }

    void OnParticleCollision(GameObject other) {
        // process hit
        ProcessHit();
    }

    void ProcessHit() {
        currentHitPoints--;
        if (currentHitPoints <= 0) {
            gameObject.SetActive(false);
            maxHitPoints += difficultyRamp;
            enemy.RewardGold();
        }
    }
}
