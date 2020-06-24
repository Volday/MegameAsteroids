using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingForward : MonoBehaviour
{
    private float moveSpeed;
    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    public void SetMoveSpeed(float _moveSpeed) {
        moveSpeed = _moveSpeed;
    }
}
