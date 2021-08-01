using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int towerPrice = 15;


    public bool CreateTower(Tower tower, Vector3 position)
    {
        Bank bank = FindObjectOfType<Bank>();

        if (bank == null) return false;
        if (bank.CurrentBalance < towerPrice) return false;

        Instantiate(tower.gameObject, position, Quaternion.identity);
        bank.Withdraw(towerPrice);
        return true;
    }
    
}
