using UnityEngine;

namespace Enemy
{
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField] private string EnemyLayer = "Enemy";
        private float lastDamageTime = -0.5f;
        private float damageCooldown = 0.5f; 

        private void OnCollisionEnter(Collision other)
        {
            TryDealDamage(other.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            TryDealDamage(other.gameObject);
        }

        private void TryDealDamage(GameObject target)
        {

            if (Time.time - lastDamageTime >= damageCooldown)
            {
                int enemyLayerIndex = LayerMask.NameToLayer(EnemyLayer);

                if (target.layer == enemyLayerIndex)
                {
                    if (target.transform.TryGetComponent(out IDamageble warrior))
                    {

                        warrior.TakeDamage(1);
                        warrior.ChangeMeshColor(Color.red);
                        lastDamageTime = Time.time; 
                    }
                }
            }
        }
    }
}