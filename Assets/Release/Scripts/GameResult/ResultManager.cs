using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace LHA
{
    public class ResultManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private void Start()
        {
            scoreText.text = string.Format("{0:#,###} 점", SingletonManager.Instance.saveMyScore);
        }

        public void OnClickOkButton()
        {
            SceneManager.LoadScene("ReleaseGameMainScene");
        }
    }
}
