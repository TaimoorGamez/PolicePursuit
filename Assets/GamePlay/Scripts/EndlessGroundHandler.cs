using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Core.GamePlay
{
    public class EndlessGroundHandler : MonoBehaviour
    {
        [SerializeField] Transform _groundTrigger;
        [SerializeField] GroundTile _tilePrefab;
        [SerializeField] int _gridSize = 3;

        bool _isShifting = false;
        float _repositionDelay = 0.01f;
        int _gridCenter;
        Vector2 _tileSize;
        Dictionary<int, GroundTile> _groundTiles;
        Coroutine _shiftCoroutine;

        void OnEnable()
        {
            _tileSize = _tilePrefab.TileSprite.bounds.size;
            _gridCenter = _gridSize / 2;
            GenerateGroundGrid();
        }

        void GenerateGroundGrid()
        {
            _groundTiles = new Dictionary<int, GroundTile>();
            for (int i = 0, y = 0; y < _gridSize; y++)
            {
                for (int x = 0; x < _gridSize; x++)
                {
                    float posX = (x - _gridCenter) * _tileSize.x;
                    float posY = (y - _gridCenter) * _tileSize.y;

                    Vector3 spawnPos = new Vector3(posX, posY, 0);
                    GroundTile newTile = Instantiate(_tilePrefab, spawnPos, Quaternion.identity, transform);
                    newTile.TileIndex = i;
                    _groundTiles.Add(i, newTile);
                    i++;
                    if (i >= _gridSize * _gridSize)
                        break;
                }
            }
        }

        public void RepositionTiles(int centerTileIndex)
        {
            if (_isShifting)
                return;

            _isShifting = true;
            if (_shiftCoroutine != null)
                StopCoroutine(_shiftCoroutine);

            if(gameObject.activeInHierarchy)
            _shiftCoroutine = StartCoroutine(RepositionTilesSmooth(centerTileIndex));
        }

        public IEnumerator RepositionTilesSmooth(int centerTileIndex)
        {
            Vector3 centerPos = _groundTiles[centerTileIndex].transform.position;
            Stack<GroundTile> availableTiles = new Stack<GroundTile>();
            for (int c = 0; c < _groundTiles.Count; c++)
            {
                if (c != centerTileIndex)
                    availableTiles.Push(_groundTiles[c]);
            }
            GroundTile centerTile = _groundTiles[centerTileIndex];
            _groundTiles.Clear();
            _groundTiles = new Dictionary<int, GroundTile>();
            for (int i = 0, y = 0; y < _gridSize; y++)
            {
                for (int x = 0; x < _gridSize; x++)
                {
                    GroundTile newTile = null;
                    if (x == _gridCenter && y == _gridCenter)
                    {
                        newTile = centerTile;
                        _groundTrigger.position = newTile.transform.position;
                    }
                    else
                    {
                        Vector3 targetPos = new Vector3(
                        centerPos.x + (x - _gridCenter) * _tileSize.x,
                        centerPos.y + (y - _gridCenter) * _tileSize.y,
                        0f
                        );
                        newTile = availableTiles.Pop();
                        newTile.transform.position = targetPos;
                    }
                    newTile.TileIndex = i;
                    _groundTiles.Add(i, newTile);
                    i++;
                    if (i >= _gridSize * _gridSize)
                        break;
                }
            }
            yield return new WaitForSeconds(_repositionDelay);

            _isShifting = false;
        }
    }
}
