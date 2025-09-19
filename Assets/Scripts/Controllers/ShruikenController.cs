using UnityEngine;

public class ShruikenController : MonoBehaviour
{
    private void Update()
    {
        if(gameObject.activeInHierarchy)
        {
            transform.Rotate(0f, 0f, 20 * Time.deltaTime);
        }
    }

    public void LaunchShruiken(Vector2 dir)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * 75;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("SideColliders"))
        {
            gameObject.SetActive(false);
        }
    }
}
