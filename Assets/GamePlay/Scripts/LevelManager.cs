using UnityEngine;
using Core.Events;
using Core.DB.Variables;
using System.Collections;

namespace Core.GamePlay
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] SOEvents InitLevelEvent;
        [SerializeField] DBInt ActiveCarIndex;
        [SerializeField] Transform CarHolder;

        string _carsPath => $"Cars/Car {ActiveCarIndex.Value}";
        GameObject _currentCar;
        Coroutine _loadRoutine;

        private void Start()
        {
            LoadCar();
        }

        public void LoadCar()
        {
            // Stop any previous coroutine if still running
            if (_loadRoutine != null)
                StopCoroutine(_loadRoutine);

            _loadRoutine = StartCoroutine(LoadCarAsync());
        }

        private IEnumerator LoadCarAsync()
        {
            // Optional: small delay if you’re switching cars rapidly
            yield return null;

            // Clean up old car if any
            if (_currentCar != null)
                Destroy(_currentCar);

            // Begin async load
            ResourceRequest request = Resources.LoadAsync<GameObject>(_carsPath);
            yield return request;

            // Check if asset is valid
            if (request.asset == null)
            {
                Debug.LogError($"Car prefab not found at: {_carsPath}");
                yield break;
            }

            // Instantiate car as child of CarHolder
            GameObject prefab = request.asset as GameObject;
            _currentCar = Instantiate(prefab, CarHolder);

            _loadRoutine = null;
        }
    } 
}
