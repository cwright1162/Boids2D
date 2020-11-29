using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boid/Settings")]
public class Settings : ScriptableObject
{
    public float minSpeed = 1.0f;
    public float maxSpeed = 5.0f;

    public float flockRadius = 2.5f;       // Boids inside this radius are considered flockmates
    public float separationRadius = 1.0f;  // Boids inside this radius will cause them to separate

    public float maxSteerForce = 3.0f;     // Multiplier that will determine how drastic movements will be in regards to behaviors
    
    public float alignmentWeight = 1.0f;
    public float cohesionWeight = 1.0f;
    public float separationWeight = 1.0f;

    public float targetWeight = 1.0f;

    [Header("Collisions")]
    public LayerMask obstacleMask;
    public float castRadius = 0.15f;        // To be used with CircleCast
    public float avoidObstacleWeight = 2.0f;
    public float avoidObstacleDist = 5.0f;
}
