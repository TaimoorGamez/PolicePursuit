using UnityEngine;

namespace Core.GamePlay
{

    public class GroundTile : MonoBehaviour
    {
        public EndlessGroundHandler EndlessGroundHandler;
        public SpriteRenderer TileSprite;

        [SerializeField] BoxCollider2D _tileCollider;

        bool _canTrigger = false;
        int _tileIndex;

        public void ActiveTile(int tileIndex)
        {
            _tileIndex = tileIndex;
            _tileCollider.enabled = true;
            _canTrigger = true;
        }

        public void MakeCenterTile(int tileIndex)
        {
            _tileIndex = tileIndex;
            _tileCollider.enabled = false;
            _canTrigger = false;
        }   

        void OnTriggerEnter2D(Collider2D other)
        {
            if (_canTrigger)
            {
                _canTrigger = false;
                if (other.CompareTag("Player"))
                {
                    _tileCollider.enabled = false;
                    EndlessGroundHandler.RepositionTiles(_tileIndex);
                }
            }
        }
    }
}
