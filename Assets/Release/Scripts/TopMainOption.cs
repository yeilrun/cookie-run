using UnityEngine;
using UnityEngine.UI;

namespace CSH
{
    public class TopMainOption : MonoBehaviour
    {
        [SerializeField] private GameObject targetModalGo;

        private void Start()
        {
            Button myButton = GetComponent<Button>();      
            myButton.onClick.AddListener(OnClickOptionModal);
        }

        private void OnClickOptionModal()
        {
            targetModalGo.SetActive(true);
        }
    }
    
}

