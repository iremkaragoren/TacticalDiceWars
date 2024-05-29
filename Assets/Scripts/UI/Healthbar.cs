using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Healthbar:MonoBehaviour
    {
        [SerializeField] private Image healthbarSprite;

        public void UpdateHealthBar(float maxHealth, float currentHealth)
        {
            healthbarSprite.fillAmount = currentHealth / maxHealth;
        }
    }
}