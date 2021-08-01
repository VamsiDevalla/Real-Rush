using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int goldReward = 25;
    [SerializeField] int goldPenality = 25;

    Bank bank;

    // Start is called before the first frame update
    void Start()
    {
        bank = FindObjectOfType<Bank>();
    }

    public int RewardGold()
    {
        if (bank == null) return -1;
        return bank.Deposit(goldReward);
    }

    public int StealGold()
    {
        if (bank == null) return -1;
        if (bank.CurrentBalance < goldPenality)
        {
            GameManager.ReloadScene();
            return 0;
        }
        else
        {
            return bank.Withdraw(goldPenality);
        }
    }
}
