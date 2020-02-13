using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target;
    public bool targetPlayer = true;

    Transform startPosition;

    private void Start()
    {
        startPosition = this.transform;
        if (true)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(transform.forward, direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 100 * Time.deltaTime);

    }
}
