using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathCreatorTouch : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private List<Vector3> _points = new List<Vector3>();

    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { };

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Cleared Points");
            _points.Clear();
        }

        if (Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (DistanceToLastPoint(hitInfo.point) > 1f)
                {
                    Debug.Log("Added Point");
                    _points.Add(hitInfo.point);

                    _lineRenderer.positionCount = _points.Count;
                    _lineRenderer.SetPositions(_points.ToArray());
                }
            }
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            Debug.Log("Called OnNewPathCreated, passed " + _points.Count.ToString());
            OnNewPathCreated(_points);
        }
    }

    private float DistanceToLastPoint(Vector3 point)
    {
        return !_points.Any() ? Mathf.Infinity : Vector3.Distance(_points.Last(), point);
    }
}