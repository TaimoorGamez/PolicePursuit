using UnityEngine;

namespace Core.GamePlay
{
    public class CarController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D _carRigidbody;

        [Header("Base Stats")]
        [SerializeField] private float _baseEnginePower = 5f;
        [SerializeField] private float _baseAcceleration = 3f;
        [SerializeField] private float _baseMaxSpeed = 8f;
        [SerializeField] private float _baseTurnPower = 100f;
        [SerializeField] private float _baseHealth = 100f;

        [Header("Upgrade Levels")]
        [SerializeField] private int _engineLevel = 0;
        [SerializeField] private int _accelerationLevel = 0;
        [SerializeField] private int _speedLevel = 0;
        [SerializeField] private int _turnLevel = 0;
        [SerializeField] private int _healthLevel = 0;

        [Header("Runtime Stats")]
        private float _enginePower;
        private float _acceleration;
        private float _maxSpeed;
        private float _turnPower;
        private float _maxHealth;
        private float _currentHealth;

        private float _currentSpeed;
        private float _steerInput;

        private void Start()
        {
            if (_carRigidbody == null)
                _carRigidbody = GetComponent<Rigidbody2D>();

            ApplyUpgrades();
            _currentHealth = _maxHealth;
        }

        private void Update()
        {
            // Use A/D or Left/Right arrow keys for steering
            _steerInput = Input.GetAxis("Horizontal");
        }

        private void FixedUpdate()
        {
            // Move car forward based on acceleration and engine
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, _maxSpeed, _acceleration * Time.fixedDeltaTime);
            _carRigidbody.linearVelocity = transform.up * _currentSpeed;

            // Turning control depends on current speed and upgrade
            float rotationAmount = -_steerInput * _turnPower * Time.fixedDeltaTime * (_currentSpeed / _maxSpeed);
            _carRigidbody.MoveRotation(_carRigidbody.rotation + rotationAmount);
        }

        public void ApplyUpgrades()
        {
            _enginePower = _baseEnginePower + _engineLevel * 0.5f;
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
                Debug.Log("Car destroyed!");
                // Add restart or explosion logic here
            }
        }

        public void RepairCar(float amount)
        {
            _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        }
    }
}
