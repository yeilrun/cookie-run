using System.Collections.Generic;
using UnityEngine;

namespace SHJ
{
    public class CreateMapObject : MonoBehaviour
    {
        [Header("지형"), SerializeField] private GameObject landPrefab;
        [Header("젤리"), SerializeField] private GameObject jellyPrefab;
        // [Header("밑에있는 장애물 작은거"), SerializeField] private BoxCollider2D wall_Land_Small = null;
        // [Header("밑에있는 장애물 큰거"), SerializeField] private BoxCollider2D wall_Land_Large = null;
        // [Header("위에있는 장애물 작은거"), SerializeField] private BoxCollider2D wall_Sky_Small = null;
        // [Header("위에있는 장애물 큰거"), SerializeField] private BoxCollider2D wall_Sky_Large = null;

        [SerializeField, Range(1, 2)] private float moveSpeed = 2f;

        private int[] landWidths = new int[]
        {
            16, -2, 6, -2, 4, -2, 2, -2, 10
        };
        
        // 지형과 젤리 거리
        // private float jHeight = 0.3f;
        
        // 젤리와 젤리 거리
        // private float jOffset = 0.4f;

        // 랜드 생성후 젤리 생성 위치 오프셋
        // private int jellyStart = 5;
        
        // 맵 길이
        // private int mapDistance = 200;
        
        // 부모로 사용할 게임 오브젝트
        private GameObject targetMap = null;
        
        private void Start()
        {
            MapInit();
        }

        private void Update()
        {
            if (targetMap != null)
            {
                targetMap.transform.position += (Time.deltaTime * moveSpeed * Vector3.left);
            }
        }

        private void MapInit()
        {
            targetMap = new GameObject();
            
            // 지형을 그리면서 X 구하기
            float[] jellyPosX = GroundInit();
            // 장애물을 놓으면서 Y 구하기
            Vector2[] jellyPosXY = WallInit(jellyPosX);
            // 젤리 생성
            JellyInit(jellyPosXY);
        }

        private float[] GroundInit()
        {
            // 구덩이 갯수에따른 지형 생성 개수 달라짐
            // 하지만 구덩이 생성 패턴이 에피소드마다 다르고 거리마다 같은 패턴이면 안됨
            // 길이값이 들어있는 배열을 받는게 좋을거 같다
            
            SpriteRenderer originSprite = landPrefab.GetComponent<SpriteRenderer>();

            float plusX = 0;
            for (int i = 0; i < landWidths.Length; ++i)
            {
                if (landWidths[i] > 0)
                {
                    GameObject cloneLand = Instantiate(landPrefab, targetMap.transform);
                    BoxCollider2D bc = cloneLand.GetComponent<BoxCollider2D>();
                    SpriteRenderer sp = cloneLand.GetComponent<SpriteRenderer>();
                    bc.size = new Vector2(originSprite.size.x * Mathf.Abs(landWidths[i]), originSprite.size.y); 
                    sp.size = bc.size;
                    if (i == 0)
                    {
                        cloneLand.transform.position = new Vector2(0, landPrefab.transform.position.y);
                        plusX = sp.bounds.size.x * 0.5f;
                    }
                    else
                    {
                        plusX += sp.bounds.size.x * 0.5f;
                        cloneLand.transform.position = new Vector2(plusX, landPrefab.transform.position.y);
                        plusX += sp.bounds.size.x * 0.5f;
                    }
                }
                else
                {
                    plusX += (originSprite.size.x * 2 * Mathf.Abs(landWidths[i])) * 0.5f;
                }
            }
            
            return new[] {0f};
        }
        
        private Vector2[] WallInit(float[] jellyPosX)
        {
            return new Vector2[] {Vector2.zero};
        }

        private void JellyInit(Vector2[] jellyPosXY)
        {
            
        }

        // private void CreateMap();
        // {
        //     // // 부모 오브젝트 생성
        //     // targetMap = new GameObject();
        //     //
        //     // // ground init
        //     // GameObject cloneLand = Instantiate(landPrefab, targetMap.transform);
        //     // SpriteRenderer spr = cloneLand.GetComponent<SpriteRenderer>();
        //     // BoxCollider2D bc2 = cloneLand.GetComponent<BoxCollider2D>();
        //     // float xDis = (spr.size.x * mapDistance);
        //     // float startX = (xDis * 0.5f * cloneLand.transform.localScale.x) - jellyStart;
        //     // targetMap.transform.position = new Vector3(
        //     //     startX, 
        //     //     targetMap.transform.position.y, 
        //     //     targetMap.transform.position.z
        //     //     );
        //     //
        //     // bc2.size = new Vector2(xDis, spr.size.y);
        //     // spr.size = new Vector2(xDis, spr.size.y);
        //     //
        //     // float x = jellyStart;
        //     // float y = (cloneLand.transform.position.y) + Mathf.Abs(cloneLand.transform.position.y) + height;
        //     //
        //     // // wall init
        //     // float lsYmaxY = landWallSmall.size.y + landWallSmall.offset.y;
        //     // List<float> lsY = MakePosY(y, lsYmaxY, offset);
        //     // int lsYindex = 0;
        //     // int halfIndex = (int)(lsY.Count * 0.5f) + 1;
        //     //
        //     // List<Vector2> wallPos = new List<Vector2>();
        //     // float limitX = (xDis * 0.5f) - (jellyStart * 2);
        //     // while (x < limitX)
        //     // {
        //     //     x += offset;
        //     //     if (lsYindex < lsY.Count)
        //     //     {
        //     //         wallPos.Add(new Vector2(x, lsY[lsYindex]));
        //     //         if (lsYindex == halfIndex)
        //     //         {
        //     //             Instantiate(landWallSmall, new Vector2(x - offset, y), Quaternion.identity, targetMap.transform);
        //     //         }
        //     //         ++lsYindex;
        //     //     }
        //     //     else
        //     //     {
        //     //         wallPos.Add(new Vector2(x, y));
        //     //     }
        //     // }
        //     //
        //     // // jelly init
        //     // foreach (Vector2 p in wallPos)
        //     // {
        //     //     Instantiate(jelly, p, Quaternion.identity, targetMap.transform);
        //     // }
        //     
        // }

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

