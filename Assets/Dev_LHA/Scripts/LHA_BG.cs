using UnityEngine;

public class LHA_BG : MonoBehaviour
{
    [SerializeField] private float movespeed = 3f;
    [SerializeField] private float Offset = 0f;

    private void Update()
    {
        Offset -= movespeed * Time.deltaTime;
        transform.position = new Vector2(Offset, transform.position.y);
    }
}
