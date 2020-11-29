using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Boid Prefab;
    public Color Color;
    public int SpawnCount = 25;

    void Awake()
    {
        for (int i = 0; i < SpawnCount; i++)
        {
            Boid boid = Instantiate(
                Prefab,
                Random.insideUnitCircle * SpawnCount,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                null
                );

            boid.SetColor(Color);
        }
    }
}
