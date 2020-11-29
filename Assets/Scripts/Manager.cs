using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Settings Settings;
    public Transform Target;

    Boid[] boids;

    void Start()
    {
        boids = FindObjectsOfType<Boid>();
        foreach (Boid b in boids)
        {
            b.Initialize(Settings, Target == null ? null : Target);
        }
    }

    void Update()
    {
        if (boids != null)
        {
            int numBoids = boids.Length;

            for (int i = 0; i < numBoids; i++)
            {
                boids[i].numLocalFlockmates = 0;
                //Debug.Log(boids[i].position);
                for (int j = 0; j < numBoids; j++)
                {
                    if (i != j)
                    {
                        Boid b = boids[j];
                        Vector2 offset = b.position - boids[i].position;
                        float sqrDst = offset.x * offset.x + offset.y * offset.y;

                        if (sqrDst < Settings.flockRadius * Settings.flockRadius)
                        {
                            boids[i].numLocalFlockmates += 1;
                            boids[i].avgFlockHeading += (Vector2)b.transform.position;
                            boids[i].flockCenter += b.position;
                            //Debug.Log(boids[i].avgFlockHeading);

                            if (sqrDst < Settings.separationRadius * Settings.separationRadius)
                            {
                                boids[i].avgSeparationHeading -= offset / sqrDst;
                            }
                        }
                    }
                }
                boids[i].UpdateBoid();
            }
        }
    }
}
