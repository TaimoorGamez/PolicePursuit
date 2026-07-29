using UnityEngine;
using Core.Events;

namespace Core.GamePlay
{
    public class DefenseVehicleAI : MonoBehaviour
    {
        public Transform PlayerVehicle;
        public float MoveSpeed = 5f, VehicleHealth = 100f, VehicleDamage = 20;
        
        [SerializeField] SOEvents SteeringReleaseEvent, TurnLeftEvent, TurnRightEvent;

        protected float stopTime = 0.2f, stopTimer = 0f, turnSpeed, currentSpeed;
        protected bool canMove = true, isAlive = true;

        private void OnEnable()
        {
            TurnLeftEvent.EventHandler += OnSteerDown;
            TurnRightEvent.EventHandler += OnSteerDown;
            SteeringReleaseEvent.EventHandler += OnSteerUp;
        }

        private void OnDisable()
        {
            TurnLeftEvent.EventHandler -= OnSteerDown;
            TurnRightEvent.EventHandler -= OnSteerDown;
            SteeringReleaseEvent.EventHandler -= OnSteerUp;
        }

        private void Start()
        {
            turnSpeed = MoveSpeed * 4;
            currentSpeed = MoveSpeed;
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

            // --- Smooth rotation towards player ---
            Vector2 direction = (PlayerVehicle.position - transform.position).normalized;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.fixedDeltaTime
            );
            
            float speed = currentSpeed;
            transform.position += transform.up * speed * Time.deltaTime;
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") || other.CompareTag("Respawn"))
            {
                stopTimer = stopTime;
                canMove = false;
                gameObject.SetActive(false);
            }
        }

        void OnSteerDown()
        {
            currentSpeed *= 0.92f; 
        }

        void OnSteerUp()
        {
            currentSpeed = MoveSpeed;
        }
    }
}
