using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target;
    public float lerpTime;
    public bool targetPlayer = true;

    Transform startPosition;

    private void Start()
    {
        startPosition = this.transform;
        if (true)
        {
            target = Camera.main.transform;
        }
    }

    private void Update()
    {
        Vector3 relativePos = target.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, lerpTime * Time.deltaTime);

    }
}
