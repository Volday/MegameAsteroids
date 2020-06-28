using System;
using System.Collections.Generic;
using UnityEngine;

public class EdgeScreenTeleport : MonoBehaviour
{
    private Camera mainCamera;

    Queue<Action> actions = new Queue<Action>();

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        float screenRatio = (float)Screen.width / Screen.height;

        //Телепорт по вертикали экрана
        if (transform.position.z > mainCamera.orthographicSize) 
        {
            NotifyAboutTeleport();
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - mainCamera.orthographicSize * 2);
        }
        if (transform.position.z < -mainCamera.orthographicSize)
        {
            NotifyAboutTeleport();
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + mainCamera.orthographicSize * 2);
        }
        //Телепорт по горизонтали экрана
        float horizontalOrthographicSize = mainCamera.orthographicSize * screenRatio;
        if (transform.position.x > horizontalOrthographicSize)
        {
            NotifyAboutTeleport();
            transform.position = new Vector3(transform.position.x - horizontalOrthographicSize * 2, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -horizontalOrthographicSize)
        {
            NotifyAboutTeleport();
            transform.position = new Vector3(transform.position.x + horizontalOrthographicSize * 2, transform.position.y, transform.position.z);
        }
    }

    public void AddAction(Action _action)
    {
        actions.Enqueue(_action);
    }

    public void NotifyAboutTeleport()
    {
        int actionsCount = actions.Count;
        for (int i = 0; i < actionsCount; i++)
        {
            actions.Dequeue()();
        }
    }
}
