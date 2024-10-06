using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCameraFollow : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update () {
        transform.position = player.transform.position + new Vector3(0, 1, -5);
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
