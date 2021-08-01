using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] [Range(0f, 5f)] float speed = 1f;
    List<Node> path = new List<Node>();

    Enemy enemy;
    PathFinder pathFinder;
    GridManager gridManager;

    void OnEnable()
    {
        ReturnToStart();
        RecalculatePath(true);
    }

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    void RecalculatePath(bool resetPath)
    {
        Vector2Int coords = new Vector2Int();

        if (resetPath)
        {
            coords = pathFinder.StartCoordinates;
        } else
        {
            coords = gridManager.GetCoordinatesFromPosition(transform.position);
        }
        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(coords);
        StartCoroutine(FollowPath());
    }

    void ReturnToStart()
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathFinder.StartCoordinates);
    }

    void FinishPath()
    {
        enemy.StealGold();
        gameObject.SetActive(false);
    }


    IEnumerator FollowPath()
    {
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 start = transform.position;
            Vector3 end = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercent = 0f;
            transform.LookAt(end);
            while (travelPercent < 1)
            {
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(start, end, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

        FinishPath();
    }
}
