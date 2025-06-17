using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace LHA
{
    public class CookieController : MonoBehaviour
    {
        public delegate void OnCookieIsClashCallback(GameObject cookie, GameObject target);
        public static OnCookieIsClashCallback onCookieIsClashCallback;

        private LayerMask groundLayer;
        private int jumpCount = 0;
        private float jumpTime = 0f;
        private float jumpVelocity = 13f;
        private float gravity = -40f;
        private bool isJumping = false;
        private float startY = 0f;

        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private Camera cam;
        private AudioSource audioSource;

        [SerializeField] AudioClip jumpAudio;
        [SerializeField] AudioClip slidingAudio;

        private Vector3 cameraOriginalPos;
        
        private bool isDead = false;

        private float originY = 0f;

        private void Start()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
            originY = transform.position.y;
            groundLayer = LayerMask.GetMask("Ground");
            animator.SetBool("Grounded", false);
            cam = Camera.main;
            cameraOriginalPos = cam.transform.position;
        }

        private void Update()
        {
            if (isDead) return;

            bool isHitDead = Grounding();
            Jumping(isHitDead);
            Sliding();
        }

        public void Die()
        {
            isDead = true;
            animator.SetTrigger("Die");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "wall")
            {
                animator.SetBool("Clash", true);
                StartCoroutine(Blink());
                StartCoroutine(CameraShake(0.3f, 0.1f));
            }

            onCookieIsClashCallback?.Invoke(gameObject, other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "wall")
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

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                audioSource.PlayOneShot(slidingAudio);
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                animator.SetBool("Slide", false);
            }
        }

        private void Jumping(bool _isHitDead)
        {
            if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
            {
                jumpCount++;
                jumpTime = 0f;
                startY = transform.position.y;
                isJumping = true;
                animator.SetBool("Grounded", false);
                animator.SetTrigger("Jump");
                audioSource.PlayOneShot(jumpAudio);

                if (jumpCount == 2)
                {
                    animator.SetTrigger("DoubleJump");
                }
            }

            if (isJumping)
            {
                jumpTime += Time.deltaTime;
                float newY = startY + jumpVelocity * jumpTime + 0.5f * gravity * Mathf.Pow(jumpTime, 2);
                transform.position = new Vector2(transform.position.x, newY);

                if (gravity * jumpTime + jumpVelocity <= 0f && _isHitDead)
                {
                    transform.position = new Vector2(transform.position.x, originY);
                    //transform.position = new Vector2(transform.position.x, Mathf.Round(transform.position.y * 100f) / 100f);
                    isJumping = false;
                }
            }

            if (!isJumping && _isHitDead)
            {
                jumpCount = 0;
                animator.SetBool("Grounded", true);
            }


        }

        private bool Grounding()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);

            if (!isJumping && hit.collider == null)
            {
                Vector2 v2 = new Vector2(transform.position.x, transform.position.y + (Vector2.down.y * Time.deltaTime * 10f));
                transform.position = v2;
            }

            return hit.collider != null;
        }

        private IEnumerator Blink()
        {
            for (int i = 0; i < 5; i++)
            {
                Color c = spriteRenderer.color;
                c.a = 0.3f;
                spriteRenderer.color = c;

                yield return new WaitForSeconds(0.2f);

                c.a = 1f;
                spriteRenderer.color = c;

                yield return new WaitForSeconds(0.2f);
            }
        }

        public IEnumerator CameraShake(float duration, float magnitude)
        {
            float timer = 0;

            while (timer <= duration)
            {
                cam.transform.localPosition = Random.insideUnitSphere * magnitude + cameraOriginalPos;

                timer += Time.deltaTime;
                yield return null;
            }

            cam.transform.localPosition = cameraOriginalPos;
        }


    }
}