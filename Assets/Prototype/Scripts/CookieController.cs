using UnityEngine;

namespace LHA {
    public class CookieController : MonoBehaviour
    {
        private float sinDist = 3f;

        private int jumpCount = 0;
        private bool isGrounded = false;
        private bool isDead = false;
        private bool isSliding = true;
        private bool isClashed = true;

        private Rigidbody2D playerRigidbody;
        private Animator animator;

        private void Start()
        {
            playerRigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (isDead)
                return;

            Jumping();
            Sliding();
            animator.SetBool("Grounded", isGrounded);
        }

        private void Die()
        {
            animator.SetTrigger("Die");

            playerRigidbody.linearVelocity = Vector2.zero;
            isDead = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Dead" && !isDead)
            {
                Die();
            }

            if (other.gameObject.name.Contains("wall"))
            {
                animator.SetBool("Clash", isClashed);
            }

        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.name.Contains("wall"))
            {
                animator.SetBool("Clash", !isClashed);
            }
        }



        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.contacts[0].normal.y > 0.7f)
            {
                isGrounded = true;
                jumpCount = 0;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            isGrounded = false;
        }

        private void Sliding()
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                animator.SetBool("Slide", isSliding);
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                animator.SetBool("Slide", !isSliding);
            }
        }

        private void Jumping()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && jumpCount < 2)
            {
                jumpCount++;
                playerRigidbody.linearVelocity = Vector3.zero;

                Vector3 jumpPos = transform.position;

                transform.position = jumpPos + new Vector3(0f, Mathf.Sin(2f) * sinDist, 0f);

                if (jumpCount == 2)
                {
                    animator.SetTrigger("DoubleJump");
                }
            }
        }


    }
}



