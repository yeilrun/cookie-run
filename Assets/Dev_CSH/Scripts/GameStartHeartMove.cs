using UnityEngine;
using UnityEngine.UI;


namespace CSH
{
    public class GameStartHeartMove : MonoBehaviour
    {
        [SerializeField]public float fallspeed;
        [SerializeField]public float groundY;
        [SerializeField] private bool isfalling;

        public void Start()
        {
        //      transform.position+=Vector3.down*fallspeed*Time.deltaTime;
        }

        public void Update()
        {
            if (isfalling)
            {
                Vector3 currentPosition = transform.position;
               // transform.position+=Vector3.down*fallspeed*Time.deltaTime;
                currentPosition.y-=fallspeed*Time.deltaTime;
                transform.position=currentPosition;

                if (transform.position.y<=groundY)
                {
                    transform.position=new Vector3(currentPosition.x, groundY, currentPosition.z);
                    Destroy(gameObject);
                    isfalling=false;
                }
            }
        } 
     }
}