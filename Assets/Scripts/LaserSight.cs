﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : MonoBehaviour
{
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
      lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
      Vector3 lineEndPosition = transform.position + (transform.forward * 50);
      lineRenderer.SetPosition(0, transform.position);
      lineRenderer.SetPosition(1, lineEndPosition); 
    }
}
