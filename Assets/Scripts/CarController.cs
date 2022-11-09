using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Settings")]
    public float driftFactor = 0.95f;
    public float acceralationFactor = 30.0f;
    public float turnFactor = 3.5f;

    public float maxSpeed = 25f;

    [Header("Sprites")]
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer shadowSpriteRenderer;

    private float accelerationValue = 0;
    private float steeringValue = 0;
    private float rotaionAngle = 0;
    private float applicableVelocity = 0;

    private Rigidbody2D carRigidbody;
    private Collider2D carCollider;


    private void Awake()
    {
        carRigidbody = GetComponent<Rigidbody2D>();
        carCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        rotaionAngle = transform.rotation.eulerAngles.z;
        Debug.Log(rotaionAngle);
    }
}
