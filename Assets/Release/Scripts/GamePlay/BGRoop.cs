using UnityEngine;

namespace LHA
{
    public class BGRoop : MonoBehaviour
    {
        private float width;
        private float speed = 0.3f;

        private void Awake()
        {
            BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
            width = backgroundCollider.size.x;
        }

        private void Update()
        {
            if (transform.position.x <= -width)
            {
                Reposition();
            }
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        private void Reposition()
        {
            Vector2 offset = new Vector2(width * 2f, 0);
            transform.position = (Vector2)transform.position + offset;
        }
    }
}