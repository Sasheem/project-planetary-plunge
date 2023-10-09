using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int cost = 75;
    [SerializeField] float buildDelay = 1f;

    void Start() {
        StartCoroutine(Build());
    }

    public bool CreateTower(Tower tower, Vector3 position) {
        Bank bank = FindObjectOfType<Bank>();

        if (bank == null) {
            return false;
        }

        if (bank.CurrentBalance >= cost) {
            Instantiate(tower, position, Quaternion.identity);
            bank.Withdraw(cost);
            return true;
        }

        return false;
    }

    // Add a build delay
    IEnumerator Build() {
        // turn off all children and grandchildren in the hiearcrhy 
        foreach(Transform child in transform) {
            child.gameObject.SetActive(false);
            foreach(Transform grandchild in child) {
                grandchild.gameObject.SetActive(false);
            }
        }
        // enable the children and grandchildren sequentially
        foreach(Transform child in transform) {
            child.gameObject.SetActive(true);

            // Add a build delay to control how quickly they become active
            yield return new WaitForSeconds(buildDelay);
            foreach(Transform grandchild in child) {
                grandchild.gameObject.SetActive(true);
            }
        }
    }
}
