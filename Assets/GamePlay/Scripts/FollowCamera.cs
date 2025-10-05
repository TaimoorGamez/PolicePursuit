using UnityEngine;

namespace Core.GamePlay
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform _targetedCar;
        [SerializeField] Vector3 _carOffset = new Vector3(0, 5, -10);
        [SerializeField] bool _bankingOnTurns = false;
        [SerializeField] float _maxBankAngle = 15f, _bankSpeed = 2f;         // How fast camera tilts

        float _currentBank = 0f, _recenterSpeed;

        private void OnEnable()
        {
            _recenterSpeed = _bankSpeed * 2f;
        }

        void LateUpdate()
        {
            transform.position = _targetedCar.position + _carOffset;

            if (_bankingOnTurns)
            {
                float turnInput = Input.GetAxis("Horizontal"); 
                float targetBank = turnInput != 0 ? Mathf.Clamp(turnInput * _maxBankAngle, -_maxBankAngle, _maxBankAngle) : 0f;

                float lerpSpeed = turnInput != 0 ? _bankSpeed : _recenterSpeed;
                _currentBank = Mathf.Lerp(_currentBank, targetBank, lerpSpeed * Time.deltaTime);

                transform.rotation = Quaternion.Euler(0f, 0f, _currentBank);
            }
        }
    }
}