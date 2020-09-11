﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{

    private float rotZ;
    public float RotationSpeed;
    public bool ClockwiseRotation;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ClockwiseRotation == false)
        {
            rotZ += Time.deltaTime * RotationSpeed;

        }

        else
        {
            rotZ += -Time.deltaTime * RotationSpeed;
        }

        transform.rotation = Quaternion.Euler(0, 0, rotZ);

    }

}
