using UnityEngine;
using Core.Events;

namespace Core.GamePlay
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] SOEvents SteeringReleaseEvent, TurnLeftEvent, TurnRightEvent;

        [Header("References")]
        [SerializeField] private Rigidbody2D _carRigidbody;

        [Header("Base Stats")]
        [SerializeField] private float _baseAcceleration = 3f;
        [SerializeField] private float _baseMaxSpeed = 8f;
        [SerializeField] private float _baseTurnPower = 100f;
        [SerializeField] private float _baseHealth = 100f;

        [Header("Upgrade Levels")]
        [SerializeField] private int _accelerationLevel = 0;
        [SerializeField] private int _speedLevel = 0;
        [SerializeField] private int _turnLevel = 0;
        [SerializeField] private int _healthLevel = 0;

        [Header("Runtime Stats")]
        private float _acceleration;
        private float _maxSpeed;
        private float _turnPower;
        private float _maxHealth;
        private float _currentHealth;

        private float _currentSpeed;
        private float _steerInput;

        private void OnEnable()
        {
            TurnLeftEvent.EventHandler += OnLeftDown;
            TurnRightEvent.EventHandler += OnRightDown;
            SteeringReleaseEvent.EventHandler += OnSteerUp;
        }

        private void OnDisable()
        {
            TurnLeftEvent.EventHandler -= OnLeftDown;
            TurnRightEvent.EventHandler -= OnRightDown;
            SteeringReleaseEvent.EventHandler -= OnSteerUp;
        }

        private void Start()
        {
            if (_carRigidbody == null)
                _carRigidbody = GetComponent<Rigidbody2D>();

            ApplyUpgrades();
            _currentHealth = _maxHealth;
        }

        private void FixedUpdate()
        {
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, _maxSpeed, _acceleration * Time.fixedDeltaTime);
            _carRigidbody.linearVelocity = transform.up * _currentSpeed;

            float rotationAmount = -_steerInput * _turnPower * Time.fixedDeltaTime * (_currentSpeed / _maxSpeed);
            _carRigidbody.MoveRotation(_carRigidbody.rotation + rotationAmount);
        }

        void OnLeftDown()
        {
            _steerInput = -1f;
        }

        void OnRightDown()
        {
            _steerInput = 1f; 
        }

        void OnSteerUp()
        {
            _steerInput = 0f;
        }

        public void ApplyUpgrades()
        {
            _acceleration = _baseAcceleration + _accelerationLevel * 0.5f;
            _maxSpeed = _baseMaxSpeed + _speedLevel * 1.0f;
            _turnPower = _baseTurnPower + _turnLevel * 20f;
            _maxHealth = _baseHealth + _healthLevel * 25f;
        }

        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;

            if (_currentHealth <= 0)
            {

            }
        }

        public void RepairCar(float amount)
        {
            _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        }
    }
}
