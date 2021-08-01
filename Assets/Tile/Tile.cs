using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;
    [SerializeField] bool isPlaceable;
    public bool IsPlaceable { get { return isPlaceable; } }

    void OnMouseDown()
    {
        if (isPlaceable)
        {
            bool towerPlaced = towerPrefab.CreateTower(towerPrefab, transform.position);
            isPlaceable = !towerPlaced;
        }
    }

}
