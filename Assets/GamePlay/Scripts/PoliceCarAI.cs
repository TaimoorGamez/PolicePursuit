using UnityEngine;

namespace Core.GamePlay
{
    public class PoliceCarAI : DefenseVehicleAI
    {
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        override protected void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
        }
    }
}
