using UnityEngine;
namespace CSH
{
   
    public class ShrikingRotation : MonoBehaviour
    {
        public float shrinkSpeed = 0.01f;
        public float rotationSpeed = 10f;
        private Vector3 initialScale;

        public void Start()
        {
            initialScale = transform.localScale;
        }

        public void Update() { 
            //작아지는 것
            transform.localScale=new Vector3(shrinkSpeed,shrinkSpeed,shrinkSpeed);

            //회전
            transform.Rotate(0f,rotationSpeed*Time.deltaTime,0f);

            //초기값으로 리셋
            if (transform.localScale.x<=0)
            {
                transform.localScale=initialScale;
            }


        }
    }}