using System.Collections;
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
    }

    public void OnClickRotateItemList(bool b)
    {
        StartCoroutine(LoopScaleRotate(b));
    }

    private IEnumerator LoopScaleRotate(bool b)
    {
        GameObject targetGo = b ? itemListGo : rankListGo;  // disable target
        GameObject activeGo = !b ? itemListGo : rankListGo;  // enable target
        
        Vector3 originS = targetGo.transform.localScale;
        Quaternion originRot = targetGo.transform.localRotation;
        
        Vector3 s = targetGo.transform.localScale;
        float t = Time.deltaTime * 10;
        float r = targetGo.transform.rotation.y;
        for (int i = 0; i < 80; ++i)
        {
            if (i <= 10)
            {
                targetGo.transform.localScale = new Vector3(s.x - t, s.y - t, s.z - t);
                s = targetGo.transform.localScale;
            }
            else
            {
                targetGo.transform.Rotate(new Vector3(0, r + 2f, 0));
            }
            yield return null;
        }

        activeGo.transform.localScale = targetGo.transform.localScale;
        activeGo.transform.localRotation = targetGo.transform.localRotation;
        
        targetGo.transform.localScale = originS;
        targetGo.transform.localRotation = originRot;
        targetGo.transform.parent.gameObject.SetActive(false);
        
        activeGo.transform.parent.gameObject.SetActive(true);
        for (int i = 0; i < 80; ++i)
        {
            if (i <= 10)
            {
                activeGo.transform.localScale = new Vector3(s.x + t, s.y + t, s.z + t);
                s = activeGo.transform.localScale;
            }
            else
            {
                activeGo.transform.Rotate(new Vector3(0, r - 2f, 0));
            }
            yield return null;
        }
        activeGo.transform.localScale = originS;
        activeGo.transform.localRotation = originRot;
    }
}
