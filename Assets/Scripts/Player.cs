using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Animator animator;
	public Rigidbody rigidbody;
	public float speedForward = 200f;
	public float speedBackwards = 150f;
	public float speedRotate = 150f;
	public float speedClimb = 20f;
	public KeyCode forward = KeyCode.W;
	public KeyCode backward = KeyCode.S;
	public KeyCode rotateLeft = KeyCode.A;
	public KeyCode rotateRight = KeyCode.D;
	public KeyCode jump = KeyCode.Space;
	
	public bool isGrounded = false;
	public bool isHanging = false;
	
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(NotGrounded());
    }
	
	void Update ()
	{
		if (Input.GetKeyDown (jump))
		{
			if (isGrounded)
			{
				isGrounded = false;
				animator.Play ("Jump");
				rigidbody.AddForce (Vector3.up * 35f, ForceMode.Impulse);
			}
			if (isHanging)
			{
				isHanging = false;
				animator.SetBool ("IsHanging", false);
				animator.Play ("Jump");
				rigidbody.AddForce (-transform.forward * 10f, ForceMode.Impulse);
				StartCoroutine(NotGrounded());
			}
		}
	}
	
    void FixedUpdate()
    {
        Move ();
    }
	
	void Move ()
	{
		if (isGrounded)
		{
			animator.SetBool ("IsHanging", false);
			
			if (Input.GetKey (forward))
			{
				animator.SetBool ("IsRunning", true);
				rigidbody.velocity = transform.forward * Time.fixedDeltaTime * speedForward;
			}
			
			if (Input.GetKey (backward))
			{
				animator.SetBool ("IsRunningBackwards", true);
				rigidbody.velocity = -transform.forward * Time.fixedDeltaTime * speedBackwards;
			}
			
			if (!Input.GetKey (forward) && !Input.GetKey (backward))
			{
				animator.SetBool ("IsRunning", false);
				animator.SetBool ("IsRunningBackwards", false);
				rigidbody.velocity = Vector3.zero;
			}
			
			if (Input.GetKey (rotateLeft))
			{
				rigidbody.MoveRotation (rigidbody.rotation * Quaternion.Euler(new Vector3 (0f, -speedRotate, 0f) * Time.fixedDeltaTime));
			}
			
			if (Input.GetKey(rotateRight))
			{
				rigidbody.MoveRotation (rigidbody.rotation * Quaternion.Euler(new Vector3 (0f, speedRotate, 0f) * Time.fixedDeltaTime));
			}
		}
		
		if (isHanging)
		{
			animator.SetBool ("IsRunning", false);
			animator.SetBool ("IsRunningBackwards", false);
			
			if (Input.GetKey (forward))
			{
				animator.SetBool ("IsClimbingUp", true);
				rigidbody.velocity = transform.up * Time.fixedDeltaTime * speedClimb;
			}
			
			if (Input.GetKey (backward))
			{
				animator.SetBool ("IsClimbingDown", true);
				rigidbody.velocity = -transform.up * Time.fixedDeltaTime * speedClimb;
			}
			
			if (Input.GetKey (rotateLeft))
			{
				animator.SetBool ("IsClimbingLeft", true);
				rigidbody.velocity = -transform.right * Time.fixedDeltaTime * speedClimb;
			}
			
			if (Input.GetKey(rotateRight))
			{
				animator.SetBool ("IsClimbingRight", true);
				rigidbody.velocity = transform.right * Time.fixedDeltaTime * speedClimb;
			}
			
			if (!Input.GetKey (forward) && !Input.GetKey (backward) && !Input.GetKey (rotateLeft) && !Input.GetKey (rotateRight))
			{
				animator.SetBool ("IsHanging", true);
				animator.SetBool ("IsClimbingLeft", false);
				animator.SetBool ("IsClimbingRight", false);
				animator.SetBool ("IsClimbingUp", false);
				animator.SetBool ("IsClimbingDown", false);
				rigidbody.velocity = Vector3.zero;
			}
		}
	}
	
	IEnumerator NotGrounded ()
	{
		yield return new WaitForSeconds (0.125f);
		
		if (!isHanging)
		{
			rigidbody.AddForce (-Vector3.up * 2.5f, ForceMode.Impulse);
		}
	}
	
	void OnCollisionStay (Collision collision)
	{
		if (collision.gameObject.layer == 7)
		{
			isGrounded = true;
		}
		
		if (collision.gameObject.layer == 8)
		{
			isHanging = true;
		}
	}
	
	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.layer == 8)
		{
			transform.rotation = collision.gameObject.transform.rotation;
		}
	}
	
	void OnCollisionExit (Collision collision)
	{
		if (collision.gameObject.layer == 7)
		{
			isGrounded = false;
			StartCoroutine(NotGrounded());
		}
		
		if (collision.gameObject.layer == 8)
		{
			isHanging = false;
		}
		
	}
}
