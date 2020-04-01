using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public GameObject manager;
    public Vector2 location = Vector2.zero;
    public Vector2 velocity;
    Vector2 goalPos = Vector2.zero;
    Vector2 currentForce;

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector2(Random.Range(0.01f, 0.1f), Random.Range(0.01f, 0.1f));
        location = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
    }

    Vector2 Seek(Vector2 target)
    {
        return (target - location);
    }

    void ApplyForce(Vector2 f)
    {
        Vector3 force = new Vector3(f.x, f.y, 0);
        if(force.magnitude > manager.GetComponent<AllUnits>().maxForce)
        {
            force = force.normalized;
            force *= manager.GetComponent<AllUnits>().maxForce;
        }
        this.GetComponent<Rigidbody2D>().AddForce(force);

        if (this.GetComponent<Rigidbody2D>().velocity.magnitude > manager.GetComponent<AllUnits>().maxVelocity)
        {
            this.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity.normalized;
            this.GetComponent<Rigidbody2D>().velocity *= manager.GetComponent<AllUnits>().maxVelocity;
        }

        Debug.DrawRay(this.transform.position, force, Color.white);
    }

    Vector2 Align()
    {
        float neighbordist = manager.GetComponent<AllUnits>().neighbourDistance;
        Vector2 sum = Vector2.zero;
        int count = 0;
        foreach (GameObject other in manager.GetComponent<AllUnits>().units)
        {
            if (other == this.gameObject) continue;

            float d = Vector2.Distance(location, other.GetComponent<UnitController>().location);

            if (d < neighbordist)
            {
                sum += other.GetComponent<UnitController>().velocity;
                count++;
            }
        }

        if (count > 0)
        {
            sum /= count;
            Vector2 steer = sum - velocity;
            return steer;
        }

        return Vector2.zero;
    }

    Vector2 Cohesion()
    {
        float neighbordist = manager.GetComponent<AllUnits>().neighbourDistance;
        Vector2 sum = Vector2.zero;
        int count = 0;
        foreach (GameObject other in manager.GetComponent<AllUnits>().units)
        {
            if (other == this.gameObject) continue;

            float d = Vector2.Distance(location, other.GetComponent<UnitController>().location);

            if (d < neighbordist)
            {
                sum += other.GetComponent<UnitController>().location;
                count++;
            }
        }

        if (count > 0)
        {
            sum /= count;            
            return Seek(sum);
        }

        return Vector2.zero;
    }

    void Flock()
    {
        location = this.transform.position;
        velocity = this.GetComponent<Rigidbody2D>().velocity;

        if (Random.Range(0, 50) <= 1)
        {
            Vector2 ali = Align();
            Vector2 coh = Cohesion();
            Vector2 gl;
            gl = Seek(goalPos);
            currentForce = gl + ali + coh;

            currentForce = currentForce.normalized;
        }

        ApplyForce(currentForce);
    }

    // Update is called once per frame
    void Update()
    {
        Flock();
        goalPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //goalPos = manager.transform.position;
    }
}

// https://www.youtube.com/watch?v=4mlyu9-WimM - 2D Flocking with Unity Part 1
// https://www.youtube.com/watch?v=iFAyb6x-a3Q - 2D Flocking with Unity Part 2
