using System.Collections;
using UnityEngine;

namespace SHJ
{
    public class GameMainSecond : MonoBehaviour
    {
        public delegate IEnumerator PlayLoadingDelegate();
        public static event PlayLoadingDelegate playLoadingMainSecond;
        
        public void OnClickGamePlayScene()
        {
            StartCoroutine(playLoadingMainSecond?.Invoke());
        }
    }
}

