﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class CameraMovement : MonoBehaviour
{

    public static CameraMovement instance;

    private GameObject player;
    private Vector3 displacement = new Vector3(0, 5, -20);

    private Transform myTransform;
    private Transform targetTransform;

    private Vector3 targetLocation;
    private Vector3 currentLocation;
    private Vector3 distance;

    private Vector3 initialPosition;
    private Vector3 lastPosition;
    private Vector3 shakeDisplacement;
    private float acceleration = 4;

    void Awake()
    {
        instance = this;
    }

    public IEnumerator<float> Shake(float shake, float intensity)
    {
        while (shake > 0)
        {
            shakeDisplacement = Random.onUnitSphere * shake * intensity;
            shake -= Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
    }

    void Start()
    {
        myTransform = gameObject.transform;
        lastPosition = myTransform.position;
    }

    public void UpdatePlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        targetTransform = player.gameObject.transform;

        Timing.RunCoroutine(Shake(0.2f, 5f));
    }

    void Update()
    {

        if (player != null)
        {
            currentLocation = lastPosition;
            targetLocation = new Vector3(targetTransform.position.x, targetTransform.position.y, 0) + displacement;

            distance = targetLocation - currentLocation;
            myTransform.position = currentLocation + distance * acceleration * Time.deltaTime;
            lastPosition = myTransform.position;
        }
        myTransform.position += shakeDisplacement;
    }
}
