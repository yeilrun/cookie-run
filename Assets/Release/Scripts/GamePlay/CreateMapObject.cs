using System.Collections.Generic;
using UnityEngine;

namespace SHJ
{
    public class CreateMapObject : MonoBehaviour
    {
        [Header("지형"), SerializeField] private GameObject landPrefab;
        [Header("젤리"), SerializeField] private GameObject jellyPrefab;
        [Header("밑에있는 장애물 작은거"), SerializeField] private BoxCollider2D wallSmallB = null;
        // [Header("밑에있는 장애물 큰거"), SerializeField] private BoxCollider2D wall_Land_Large = null;
        // [Header("위에있는 장애물 작은거"), SerializeField] private BoxCollider2D wall_Sky_Small = null;
        // [Header("위에있는 장애물 큰거"), SerializeField] private BoxCollider2D wall_Sky_Large = null;

        [SerializeField, Range(1, 2)] private float moveSpeed = 2f;

        private int[] landWidths = new int[]
        {
            14, -2, 6, -2, 4, -2, 2, -2, 16, -2, 16, -2, 16
        };
        
        // 지형과 젤리 거리
        private float jHeight = 1.2f;
        
        // 젤리와 젤리 거리
        private float jOffset = 1f;

        // 랜드 생성후 젤리 생성 위치 오프셋
        private int jellyStart = 4;
        
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
            List<Vector2> jellyPosX = GroundInit();
            // 장애물을 놓으면서 Y 구하기
            List<Vector2> jellyPosXY = WallInit(jellyPosX);
            // 젤리 생성
            JellyInit(jellyPosXY);
        }

        private List<Vector2> GroundInit()
        {
            // 구덩이 갯수에따른 지형 생성 개수 달라짐
            // 하지만 구덩이 생성 패턴이 에피소드마다 다르고 거리마다 같은 패턴이면 안됨
            // 길이값이 들어있는 배열을 받는게 좋을거 같다
            
            SpriteRenderer originSprite = landPrefab.GetComponent<SpriteRenderer>();
            float startX = jellyStart;
            
            float plusX = 0;
            List<Vector2> jellyPosX = new List<Vector2>();
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

                    int idx = 0;
                    while (startX < plusX)
                    {
                        Vector2 addPos = new Vector2(startX, 0f);
                        if (landWidths[i] >= 16 && idx != 0 && idx % 5 == 0)
                        {
                            addPos.y = 1f;
                        }
                        jellyPosX.Add(addPos);
                        startX += jOffset;
                        ++idx;
                    }
                    
                }
                else
                {
                    float nonX = (originSprite.size.x * 2 * Mathf.Abs(landWidths[i])) * 0.5f;
                    plusX += nonX;
                    startX += nonX;
                }
            }
            
            return jellyPosX;
        }
        
        private List<Vector2> WallInit(List<Vector2> jellyPosX)
        {
            SpriteRenderer originSprite = landPrefab.GetComponent<SpriteRenderer>();
            List<Vector2> result = new List<Vector2>();
            foreach (Vector2 v in jellyPosX)
            {
                Vector2 jp = v;
                if (v.y > 0f && v.y <= 1f)
                {
                    Instantiate(wallSmallB, new Vector2(v.x, originSprite.bounds.max.y), Quaternion.identity, targetMap.transform);
                }

                jp.y = landPrefab.transform.position.y + jHeight;
                result.Add(jp);
            }
            return result;
        }

        private void JellyInit(List<Vector2> jellyPosXY)
        {
            foreach (Vector2 p in jellyPosXY)
            {
                Instantiate(jellyPrefab, p, Quaternion.identity, targetMap.transform);
            }
        }
    }
}

