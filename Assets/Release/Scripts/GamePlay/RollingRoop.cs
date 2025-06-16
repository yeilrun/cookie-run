using UnityEngine;

namespace LHA
{
    public class RollingRoop : MonoBehaviour
    {
        [SerializeField] private float speed = 3f;
        private float width;

        public static bool isActive = true;

        private void Awake()
        {
            BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
            width = backgroundCollider.bounds.size.x;
        }

        private void Update()
        {
            if (isActive)
            {
                if (transform.position.x <= -width)
                {
                    Reposition();
                }
                transform.Translate( speed * Time.deltaTime * Vector3.left);
            }
        }
        private void Reposition()
        {
            Vector2 offset = new Vector2(width * 2f, 0);
            transform.position = (Vector2)transform.position + offset;
        }
    }
}