using Scripts.Helpers;
using UnityEngine;

namespace Scripts.Game.Controllers
{
    public class SelectionController
    {
        private readonly TouchHandler _touchHandler;
        private ISelectable _selectedItem;
        private readonly Camera _camera;

        public SelectionController()
        {
            Input.multiTouchEnabled = false;
            _touchHandler = TouchHandler.Instance;
            _camera = Camera.main;
            Subscribe();
        }
        
        private void Subscribe()
        {
            ClearTouchEvents();
            //_touchHandler.OnTouchBeginEvent += PressDown;
            _touchHandler.OnTouchMovedEvent = PressMoved;
            _touchHandler.OnTouchEndedEvent = PressUp;
        }

        private void ClearTouchEvents()
        {
            _touchHandler.OnTouchBeginEvent = null;
            _touchHandler.OnTouchMovedEvent = null;
            _touchHandler.OnTouchEndedEvent = null;
        }
        
        ~SelectionController()
        {
            ClearTouchEvents();
        }
        
        private void PressMoved(Touch touch)
        {
            if (_selectedItem == null) return;
            _selectedItem.OnHold(MouseWorldPosition(touch, _selectedItem.Position));
        }
        
        private void PressUp(Touch touch)
        {
            Debug.LogError("Pressed Up");
            if(_selectedItem == null) return;
            _selectedItem.OnRelease(out bool placed);
            if (placed)
                _selectedItem = null;

        }
        
        private Vector3 MouseWorldPosition(Touch touch, Vector3 selectedCoinPos)
        {
            Vector3 mouseScreenPos = touch.position;
            mouseScreenPos.z = _camera.WorldToScreenPoint(selectedCoinPos).z;
            return _camera.ScreenToWorldPoint(mouseScreenPos);
        }
        public void SetSelectable(ISelectable selectable)
        {
            _selectedItem = selectable;
        }
    }

    public interface ISelectable
    {
        public Vector3 Position { get; }
        bool Available { get;}
        void OnHold(Vector3 mouseWorldPos);
        void OnRelease(out bool placed);
    }
}