using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    [SerializeField] int initialBalance = 150;
    [SerializeField] GameManager gameManager;

    int currentBalance;
    public int CurrentBalance { get { return currentBalance; } }

    // Start is called before the first frame update
    void Start()
    {
        currentBalance = initialBalance;
        BalenceChanged();
    }

    public int Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);
        BalenceChanged();
        return currentBalance;
    }

    public int Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);
        BalenceChanged();
        return currentBalance;
    }

    void BalenceChanged()
    {
        gameManager.UpdateGoldReserves(currentBalance);
    }
}
