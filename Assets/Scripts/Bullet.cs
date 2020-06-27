﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EdgeScreenTeleport))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Bullet : MonoBehaviour
{
    private Vector3 lastPosition;
    private bool skipTick = false;
    public bool useRayCastHitScan = true;
    public float bulletLifeTime;
    public float bulletMoveSpeed;

    public void Activate()
    {
        gameObject.SetActive(true);
        GetComponent<MovingForward>().SetMoveSpeed(bulletMoveSpeed);
        StartCoroutine(DieInTime(bulletLifeTime));

        lastPosition = transform.position;
    }

    public void Activate(float _bulletLifeTime)
    {
        bulletLifeTime = _bulletLifeTime;
        Activate();
    }

    private void LateUpdate()
    {
        if (useRayCastHitScan) {
            if (!skipTick)
            {
                RaycastHit hit;
                Ray ray = new Ray(lastPosition, transform.position - lastPosition);
                if (Physics.Raycast(ray, out hit, ray.direction.magnitude)) {
                    if (hit.transform.GetComponent<Damageable>() != null) {
                        hit.transform.GetComponent<Damageable>().TakeHit();
                        gameObject.SetActive(false);
                    }
                }
            }
            else {
                skipTick = false;
            }
        }

        lastPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Damageable>() != null)
        {
            other.gameObject.GetComponent<Damageable>().TakeHit();
            gameObject.SetActive(false);
        }
    }

    public void SkipOneTick()
    {
        skipTick = true;
    }

    public void Die() {
        gameObject.SetActive(false);
    }

    IEnumerator DieInTime(float _bulletLifeTime) {
        yield return new WaitForSeconds(_bulletLifeTime);
        Die();
    }
}
