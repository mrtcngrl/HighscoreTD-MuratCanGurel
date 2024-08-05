using System;
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

        private UserProgressDataManager _userProgressDataManager;
        [Inject]
        private void OnInject(UserProgressDataManager userProgressDataManager)
        {
            _userProgressDataManager = userProgressDataManager;
            foreach (var turretPurchaseButton in _purchaseButtons)
            { 
                turretPurchaseButton.Initialize(turretPurchaseButton.TurretProperties.StartPrice, OnPurchased);
            }

            GameConstants.OnDataRecover += OnDataRecover;
            GameConstants.OnRetry += OnRetry;
        }

        private void OnRetry()
        {
            foreach (var turretPurchaseButton in _purchaseButtons)
            { 
                turretPurchaseButton.Initialize(turretPurchaseButton.TurretProperties.StartPrice, OnPurchased);
            }

            foreach (var purchaseProgress in _userProgressDataManager.Progress.PurchaseProgressData.PurchaseProgresses)
            {
                purchaseProgress.PurchaseStep = 0;
            }
        }
        private void OnDestroy()
        {
            GameConstants.OnDataRecover -= OnDataRecover;
        }

        private void OnPurchased(TurretPurchaseButton purchaseButton)
        {
            var purchaseProgress = _userProgressDataManager.GetPurchaseDataById(purchaseButton.TurretProperties.ID);
            purchaseProgress.PurchaseStep++;
            var properties = purchaseButton.TurretProperties;
            int price =  properties.StartPrice + ( properties.PriceIncreaseAmount * purchaseProgress.PurchaseStep);
            purchaseButton.SetPrice(price);
        }

        private void OnDataRecover(UserProgressData data)
        {
            var purchaseProgresses = data.PurchaseProgressData.PurchaseProgresses;
            foreach (var purchaseProgress in purchaseProgresses)
            {
               var button = _purchaseButtons.FirstOrDefault(b => b.TurretProperties.ID == purchaseProgress.ID);
               if(button == null) continue;
               var properties = button.TurretProperties;
               int price = properties.StartPrice + ( properties.PriceIncreaseAmount * purchaseProgress.PurchaseStep);
               button.SetPrice(price);
            }
            _userProgressDataManager.Progress.PurchaseProgressData.PurchaseProgresses = purchaseProgresses.ToList();
        }
    }
}