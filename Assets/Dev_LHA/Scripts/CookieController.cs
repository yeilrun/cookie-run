using UnityEngine;

namespace LHA
{
    public class CookieController : MonoBehaviour
    {
        private LayerMask groundLayer;
        private int jumpCount = 0;
        private float jumpTime = 0f;
        private float jumpVelocity = 7f;
        private float gravity = -25f;
        private bool isJumping = false;
        private float startY = 0f;

        private Animator animator;
        private bool isDead = false;

        private void Start()
        {
            groundLayer = LayerMask.GetMask("Ground");
            animator = GetComponent<Animator>();
            animator.SetBool("Grounded", false);
        }

        private void Update()
        {
            if (isDead)
                return;

            Jumping();
            Sliding();
        }

        private void Die()
        {
            animator.SetTrigger("Die");

           //playerRigidbody.linearVelocity = Vector2.zero;
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
                animator.SetBool("Clash", true);
            }

        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.name.Contains("wall"))
            {
                animator.SetBool("Clash", false);
            }
        }


        private void Sliding()
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                animator.SetBool("Slide", true);
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                animator.SetBool("Slide", false);
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
                animator.SetBool("Grounded", false);
                animator.SetTrigger("Jump");
            }

            if (isJumping)
            {
                jumpTime += Time.deltaTime;
                float newY = startY + jumpVelocity * jumpTime + 0.5f * gravity * Mathf.Pow(jumpTime, 2);
                transform.position = new Vector2(transform.position.x, newY);

                if (gravity * jumpTime + jumpVelocity <= 0f && Grounding())
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y);
                    //transform.position = new Vector2(transform.position.x, Mathf.Round(transform.position.y * 100f) / 100f);
                    isJumping = false;
                }
            }

            if (jumpCount == 2)
            {
                animator.SetTrigger("DoubleJump");
            }

            if (!isJumping && Grounding())
            {
                jumpCount = 0;
                animator.SetBool("Grounded", true);
            }


        }

        private bool Grounding()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.01f, groundLayer);
            return hit.collider != null;
        }

    }
}