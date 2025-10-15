using UnityEngine;

namespace Core.GamePlay
{

    public class GroundTrigger : MonoBehaviour
    {
        [SerializeField] EndlessGroundHandler _endlessGroundHandler;

        float _tileDistance = 0.45f;

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Vector2 contactPoint = other.ClosestPoint(transform.position);
                Vector2 dir = (contactPoint - (Vector2)transform.position).normalized;
                int moveX = dir.x > _tileDistance ? 1 : dir.x < -_tileDistance ? -1 : 0;
                int moveY = dir.y > _tileDistance ? 1 : dir.y < -_tileDistance ? -1 : 0;
                if (moveX == 0 && moveY == 0)
                    return;

                //Debug.Log(moveX + " " + moveY);
                int centerTileIndex = GetTileIndexFromDirection(moveX, moveY);
                _endlessGroundHandler.RepositionTiles(centerTileIndex);
            }
        }

        private int GetTileIndexFromDirection(int moveX, int moveY)
        {
            int[,] indexMap = new int[3, 3]
            {
                { 0, 1, 2 },
                { 3, 4, 5 },
                { 6, 7, 8 } 
            };

            int gridX = 1 + moveX; 
            int gridY = 1 + moveY;

            return indexMap[gridY, gridX];
        }
    }
}
