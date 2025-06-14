using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    public struct Token
    {
        public string token;
        public Token(string token)
        {
            this.token = token;
        }
    }
        
    public static Token sToken = new Token();
}
