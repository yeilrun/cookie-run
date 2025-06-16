using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHJ
{
    public class CreateMapObject : MonoBehaviour
    {
        [Header("지형"), SerializeField] private GameObject landPrefab;
        [Header("젤리"), SerializeField] private GameObject jellyPrefab;
        [Header("밑에있는 장애물 작은거"), SerializeField] private SpriteRenderer wallSmallB = null;
        [Header("밑에있는 장애물 큰거"), SerializeField] private SpriteRenderer wallLargeB = null;
        // [Header("위에있는 장애물 작은거"), SerializeField] private BoxCollider2D wall_Sky_Small = null;
        // [Header("위에있는 장애물 큰거"), SerializeField] private BoxCollider2D wall_Sky_Large = null;

        [SerializeField, Range(1, 10)] private float moveSpeed = 3f;

        private int[] landWidths = new int[]
        {
            10, 6, 6, -2, 4, -2, 12, -2, 12, -2, 16, -2, 16
        };
        
        // 지형과 젤리 거리
        private float jHeight = 2f;
        
        // 젤리와 젤리 거리
        private float jOffset = 1f;

        // 랜드 생성후 젤리 생성 위치 오프셋
        private int jellyStart = 4;
        
        // 맵 길이
        // private int mapDistance = 200;
        float plusX = 0;
        
        // 부모로 사용할 게임 오브젝트
        private GameObject targetMap = null;
        private GameObject targetMapRolling = null;

        public static bool isActive = true;

        private void Start()
        {
            MapInit();
        }

        private void Update()
        {
            if (isActive)
            {
                if (targetMap.transform.position.x <= -plusX)
                {
                    Vector2 offset = new Vector2(plusX * 2f, 0);
                    targetMap.transform.position = (Vector2)targetMap.transform.position + offset;
                    Transform[] arr = targetMap.GetComponentsInChildren<Transform>(true);
                    StartCoroutine(JellySetActive(arr));
                }
                else if (targetMapRolling.transform.position.x <= -plusX)
                {
                    Vector2 offset = new Vector2(plusX * 2f, 0);
                    targetMapRolling.transform.position = (Vector2)targetMapRolling.transform.position + offset;
                    Transform[] arr = targetMapRolling.GetComponentsInChildren<Transform>(true);
                    StartCoroutine(JellySetActive(arr));
                }
                targetMap.transform.position += (Time.deltaTime * moveSpeed * Vector3.left);
                targetMapRolling.transform.position += (Time.deltaTime * moveSpeed * Vector3.left);
            }

        }

        private IEnumerator JellySetActive(Transform[] arr)
        {
            foreach (Transform obj in arr)
            {
                obj.gameObject.SetActive(true);
                yield return null;
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

            Vector2 rollingMapPos = new Vector2(
                plusX, 
                targetMap.transform.position.y);
            targetMapRolling = Instantiate(targetMap, rollingMapPos, Quaternion.identity);
        }

        private List<Vector2> GroundInit()
        {
            // 구덩이 갯수에따른 지형 생성 개수 달라짐
            // 하지만 구덩이 생성 패턴이 에피소드마다 다르고 거리마다 같은 패턴이면 안됨
            // 길이값이 들어있는 배열을 받는게 좋을거 같다
            
            SpriteRenderer originSprite = landPrefab.GetComponent<SpriteRenderer>();
            float startX = jellyStart;
            
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
                        if (landWidths[i] == 6 && idx != 0 && idx % 3 == 0)
                        {
                            addPos.y = 1f;
                        }
                        else if (landWidths[i] == 12 && idx != 0 && idx % 4 == 0)
                        {
                            addPos.y = 2f;
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
                    jp.y = originSprite.bounds.max.y + (wallSmallB.bounds.max.y * 0.25f) + jHeight;
                    jp.x = (v.x - (jOffset * 0.55f));
                    result.Add(jp);
                    jp.x = (v.x + (jOffset * 0.55f));
                    result.Add(jp);
                    
                    jp.y = originSprite.bounds.max.y + wallSmallB.bounds.max.y + jHeight;
                    jp.x = (v.x - (jOffset * 0.25f));
                    result.Add(jp);
                    jp.x = (v.x + (jOffset * 0.25f));
                    result.Add(jp);
                }
                else if (v.y > 1f && v.y <= 2f)
                {
                    Instantiate(wallLargeB, new Vector2(v.x, originSprite.bounds.max.y), Quaternion.identity, targetMap.transform);
                    jp.y = originSprite.bounds.max.y + (wallLargeB.bounds.max.y * 0.15f) + jHeight;
                    jp.x = (v.x - (jOffset * 0.8f));
                    result.Add(jp);
                    jp.x = (v.x + (jOffset * 0.8f));
                    result.Add(jp);
                    
                    jp.y = originSprite.bounds.max.y + (wallLargeB.bounds.max.y * 0.6f) + jHeight;
                    jp.x = (v.x - (jOffset * 0.5f));
                    result.Add(jp);
                    jp.x = (v.x + (jOffset * 0.5f));
                    result.Add(jp);
                    
                    jp.y = originSprite.bounds.max.y + wallLargeB.bounds.max.y + jHeight;
                    jp.x = (v.x - (jOffset * 0.25f));
                    result.Add(jp);
                    jp.x = (v.x + (jOffset * 0.25f));
                    result.Add(jp);
                }
                else
                {
                    jp.y = landPrefab.transform.position.y + jHeight;
                    result.Add(jp);
                }

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

