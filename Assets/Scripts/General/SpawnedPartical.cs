using System;
using System.Collections;
using System.Collections.Generic;
using EventManager;
using UnityEngine;

public class SpawnedPartical : MonoBehaviour
{
    [SerializeField] private GameObject dustPartical;
    private void OnEnable()
    {
        ExternalEvents.WarriorsSpawned += OnWarriorSpawned;
        InternalEvents.BulletSpawned += OnBulletSpawned;
    }

    private void OnDisable()
    {
        ExternalEvents.WarriorsSpawned -= OnWarriorSpawned;
         InternalEvents.BulletSpawned -= OnBulletSpawned;
    }

    private void OnBulletSpawned()
    {
        GameObject particleInstance = Instantiate(dustPartical, transform.position, transform.rotation);
        
        
        Destroy(particleInstance, 3f);
    }

    private void OnWarriorSpawned()
    {
        GameObject particleInstance = Instantiate(dustPartical, transform.position, transform.rotation);
        
        Destroy(particleInstance, 3f);
    }
}
