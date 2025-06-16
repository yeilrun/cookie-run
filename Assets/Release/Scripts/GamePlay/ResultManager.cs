using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace LHA
{
    public class ResultManager : MonoBehaviour
    {
        [SerializeField] Button okButton;
        [SerializeField] private TextMeshProUGUI scoreText;
        private int score = 0;

        private void Start()
        {
            score = SingletonManager.Instance.saveMyScore;
            scoreText.text = string.Format("{0:#,###} 점", score);
        }

        public void OnClickOkButton()
        {
            SceneManager.LoadScene("ReleaseGameMainScene");
        }
    }
}
