using UnityEngine;

namespace SHJ
{
    public class CreateMapObject : MonoBehaviour
    {
        [SerializeField] private GameObject landPrefab;
        [SerializeField] private GameObject jellyPrefab;
        [SerializeField] private GameObject wallPrefab;

        [SerializeField, Range(0, 1)] private float jHeight;
        [SerializeField, Range(0, 1)] private float jOffset;

        [SerializeField, Range(1, 2)] private float moveSpeed = 2f;
        
        private GameObject targetMap = null;
        
        private void Start()
        {
            if (landPrefab != null && jellyPrefab != null)
            {
                CreateMap(landPrefab, jellyPrefab, wallPrefab, jHeight, jOffset);
            }
        }

        private void Update()
        {
            if (targetMap != null)
            {
                targetMap.transform.position += (Time.deltaTime * moveSpeed * Vector3.left);
            }
        }

        private void CreateMap(GameObject land, GameObject jelly, GameObject wall, float height, float offset)
        {
            targetMap = new GameObject();
            
            GameObject cloneLand = Instantiate(land, targetMap.transform);
            SpriteRenderer spr = cloneLand.GetComponent<SpriteRenderer>();
            BoxCollider2D bc2 = cloneLand.GetComponent<BoxCollider2D>();
            float xDis = (spr.size.x * 100);
            float startX = (xDis * 0.5f * cloneLand.transform.localScale.x) - 3f;
            targetMap.transform.position = new Vector3(
                startX, 
                targetMap.transform.position.y, 
                targetMap.transform.position.z
                );
            
            bc2.size = new Vector2(xDis, spr.size.y);
            spr.size = new Vector2(xDis, spr.size.y);

            float x = 0;
            float y = (cloneLand.transform.position.y) + Mathf.Abs(cloneLand.transform.position.y) + height;
            float limitX = xDis * 0.5f;
            while (x < limitX)
            {
                x += offset;
                Vector3 pos = Vector3.zero;
                pos.x = x;
                pos.y = y;
                Instantiate(jelly, pos, Quaternion.identity, targetMap.transform);
            }
        }
    }
}

