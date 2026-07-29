using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Core.GamePlay
{
    public class DefenseSpawner : MonoBehaviour
    {
        [SerializeField] DefenseVehicleAI _vehiclePrefab;
        [SerializeField] Transform PlayerVehicle;
        [SerializeField] int MaxVehicles = 5;
        [SerializeField] float SpawnInterval = 3f, SpawnDistanceBehind = 10f, LateralOffset = 3f;
        [SerializeField] bool IsEndless;

        float _nextSpawnTime;
        string _defenseVehiclePath => "Defense/Vehicle ";
        readonly List<DefenseVehicleAI> _activeVehicles = new List<DefenseVehicleAI>();
        Coroutine _spawnCoroutine;

            private void OnEnable()
            {
                _spawnCoroutine = StartCoroutine(SpawnRoutine());
            }

            private void OnDisable()
            {
                if (_spawnCoroutine != null)
                {
                    StopCoroutine(_spawnCoroutine);
                    _spawnCoroutine = null;
                }
            }

            private IEnumerator SpawnRoutine()
            {
                while (true)
                {
                    yield return new WaitForSeconds(SpawnInterval);

                    if (IsEndless || _activeVehicles.Count < MaxVehicles)
                    {
                        SpawnDefenseVehicle();
                    }
                }
            }

        private void SpawnDefenseVehicle()
        {
            if (PlayerVehicle == null)
                return;

            // Spawn position behind player
            Vector3 spawnPos = PlayerVehicle.position
                - PlayerVehicle.up * SpawnDistanceBehind
                + PlayerVehicle.right * Random.Range(-LateralOffset, LateralOffset);

            Quaternion spawnRot = Quaternion.Euler(0, 0, PlayerVehicle.eulerAngles.z);
            DefenseVehicleAI newVehicle = Instantiate(_vehiclePrefab, spawnPos, spawnRot, transform);
            newVehicle.PlayerVehicle = PlayerVehicle;

            _activeVehicles.Add(newVehicle);
        }
    }
}
