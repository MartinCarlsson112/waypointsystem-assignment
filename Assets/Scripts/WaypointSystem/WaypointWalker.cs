using UnityEngine;
using UnityEngine.Assertions;

public class WaypointWalker : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    WaypointGroup waypoints;

    [SerializeField]
    [Range(0.001f, 1)]
    float distanceToReach = 0.05f;

    [SerializeField]
    float movementSpeed;

#pragma warning restore 0649

    Vector3 waypoint;
    bool hasWaypoint = false;
    bool reachedEnd = false;
    int waypointIndex = 0;

    private void Start()
    {
        Assert.IsNotNull(waypoints, "Waypoint walker is missing a reference to a WaypointGroup and will not execute");
    }

    private void Update()
    { 
        if(waypoints == null)
        {
            Assert.IsNotNull(waypoints, "Waypoint walker is missing a reference to a WaypointGroup and will not execute");
        }

        if(hasWaypoint)
        {
            Move();
        }
        else
        {
            hasWaypoint = waypoints.Begin(ref waypoint);
        }
    }

    private void Move()
    {
        transform.position += Vector3.Normalize((waypoint - transform.position)) * Time.deltaTime * movementSpeed;
        CheckDistance();
    }

    private void CheckDistance()
    {
        var distance = Vector3.Distance(transform.position, waypoint);
        if (distance < distanceToReach)
        {
            if (reachedEnd)
            {
                if(!waypoints.Previous(waypointIndex, out waypointIndex, ref waypoint))
                {
                    reachedEnd = false;
                    waypoints.Next(waypointIndex, out waypointIndex, ref waypoint);
                }
            }
            else
            {
                if (!waypoints.Next(waypointIndex, out waypointIndex, ref waypoint))
                {
                    reachedEnd = true;
                    waypoints.Previous(waypointIndex, out waypointIndex, ref waypoint);
                }
            }
        }
    }
}
