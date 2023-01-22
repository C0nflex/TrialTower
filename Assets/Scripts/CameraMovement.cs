using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float MaxPlayerAboveCamera = 100f;
    //public float CameraMovementSpeedInSeconds = 10f;
    public Transform PlayerLocation;
    public PlayerMovement playermovement;
    public float IncreasedCameraSpeed;

    private float MinHeightToStartMoving = -2f;
    private bool StartMoving = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, PlayerLocation.position.y + MaxPlayerAboveCamera, gameObject.transform.position.z);

        /*if (!StartMoving && PlayerLocation.position.y > MinHeightToStartMoving && playermovement.m_Grounded == true)
            StartMoving = true;
        if (PlayerLocation.position.y > gameObject.transform.position.y + MaxPlayerAboveCamera)
            IncreasedCameraSpeed = 10;
        else
            IncreasedCameraSpeed = 1;
        if (StartMoving)
            gameObject.transform.position += new Vector3(0, Time.deltaTime * IncreasedCameraSpeed, 0); */
    }
}
