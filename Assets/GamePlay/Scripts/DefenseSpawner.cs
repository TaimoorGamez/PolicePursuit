using UnityEngine;
using System.Collections.Generic;

namespace Core.GamePlay
{
    public class DefenseSpawner : MonoBehaviour
    {
        [SerializeField] Transform PlayerVehicle;
        [SerializeField] List<int> DefenseVehiclesIndex;
        [SerializeField] int MaxVehicles = 5;
        [SerializeField] float SpawnInterval = 3f, SpawnDistanceBehind = 10f, LateralOffset = 3f;

        float _nextSpawnTime;
        string _defenseVehiclePath => "Defense/Vehicle ";
        readonly List<DefenseVehicleAI> _activeVehicles = new List<DefenseVehicleAI>();

        private void Update()
        {
            // Clean null entries (destroyed enemies)
            _activeVehicles.RemoveAll(e => e == null);

            // Spawn if under max limit
            if (Time.time >= _nextSpawnTime && _activeVehicles.Count < MaxVehicles)
            {
                SpawnDefenseVehicle();
                _nextSpawnTime = Time.time + SpawnInterval;
            }
        }

        private void SpawnDefenseVehicle()
        {
            if (DefenseVehiclesIndex == null || DefenseVehiclesIndex.Count == 0 || PlayerVehicle == null)
                return;

            // Pick a random vehicle index from list
            int randomIndex = DefenseVehiclesIndex[Random.Range(0, DefenseVehiclesIndex.Count)];
            string vehiclePath = _defenseVehiclePath + randomIndex;

            // Load the prefab from Resources
            DefenseVehicleAI prefab = Resources.Load<DefenseVehicleAI>(vehiclePath);
            if (prefab == null)
            {
                Debug.LogWarning($"Could not load vehicle at path: {vehiclePath}");
                return;
            }

            // Spawn position behind player
            Vector3 spawnPos = PlayerVehicle.position
                - PlayerVehicle.up * SpawnDistanceBehind
                + PlayerVehicle.right * Random.Range(-LateralOffset, LateralOffset);

            Quaternion spawnRot = Quaternion.Euler(0, 0, PlayerVehicle.eulerAngles.z);
            DefenseVehicleAI newVehicle = Instantiate(prefab, spawnPos, spawnRot, transform);
            newVehicle.PlayerVehicle = PlayerVehicle;

            _activeVehicles.Add(newVehicle);
        }
    }
}
