using UnityEngine;

namespace Core.GamePlay
{

    public class GroundTriggerHandler : MonoBehaviour
    {
        [SerializeField] EndlessGroundHandler _endlessGroundHandler;
        [SerializeField] Rigidbody2D _playerRb;

        void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) 
                return;

            Vector2 moveDir = _playerRb.linearVelocity.normalized;
            int moveX = moveDir.x > 0.1f ? 1 : moveDir.x < -0.1f ? -1 : 0;
            int moveY = moveDir.y > 0.1f ? 1 : moveDir.y < -0.1f ? -1 : 0;
            if (moveX != 0 || moveY != 0)
                _endlessGroundHandler.RepositionTiles(moveX, moveY);

        }
    }
}
