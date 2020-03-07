using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PathMover : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Queue<Vector3> _pathPoints = new Queue<Vector3>();

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        FindObjectOfType<PathCreator>().OnNewPathCreated += SetPoints;
    }

    private void SetPoints(IEnumerable<Vector3> points)
    {
        Debug.Log(points.Count());
        _pathPoints = new Queue<Vector3>();
        LerpVector3IEnumerable(points);
    }


    private void LerpVector3IEnumerable(IEnumerable<Vector3> points)
    {
        float totalDistance = Vector3.Distance(points.First(), points.Last());

        for (int i = 0; i < points.Count() - 1; i++)
        {
            var tmpDist = Vector3.Distance(points.ElementAt(i), points.ElementAt(i + 1));

            float fractionOfJourney = tmpDist / totalDistance;
            _pathPoints.Enqueue(Vector3.Lerp(points.ElementAt(i), points.ElementAt(i + 1), fractionOfJourney));
        }

        _pathPoints.Enqueue(points.Last());
    }


    // Update is called once per frame
    void Update()
    {
        UpdatePathing();
    }

    private void UpdatePathing()
    {
        //Debug.Log(ShouldSetDestination());
        if (ShouldSetDestination())
        {
            _navMeshAgent.SetDestination(_pathPoints.Dequeue());
        }
    }

    private bool ShouldSetDestination()
    {
        //Debug.Log(_pathPoints.Count);

        if (_pathPoints.Count == 0)
        {
            return false;
        }

        return _navMeshAgent.hasPath == false || _navMeshAgent.remainingDistance < 0.5f;
    }
}