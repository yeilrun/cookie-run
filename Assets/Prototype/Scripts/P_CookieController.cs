using UnityEngine;

namespace LHA {
    public class P_CookieController : MonoBehaviour
    {

        private int jumpCount = 0;
        private float jumpTime = 0f;
        private float jumpVelocity = 13f;
        private float gravity = -25f;
        private bool isJumping = false;
        private float startY = 0f;
        private LayerMask groundLayer;
        //private bool isGrounded = false;
        //private bool isDead = false;
        //private bool isSliding = true;
        //private bool isClashed = true;
        //private Animator animator;

        private void Start()
        {
            groundLayer = LayerMask.GetMask("Ground");
            //animator = GetComponent<Animator>();
        }

        private void Update()
        {
            //if (isDead)
            //    return;

            Jumping();
            Sliding();
            //animator.SetBool("Grounded", isGrounded);
        }

        private void Die()
        {
            //animator.SetTrigger("Die");

            //playerRigidbody.linearVelocity = Vector2.zero;
            //isDead = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Dead")
            {
                Die();
            }

            if (other.gameObject.name.Contains("wall"))
            {
                //animator.SetBool("Clash", isClashed);
            }

        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.name.Contains("wall"))
            {
                //animator.SetBool("Clash", !isClashed);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.contacts[0].normal.y > 0.7f)
            {
                //isGrounded = true;
                jumpCount = 0;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            //isGrounded = false;
        }

        private void Sliding()
        {
            transform.localScale = new Vector3(1f, 1.5f, 1f);
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                //animator.SetBool("Slide", isSliding);
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                transform.localScale = new Vector3(1f, 1.5f, 1f);
                //animator.SetBool("Slide", !isSliding);
            }
        }

        private void Jumping()
        {
            if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
            {
                jumpCount++;
                jumpTime = 0f;
                startY = transform.position.y;
                isJumping = true;
            }

            if (isJumping)
            {
                jumpTime += Time.deltaTime;
                float newY = startY + jumpVelocity * jumpTime + 0.5f * gravity * Mathf.Pow(jumpTime, 2);
                transform.position = new Vector2(transform.position.x, newY);

                if (gravity * jumpTime + jumpVelocity <= 0f && IsGrounded())
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y);
                    //transform.position = new Vector2(transform.position.x, Mathf.Round(transform.position.y * 100f) / 100f);
                    isJumping = false;
                }

            }

            if (!isJumping && IsGrounded())
            {
                jumpCount = 0;
            }

            if (jumpCount == 2)
            {
                    //animator.SetTrigger("DoubleJump");
            }
        }

        private bool IsGrounded()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
            return hit.collider != null;
        }

    }
}



