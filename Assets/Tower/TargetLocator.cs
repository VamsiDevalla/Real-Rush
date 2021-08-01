using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weaponTransform;
    [SerializeField] float range = 15f;
    [SerializeField] ParticleSystem projectileParticles;
    Transform enemyTransform;

    // Update is called once per frame
    void Update()
    {
        FindClosestTarget();
        AimWeapon();
    }

    void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTransform = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestTransform = enemy.transform;
            }
        }

        enemyTransform = closestTransform;
    }

    void AimWeapon()
    {
        if (enemyTransform == null) return;

        float targetDistance = Vector3.Distance(enemyTransform.position, transform.position);
        if (targetDistance > range)
        {
            Attack(false);
        } else
        {
            Attack(true);
        }
        weaponTransform.transform.LookAt(enemyTransform);
    }

    void Attack(bool isActive)
    {
        var emission = projectileParticles.emission;
        emission.enabled = isActive;
    }
}
