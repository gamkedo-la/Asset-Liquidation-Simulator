﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    [SerializeField] [Range(0, 1)] float movementFactor;
    [SerializeField] float period = 2f;
    const float tau = Mathf.PI * 2;


    private void Start()
    {
        startingPosition = transform.position;
    }


    private void Update()
    {
        if (period <= Mathf.Epsilon) { return; }

        float cycles = Time.time / period;
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = (rawSinWave + 1f) * 0.5f;

        Vector3 offset = movementVector * movementFactor;

        transform.position = startingPosition + offset;
    }
}
