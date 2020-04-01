using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllUnits : MonoBehaviour
{
    public GameObject[] units;
    public GameObject unit;
    public int numUnits = 20;
    public Vector3 range = new Vector3(5, 5, 5);

    [Range(0, 200)]
    public int neighbourDistance = 50;

    [Range(0, 2)]
    public float maxForce = 0.5f;

    [Range(0, 5)]
    public float maxVelocity = 2.0f;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(this.transform.position, range * 2);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, 0.2f);
    }

    // Start is called before the first frame update
    void Start()
    {
        units = new GameObject[numUnits];
        for (int i = 0; i < numUnits; i++)
        {
            Vector3 unitPos = new Vector3(Random.Range(-range.x, range.x), Random.Range(-range.y, range.y), Random.Range(0, 0));
            units[i] = Instantiate(unit, this.transform.position + unitPos, Quaternion.identity) as GameObject;
            units[i].GetComponent<UnitController>().manager = this.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

// https://www.youtube.com/watch?v=4mlyu9-WimM - 2D Flocking with Unity Part 1
// https://www.youtube.com/watch?v=iFAyb6x-a3Q - 2D Flocking with Unity Part 2
