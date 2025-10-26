using UnityEngine;

namespace Core.GamePlay
{
    public class DefenseVehicleAI : MonoBehaviour
    {
        public Transform PlayerVehicle;
        public float MoveSpeed = 5f, VehicleHealth = 100f, VehicleDamage = 20;

        protected float stopTime = 0.2f, stopTimer = 0f, turnSpeed;
        protected bool canMove = true, isAlive = true;

        private void Start()
        {
            turnSpeed = MoveSpeed * 4;
        }

        protected virtual void FixedUpdate()
        {
            if(!isAlive || PlayerVehicle == null)
                return;

            if (!canMove)
            {
                stopTimer -= Time.deltaTime;
                if (stopTimer <= 0f)
                { canMove = true; }
                else { return; }
            }

            transform.position += transform.up * MoveSpeed * Time.deltaTime;

            // --- Smooth rotation towards player ---
            Vector2 direction = (PlayerVehicle.position - transform.position).normalized;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.fixedDeltaTime
            );
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                stopTimer = stopTime;
                canMove = false;
            }
        }
    }
}
