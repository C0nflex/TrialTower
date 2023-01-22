using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasHandler : MonoBehaviour
{
    public TextMeshProUGUI GameOverText;
    public TextMeshProUGUI ScoreText;
    public Vector3 ScoreCameraOffset;
    public GameObject Camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameOverText.transform.position = new Vector3(Camera.transform.position.x, Camera.transform.position.y, -1);
        ScoreText.transform.position = new Vector3(Camera.transform.position.x, Camera.transform.position.y, -1) + ScoreCameraOffset;
    }
}
