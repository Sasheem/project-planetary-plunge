using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 150;
    [SerializeField] int currentBalance;
    public int CurrentBalance { get { return currentBalance; } }

    [SerializeField] TextMeshProUGUI displayBalance;

    void Awake() {
        currentBalance = startingBalance;
        updateDisplay();
    }

    public void Deposit(int amount) {
        currentBalance += Mathf.Abs(amount);
        updateDisplay();
    }

    public void Withdraw(int amount) {
        currentBalance -= Mathf.Abs(amount);
        updateDisplay();

        if (currentBalance < 0) {
            // production - call in to another method or scene manager to handle end of level
            // lose the game condition
            ReloadScene();
        }
    }

    void updateDisplay() {
        displayBalance.text = "Gold: " + currentBalance;
    }

    void ReloadScene() {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
