using System.Collections.Generic;
using UnityEngine;

namespace SHJ
{
    public class CreateMapObject : MonoBehaviour
    {
        [Header("지형"), SerializeField] private GameObject landPrefab;
        [Header("젤리"), SerializeField] private GameObject jellyPrefab;
        
        [Header("밑에있는 장애물 작은거"), SerializeField] private BoxCollider2D wall_Land_Small = null;
        [Header("밑에있는 장애물 큰거"), SerializeField] private BoxCollider2D wall_Land_Large = null;
        
        [Header("위에있는 장애물 작은거"), SerializeField] private BoxCollider2D wall_Sky_Small = null;
        [Header("위에있는 장애물 큰거"), SerializeField] private BoxCollider2D wall_Sky_Large = null;

        [Header("지형과 젤리 거리")]
        [SerializeField, Range(0, 1)] 
        private float jHeight;
        
        [Header("젤리와 젤리 거리")]
        [SerializeField, Range(0, 1)] 
        private float jOffset;

        [Header("랜드 생성후 젤리 생성 위치 오프셋")]
        [SerializeField, Range(5, 10)]
        private int jellyStart;
        
        [Header("맵 길이")]
        [SerializeField, Range(100, 200)] 
        private int mapDistance;
        
        [SerializeField, Range(1, 2)] private float moveSpeed = 2f;
        
        private GameObject targetMap = null;
        private int playLevel = 1;
        
        private void Start()
        {
            if (landPrefab != null && jellyPrefab != null)
            {
                CreateMap(
                    landPrefab, 
                    jellyPrefab, 
                    wall_Land_Small, 
                    wall_Land_Large,
                    wall_Sky_Small,
                    wall_Sky_Large,
                    jHeight, 
                    jOffset,
                    playLevel
                    );
            }
        }

        private void Update()
        {
            if (targetMap != null)
            {
                targetMap.transform.position += (Time.deltaTime * moveSpeed * Vector3.left);
            }
        }

        private void CreateMap(
            GameObject land, 
            GameObject jelly, 
            BoxCollider2D landWallSmall, 
            BoxCollider2D landWallLarge, 
            BoxCollider2D skyWallSmall, 
            BoxCollider2D skyWallLarge, 
            float height, 
            float offset,
            int playLevel
            )
        {
            targetMap = new GameObject();
            
            // ground init
            GameObject cloneLand = Instantiate(land, targetMap.transform);
            SpriteRenderer spr = cloneLand.GetComponent<SpriteRenderer>();
            BoxCollider2D bc2 = cloneLand.GetComponent<BoxCollider2D>();
            float xDis = (spr.size.x * mapDistance);
            float startX = (xDis * 0.5f * cloneLand.transform.localScale.x) - jellyStart;
            targetMap.transform.position = new Vector3(
                startX, 
                targetMap.transform.position.y, 
                targetMap.transform.position.z
                );
            
            bc2.size = new Vector2(xDis, spr.size.y);
            spr.size = new Vector2(xDis, spr.size.y);
            
            float x = jellyStart;
            float y = (cloneLand.transform.position.y) + Mathf.Abs(cloneLand.transform.position.y) + height;

            // wall init
            float lsYmaxY = landWallSmall.size.y + landWallSmall.offset.y;
            List<float> lsY = MakePosY(y, lsYmaxY, offset);
            int lsYindex = 0;
            int halfIndex = (int)(lsY.Count * 0.5f) + 1;
            
            List<Vector2> wallPos = new List<Vector2>();
            float limitX = (xDis * 0.5f) - (jellyStart * 2);
            while (x < limitX)
            {
                x += offset;
                if (lsYindex < lsY.Count)
                {
                    wallPos.Add(new Vector2(x, lsY[lsYindex]));
                    if (lsYindex == halfIndex)
                    {
                        Instantiate(landWallSmall, new Vector2(x - offset, y), Quaternion.identity, targetMap.transform);
                    }
                    ++lsYindex;
                }
                else
                {
                    wallPos.Add(new Vector2(x, y));
                }
            }

            // jelly init
            foreach (Vector2 p in wallPos)
            {
                Instantiate(jelly, p, Quaternion.identity, targetMap.transform);
            }
            
        }

        private List<float> MakePosY(float minY, float maxY, float offset)
        {
            float startY = minY;
            List<float> posY = new List<float>();
            posY.Add(minY);
            while (startY < maxY)
            {
                startY += (offset * 0.5f);
                posY.Add(startY);
            }
            List<float> reverseY = new List<float>(posY);
            reverseY.Add(maxY + (offset * 0.5f));
            posY.Reverse();
            reverseY.AddRange(posY);
            
            return reverseY;
        }
    }
}

