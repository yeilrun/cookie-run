using UnityEngine;

namespace LHA {
    public class CookieController : MonoBehaviour
    {

        private int jumpCount = 0;
        private int jumpForce = 250;
        //private bool isGrounded = false;
        //private bool isDead = false;
        //private bool isSliding = true;
        //private bool isClashed = true;

        private Rigidbody2D playerRigidbody;
        //private Animator animator;

        private void Start()
        {
            playerRigidbody = GetComponent<Rigidbody2D>();
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

            playerRigidbody.linearVelocity = Vector2.zero;
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
            if (Input.GetKeyDown(KeyCode.UpArrow) && jumpCount < 2)
            {
                jumpCount++;
                playerRigidbody.linearVelocity = Vector3.zero;

                this.playerRigidbody.AddForce(new Vector3(0, jumpForce, 0));

                if (jumpCount == 2)
                {
                    //animator.SetTrigger("DoubleJump");
                }
            }
        }


    }
}



