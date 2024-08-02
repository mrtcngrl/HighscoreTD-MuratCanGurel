using System.Collections.Generic;
using System.Linq;
using Scripts.User;
using UnityEngine;
using Zenject;

namespace Scripts.Game.UI.TurretPurchaseSystem
{
    public class TurretPurchaseMenu : MonoBehaviour
    {
        [SerializeField] private List<TurretPurchaseButton> _purchaseButtons;

        [Inject]
        private void OnInject(UserProgressData userProgressData)
        {
            foreach (var turretPurchaseButton in _purchaseButtons)
            {
                
            }
        }

        private void GetButtonById(int id)
        {
            var purchaseButton = _purchaseButtons.FirstOrDefault(p => p.TurretProperties.ID == id);
        }

        private void OnPurchased(int id)
        {
            
        }
    }
}