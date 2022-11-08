using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSeeker : Agent
{
    public Transform midEye, midLeftEye, midRightEye, leftEye, rightEye;
    public float range;

    [Header("PathSeeker Settings")]
    public float moveSpeed = 0;
    public float steeringSpeed = 0;

    private float[] inputs = new float[5];

    private void FixedUpdate()
    {
        if (!isSimulated) return;
        Move();
        inputs[0] = Physics2D.Raycast(leftEye.position, leftEye.transform.up, range).distance;
        inputs[0] = inputs[0] == 0 ? range : inputs[0];
        inputs[1] = Physics2D.Raycast(midLeftEye.position, midLeftEye.transform.up, range).distance;
        inputs[1] = inputs[1] == 0 ? range : inputs[1];
        inputs[2] = Physics2D.Raycast(midEye.position, midEye.transform.up, range).distance;
        inputs[2] = inputs[2] == 0 ? range : inputs[2];
        inputs[3] = Physics2D.Raycast(midRightEye.position, midRightEye.transform.up, range).distance;
        inputs[3] = inputs[3] == 0 ? range : inputs[3];
        inputs[4] = Physics2D.Raycast(rightEye.position, rightEye.transform.up, range).distance;
        inputs[4] = inputs[4] == 0 ? range : inputs[4];

        SendInputToBrain(inputs);
    }

    private void Move()
    {
        transform.position += transform.rotation * transform.up * (Time.fixedDeltaTime * moveSpeed);
    }

    protected override void GotResultFromBrain(float[] output)
    {
        float turn = output[0]* 2 - 1;
        transform.Rotate(Vector3.forward * turn * steeringSpeed * Time.fixedDeltaTime);
    }

}
