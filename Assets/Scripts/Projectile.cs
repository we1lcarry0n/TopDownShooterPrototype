using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileLife;
    [SerializeField] private int projectileDamageAmount;

    [SerializeField] private TrailRenderer projectileTrail;

    private Rigidbody2D rb2d;

    private void OnEnable()
    {
        projectileTrail.Clear();
    }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Test");
        if (collision.gameObject.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(projectileDamageAmount);
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
    }

    public void SetTarget(Vector3 target)
    {
        rb2d.velocity = target * projectileSpeed;
        StartCoroutine(ProjectileLifeCoroutine(projectileLife));
    }

    private IEnumerator ProjectileLifeCoroutine(float life)
    {
        yield return new WaitForSeconds(life);
        rb2d.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
