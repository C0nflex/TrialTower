using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{

	public PlayerMovement MovementSystem;

	//public float runSpeed = 40f;
	//Animator animator;

	private float SpringTimerStart;
	private float SpringTimerDuration = 0f;

	private void Start()
	{
		//animator = gameObject.GetComponent<Animator>();
	}
	// Update is called once per frame
	void Update()
	{
		//animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (Input.GetButtonDown("Jump"))
		{
			SpringTimerStart = Time.time;
			//animator.SetBool("IsJumping", false);
		}
		if (Input.GetButtonUp("Jump"))
		{
			SpringTimerDuration = Time.time - SpringTimerStart;
			//animator.SetBool("IsJumping", false);
		}
	}

	/*public void onlanding()
	{
		animator.SetBool("IsJumping", false);
	} */

	void FixedUpdate()
	{
		Debug.Log(Input.mousePosition);
		// Move our character
		MovementSystem.Move(SpringTimerDuration);
		SpringTimerDuration = 0f;
	}
}
