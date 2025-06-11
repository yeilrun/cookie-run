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
            //�۾����� ��
            transform.localScale=new Vector3(shrinkSpeed,shrinkSpeed,shrinkSpeed);

            //ȸ��
            transform.Rotate(0f,rotationSpeed*Time.deltaTime,0f);

            //�ʱⰪ���� ����
            if (transform.localScale.x<=0)
            {
                transform.localScale=initialScale;
            }


        }
    }}