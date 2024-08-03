using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.UI.Popup
{
    public abstract class PopupBase : MonoBehaviour
    {
        [SerializeField] protected Button PopupButton;
        [SerializeField] protected Transform BodyTransform;
        [SerializeField] private GameObject _dimmer;
        protected virtual void Awake()
        {
            PopupButton.onClick.AddListener(OnButtonClicked);
            BodyTransform.localScale = Vector3.zero;
            _dimmer.SetActive(false);
        }

        public virtual void ShowPopup()
        {
            _dimmer.SetActive(true);
            BodyTransform.DOScale(1, 0.3f).SetEase(Ease.OutElastic).OnComplete(() =>
            {
                PopupButton.interactable = true;
            });
        }

        protected virtual void HidePopup()
        {
            _dimmer.SetActive(false);
            PopupButton.interactable = false;
            BodyTransform.DOScale(0, 0.25f).SetEase(Ease.InOutBack).OnComplete(()=>
            {
                BodyTransform.localScale = Vector3.zero;
            });
        }

        protected virtual void OnButtonClicked()
        {
            HidePopup();
        }
    }
}