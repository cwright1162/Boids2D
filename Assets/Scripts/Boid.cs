using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    // Boid Settings
    Settings Settings;

    // State
    [HideInInspector]
    public Vector2 position;
    [HideInInspector]
    public Vector2 up;
    [HideInInspector]
    public Vector2 Velocity;

    // Flock Information
    [HideInInspector]
    public Vector2 avgFlockHeading;
    [HideInInspector]
    public Vector2 avgSeparationHeading;
    [HideInInspector]
    public Vector2 flockCenter;
    [HideInInspector]
    public int numLocalFlockmates;

    // Cache
    SpriteRenderer Renderer;
    Transform cachedTransform;
    Transform Target;   // Target will be used if we want boids to fly towards this point

    void Awake()
    {
        Renderer = transform.GetComponentInChildren<SpriteRenderer>();
        cachedTransform = transform;
    }

    public void Initialize(Settings Settings, Transform target)
    {
        this.Settings = Settings;
        this.Target = target;

        position = cachedTransform.position;
        up = cachedTransform.up;

        float startSpeed = (Settings.minSpeed + Settings.maxSpeed) / 2;
        Velocity = transform.up * startSpeed;
    }

    public void UpdateBoid()
    {
        Vector2 acceleration = Vector2.zero;

        if (Target != null)
        {
            Vector2 targetOffset = (Vector2)Target.position - position;
            acceleration = SteerTowards(targetOffset) * Settings.targetWeight;
        }

        if (numLocalFlockmates != 0)
        {
            flockCenter /= numLocalFlockmates;

            Vector2 offsetFlockCenter = (flockCenter - position);

            var alignmentForce = SteerTowards(avgFlockHeading) * Settings.alignmentWeight;
            var cohesionForce = SteerTowards(offsetFlockCenter) * Settings.cohesionWeight;
            var seperationForce = SteerTowards(avgSeparationHeading) * Settings.separationWeight;

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += seperationForce;
        }

        if (IsHeadingForCollision())
        {
            Vector2 collisionAvoidDir = ObstacleRays();
            Vector2 collisionAvoidForce = SteerTowards(collisionAvoidDir) * Settings.avoidObstacleWeight;
            acceleration += collisionAvoidForce;
        }

        Velocity += acceleration * Time.deltaTime;
        float speed = Velocity.magnitude;
        //Debug.Log(speed);
        if (speed == 0)
        {
            speed = Settings.minSpeed;
        }
        Vector2 dir = Velocity / speed;
        speed = Mathf.Clamp(speed, Settings.minSpeed, Settings.maxSpeed);
        Velocity = dir * speed;

        cachedTransform.position += (Vector3)Velocity * Time.deltaTime;
        cachedTransform.up = Velocity;
        position = cachedTransform.position;

    }

    Vector2 SteerTowards(Vector2 target)
    {
        Vector2 v = target.normalized * Settings.maxSpeed - Velocity;
        return Vector2.ClampMagnitude(v, Settings.maxSteerForce);
    }
    
    bool IsHeadingForCollision()
    {
        if (Physics2D.CircleCast(position, Settings.castRadius, up, Settings.avoidObstacleDist, Settings.obstacleMask))
        {
            return true;
        }

        return false;
    }

    Vector2 ObstacleRays()
    {
        Vector2[] rayDirections = BoidHelper.directions;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector2 dir = cachedTransform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(position, dir);
            //Debug.DrawRay(position, dir, Color.yellow);
            if (!Physics2D.CircleCast(ray.origin, Settings.castRadius, ray.direction, Settings.obstacleMask))
            {
                return dir;
            }
        }

        return up;
    }

    public void SetColor(Color color)
    {
        Renderer.color = color;

    }
}
