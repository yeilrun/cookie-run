using System.Collections;
using CSH;
using SHJ;
using UnityEngine;

public class LoadingMainGameManager : MonoBehaviour
{
    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject main1;

    [SerializeField] private GameObject itemListGo;
    [SerializeField] private GameObject rankListGo;
    
    private void OnEnable()
    {
        GameLoading.loginCallback += AddLoginCallback;
    }

    private void OnDisable()
    {
        GameLoading.loginCallback -= AddLoginCallback;
    }

    private void AddLoginCallback()
    {
        loading.SetActive(false);
        main1.SetActive(true);
        GameMainFirst gameMainFirst = main1.GetComponent<GameMainFirst>();
        GameLoading gameLoding = loading.GetComponent<GameLoading>();
        gameMainFirst.GetDataRequest(gameLoding.sToken.token);
    }

    public void OnClickRotateItemList(bool b)
    {
        GameObject targetGo = b ? itemListGo : rankListGo;  // disable target
        GameObject activeGo = !b ? itemListGo : rankListGo;  // enable target
        StartCoroutine(LoopScaleRotate(targetGo, activeGo));
    }

    private IEnumerator LoopScaleRotate(GameObject targetGo, GameObject activeGo)
    {
        Vector3 originS = Vector3.one;
        Quaternion originRot = Quaternion.identity;
        WaitForSeconds wait  = new WaitForSeconds(0.01f);
        // 작아진다
        float s_sub = 0.06f;
        float r_plus = 6f;
        for (int i = 0; i < 10; ++i)
        {
            Vector3 s = targetGo.transform.localScale;
            s.x = (s.x - s_sub);
            s.y = (s.y - s_sub);
            s.z = (s.z - s_sub);
            targetGo.transform.localScale = s;
            yield return wait;
        }
        // 돌린다
        for (int i = 0; i < 10; ++i)
        {
            Quaternion r = targetGo.transform.localRotation;
            r.y = (r.y + r_plus);
            targetGo.transform.Rotate(new Vector3(r.x, r.y, r.z));
            yield return wait;
        }
        
        activeGo.transform.localScale = targetGo.transform.localScale;
        activeGo.transform.localRotation = targetGo.transform.localRotation;
        yield return wait;
        
        targetGo.transform.localScale = originS;
        targetGo.transform.localRotation = originRot;
        targetGo.transform.parent.gameObject.SetActive(false);
        activeGo.transform.parent.gameObject.SetActive(true);
        yield return wait;
        
        // 돌린다
        for (int i = 0; i < 10; ++i)
        {
            Quaternion r = activeGo.transform.localRotation;
            r.y = (r.y - r_plus);
            activeGo.transform.Rotate(new Vector3(r.x, r.y, r.z));
            yield return wait;
        }
        
        // 커진다
        for (int i = 0; i < 10; ++i)
        {
            Vector3 s = activeGo.transform.localScale;
            s.x = (s.x + s_sub);
            s.y = (s.y + s_sub);
            s.z = (s.z + s_sub);
            activeGo.transform.localScale = s;
            yield return wait;
        }
        
        activeGo.transform.localScale = originS;
        activeGo.transform.localRotation = originRot;
    }
}
