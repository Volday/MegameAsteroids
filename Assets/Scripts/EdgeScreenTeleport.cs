using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeScreenTeleport : MonoBehaviour
{
    private Camera mainCamera;
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
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - mainCamera.orthographicSize * 2);
        }
        if (transform.position.z < -mainCamera.orthographicSize)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + mainCamera.orthographicSize * 2);
        }
        //Телепорт по горизонтали экрана
        float horizontalOrthographicSize = mainCamera.orthographicSize * screenRatio;
        if (transform.position.x > horizontalOrthographicSize)
        {
            transform.position = new Vector3(transform.position.x - horizontalOrthographicSize * 2, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -horizontalOrthographicSize)
        {
            transform.position = new Vector3(transform.position.x + horizontalOrthographicSize * 2, transform.position.y, transform.position.z);
        }
    }
}
