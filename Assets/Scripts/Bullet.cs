using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.CompareTag("Ghost"))
        {
            // FindObjectOfType<GameManager>().SumaPuntos();
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }
}
