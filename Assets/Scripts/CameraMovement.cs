using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float MaxPlayerAboveCamera = 1f;
    public float CameraMovementSpeedInSeconds = 1f;
    public Transform PlayerLocation;
    public PlayerMovement playermovement;

    private float MinHeightToStartMoving = -2f;
    private bool StartMoving = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!StartMoving && PlayerLocation.position.y > MinHeightToStartMoving && playermovement.m_Grounded == true)
            StartMoving = true;
        if(StartMoving)
            gameObject.transform.position += new Vector3(0, Time.deltaTime, 0);
        //if (PlayerLocation.position.y > gameObject.transform.position.y + CameraMovementSpeedInSeconds)
        //gameObject.transform.position = new Vector3(gameObject.transform.position.x, PlayerLocation.position.y + CameraMovementSpeedInSeconds, gameObject.transform.position.z);
        //else
    }
}
