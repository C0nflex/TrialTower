using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	//[SerializeField] private float m_JumpForce = 400f;
	//[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	//[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings


	const float k_GroundedRadius = .35f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;
	private bool Inairlastupdate;   // Whether or not the player is grounded.
	//const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	//private bool m_FacingRight = false;  // For determining which way the player is currently facing.
	//private Vector3 velocity = Vector3.zero; 
    //private Animator animator;

	private float JumpVectorMultiplayer = 10f;
	private float JumpVectorOffset =1f;
	private float MaxHoldDownDuration = 100000000000f;

	public float CloseToRightWall = 7.7f;
	public float CloseToLeftWall = -7.7f;


	private void Start()
	{
		//animator = gameObject.GetComponent<Animator>();
		//if (animator == null)
			//Debug.Log("PlayerBehavior cant find Animator");
		m_Grounded = false;
	}

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}


	private void FixedUpdate()
	{
		Inairlastupdate = !m_Grounded;
		m_Grounded = false;
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, k_GroundedRadius, m_WhatIsGround);
		RaycastHit2D hit2;  //to check for right k_GroundedRadius parameter.
		float orgi = 0.001f;
		float i = 0;
		do
		{
			i = i + orgi;
			hit2 = Physics2D.Raycast(transform.position, -Vector2.up, k_GroundedRadius + i, m_WhatIsGround);
		} while (hit2.collider == null); 
		//Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		if (hit.collider != null)
        {
			m_Grounded = true;
		}
		if (m_Grounded & Inairlastupdate)
			StickLanding();
		if (!m_Grounded)
		{
			if (transform.position.x > CloseToRightWall)
				CloseToWall(Vector2.left);
			if (transform.position.x < CloseToLeftWall)
				CloseToWall(Vector2.right);
		}

	
			
		/*for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				m_Grounded = true;
		} */
		// last time was in air and now grounded
		/*if (m_Grounded && Inairlastupdate)
			animator.SetBool("IsJumping", false);
		if(!m_Grounded)
			animator.SetBool("IsJumping", false);
		if (!animator.GetBool("Falling") && !animator.GetBool("IsJumping") && !m_Grounded)
			animator.SetBool("Falling", true);
		if (animator.GetBool("Falling") && m_Grounded)
			animator.SetBool("Falling", false); */
	}

	private double positionx;
	private double positiony;
	public void Move(float HoldJumpDuration)
	{
		if (HoldJumpDuration == 0f || !m_Grounded)
			return;
		if (HoldJumpDuration > MaxHoldDownDuration)
			HoldJumpDuration = MaxHoldDownDuration;
		
		Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position;
		direction.Normalize(); //makes it so it only applies direction and no additional force (thats what we want).
		m_Rigidbody2D.AddForce(direction * (HoldJumpDuration + JumpVectorOffset) * JumpVectorMultiplayer, ForceMode2D.Impulse);
	}

	public void StickLanding()
    {
		m_Rigidbody2D.velocity = Vector2.zero;
		m_Rigidbody2D.angularVelocity = 0f;
	}

	public void CloseToWall(Vector2 inNormal)
    {
		m_Rigidbody2D.velocity = Vector2.Reflect(m_Rigidbody2D.velocity, inNormal);
    }

	/*private void Flip()
	{

	} */
}