using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RedLineMovement : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public float RedLineOffset;
    public TextMeshProUGUI GameOverText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerMovement.m_Grounded && playerMovement.Inairlastupdate)
        {
            this.transform.position = new Vector3(this.transform.position.x,playerMovement.transform.position.y - RedLineOffset, this.transform.position.z);
        }
        if(playerMovement.transform.position.y < this.transform.position.y + this.transform.localScale.y / 2)
        {
            playerMovement.GameOver();
            GameOverText.enabled = true;
        }
    }
}
