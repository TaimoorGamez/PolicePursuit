using UnityEngine;

namespace Core.GamePlay
{
    public class PoliceCarAI : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float stopTime = 1f;

        private float stopTimer = 0f;
        private bool isStopped = false;

        private void Update()
        {
            if (isStopped)
            {
                stopTimer -= Time.deltaTime;
                if (stopTimer <= 0f)
                    isStopped = false;

                return; // Skip movement while stopped
            }

            if (player == null) return;

            // Move towards player
            Vector2 dir = (player.position - transform.position).normalized;
            transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);

            // Face player
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Apply damage here
                // player.TakeDamage(...);

                // Stop for a moment
                isStopped = true;
                stopTimer = stopTime;
            }
        }
    }
}
