using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Core.GamePlay
{

    public class EndlessGroundHandler : MonoBehaviour
    {
        [SerializeField] SpriteRenderer _tilePrefab;
        [SerializeField] Transform _groundTrigger;
        [SerializeField] int _gridSize = 3;

        bool _isShifting = false;
        Vector2 _tileSize;
        List<List<Transform>> _tiles;
        Coroutine _shiftCoroutine;

        void OnEnable()
        {
            _tileSize = _tilePrefab.bounds.size;
            GenerateGroundGrid();
        }

        void GenerateGroundGrid()
        {
            _tiles = new List<List<Transform>>();
            for (int y = 0; y < _gridSize; y++)
            {
                List<Transform> row = new List<Transform>();
                for (int x = 0; x < _gridSize; x++)
                {
                    float posX = (x - _gridSize / 2) * _tileSize.x;
                    float posY = (y - _gridSize / 2) * _tileSize.y;

                    Vector3 spawnPos = new Vector3(posX, posY, 0);
                    row.Add(Instantiate(_tilePrefab, spawnPos, Quaternion.identity, transform).transform);
                }
                _tiles.Add(row);
            }
        }

        public void RepositionTiles(int moveX, int moveY)
        {
            if (!_isShifting)
            {
                _isShifting = true;
                _shiftCoroutine = StartCoroutine(RepositionTilesSmooth(moveX, moveY));
            }
        }

        public IEnumerator RepositionTilesSmooth(int moveX, int moveY)
        {
            if (moveY != 0)
            {
                int tileFrom = moveY > 0 ? 0 : _gridSize - 1; // Column leaving the view
                int tileTo = moveY > 0 ? _gridSize - 1 : 0;   // Opposite side

                List<Transform> row = _tiles[tileFrom];
                _tiles.RemoveAt(tileFrom);

                // Move all tiles in this column to the new world position
                for (int r = 0; r < row.Count; r++)
                {
                    Vector3 pos = row[r].position;
                    pos.y += moveY * _tileSize.y * _gridSize;
                    row[r].position = pos;
                }

                // Add the column back at the opposite side
                _tiles.Insert(tileTo, row);
            }

            // Horizontal shift
            if (moveX != 0)
            {
                int tileFrom = moveX > 0 ? 0 : _gridSize - 1; // Column leaving the view
                int tileTo = moveX > 0 ? _gridSize - 1 : 0;   // Opposite side

                for (int c = 0; c < _gridSize; c++)
                {
                    // Pop the tile from the column leaving
                    Transform tileToMove = _tiles[c][tileFrom];
                    _tiles[c].RemoveAt(tileFrom);

                    // Move the tile to the new position
                    Vector3 pos = tileToMove.position;
                    pos.x += moveX * _tileSize.x * _gridSize;
                    tileToMove.position = pos;

                    // Add the tile back to the opposite column
                    _tiles[c].Insert(tileTo, tileToMove); // insert at same row index
                }
            }
            _groundTrigger.position = _tiles[1][1].position;
            yield return new WaitForSeconds(1.1f);
            _isShifting = false;
            if(_shiftCoroutine != null)
            {
                StopCoroutine(_shiftCoroutine);
                _shiftCoroutine = null;
            }
        }
    }
}
