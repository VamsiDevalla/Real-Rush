using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int towerPrice = 15;
    [SerializeField] float buildDelay = 1f;

    void Start()
    {
        StartCoroutine(build());
    }

    public bool CreateTower(Tower tower, Vector3 position)
    {
        Bank bank = FindObjectOfType<Bank>();

        if (bank == null) return false;
        if (bank.CurrentBalance < towerPrice) return false;

        Instantiate(tower.gameObject, position, Quaternion.identity);
        bank.Withdraw(towerPrice);
        return true;
    }

    IEnumerator build()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
            foreach(Transform grandChild in child)
            {
                grandChild.gameObject.SetActive(false);
            }
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildDelay);
            foreach (Transform grandChild in child)
            {
                grandChild.gameObject.SetActive(true);
            }
        }
    }
    
}
