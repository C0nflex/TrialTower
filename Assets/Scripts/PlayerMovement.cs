using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	//[SerializeField] private float m_JumpForce = 400f;
	//[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	private float KeepXVelocity;
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
		if(m_Rigidbody2D.velocity.x == 0)
			m_Rigidbody2D.velocity = new Vector2(KeepXVelocity, m_Rigidbody2D.velocity.y);
		Inairlastupdate = !m_Grounded;
		m_Grounded = false;
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, k_GroundedRadius, m_WhatIsGround);
		RaycastHit2D lefthit = Physics2D.Raycast(transform.position, (Vector2.down + Vector2.left).normalized, k_GroundedRadius * 2, m_WhatIsGround);
		RaycastHit2D righthit = Physics2D.Raycast(transform.position, (Vector2.down + Vector2.right).normalized, k_GroundedRadius * 2, m_WhatIsGround);
		RaycastHit2D left = Physics2D.Raycast(transform.position, Vector2.left, k_GroundedRadius * 2, m_WhatIsGround);
		RaycastHit2D right = Physics2D.Raycast(transform.position, Vector2.right, k_GroundedRadius * 2, m_WhatIsGround);
		//Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		if (hit.collider != null || 
			(lefthit.collider != null && left.collider == null && right.collider == null) || 
			(righthit.collider !=null && left.collider == null && right.collider == null))
        {
			m_Grounded = true;
		}
		if (m_Grounded & Inairlastupdate)
		{
			StickLanding();
			KeepXVelocity = 0;
		}
		if (!m_Grounded)
		{
			RaycastHit2D hitCeiling = Physics2D.Raycast(transform.position, Vector2.up, k_GroundedRadius * 2, m_WhatIsGround);
			RaycastHit2D leftCeiling = Physics2D.Raycast(transform.position, (Vector2.up + Vector2.left).normalized, k_GroundedRadius * 2, m_WhatIsGround);
			RaycastHit2D rightCeiling = Physics2D.Raycast(transform.position, (Vector2.up + Vector2.right).normalized, k_GroundedRadius * 2, m_WhatIsGround);
			if (left.collider != null || leftCeiling.collider != null)
				CloseToWall(Vector2.left);
			if (right.collider != null || rightCeiling.collider != null)
				CloseToWall(Vector2.right);
			if (hitCeiling.collider != null)
				CloseToWall(Vector2.up);
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
		KeepXVelocity = m_Rigidbody2D.velocity.x;
	}

	public void StickLanding()
    {
		m_Rigidbody2D.velocity = Vector2.zero;
		m_Rigidbody2D.angularVelocity = 0f;
	}

	public void CloseToWall(Vector2 inNormal)
    {
		if (inNormal.y == 0 && ((inNormal.x <= 0 && m_Rigidbody2D.velocity.x >= 0) || (inNormal.x >= 0 && m_Rigidbody2D.velocity.x <= 0)) ||
			(inNormal.x == 0 && ((inNormal.y <= 0 && m_Rigidbody2D.velocity.y >= 0) || (inNormal.y >= 0 && m_Rigidbody2D.velocity.y <= 0))))
			return; 
		Vector2 temp = Vector2.Reflect(m_Rigidbody2D.velocity, inNormal);
		KeepXVelocity = temp.x;
		m_Rigidbody2D.velocity = new Vector2(KeepXVelocity, temp.y);
	}

	/*private void Flip()
	{

	} */
}