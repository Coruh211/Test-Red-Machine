using Events;
using Levels;
using Player.ActionHandlers;
using UnityEngine;
using Utils.Scenes;

namespace Camera.ScrollLogic
{
    public class CameraScrollLogic : MonoBehaviour
    {
        [SerializeField] private float smoothTime = 0.3f;
        [SerializeField] private float swipeSensitivity = 0.01f;
        [SerializeField] private bool invertControl = true;
        
        private Vector3 _velocity = Vector3.zero;
        private Vector3 _targetPosition;
        private Vector3 _startPosition;
        private Transform _topRightBoundary;
        private Transform _bottomLeftBoundary;
        private ClickHandler _clickHandler;
        private Vector3 _defaultPosition;
        private bool _canScroll;

        private void Awake()
        {
            _targetPosition = transform.position;
            _defaultPosition = transform.position;
            _clickHandler = ClickHandler.Instance;
            
            ScenesChanger.SceneLoadedEvent += OnSceneLoaded;
            LevelsManager.LevelCompleteEvent += OnLevelComplete;
        }

        private void OnSceneLoaded()
        {
            if (TrySetBoundaries())
            {
                EventsController.Subscribe<EventModels.Game.NodeTapped>(this, OnNodeTapped);
                EventsController.Subscribe<EventModels.Game.PlayerScrolled>(this, OnPlayerScrolled);
                
                _clickHandler.PointerDownEvent += OnPointerDown;
                _clickHandler.PointerUpEvent += OnPointerUp;
            }
        }
        
        private bool TrySetBoundaries()
        {
            CameraScrollBordersHolder cameraScrollBordersHolder = FindObjectOfType<CameraScrollBordersHolder>();
            
            if(cameraScrollBordersHolder != null)
            {
                _topRightBoundary = cameraScrollBordersHolder.TopRightBoundary;
                _bottomLeftBoundary = cameraScrollBordersHolder.BottomLeftBoundary;
            }
            
            return _topRightBoundary != null && _bottomLeftBoundary != null;
        }
        
        private void OnPlayerScrolled(EventModels.Game.PlayerScrolled obj) => 
            _canScroll = true;

        private void OnNodeTapped(EventModels.Game.NodeTapped obj) => 
            _canScroll = false;
        
        private void OnPointerDown(Vector3 position)
        {
            if(!_canScroll)
            {
                return;
            }
            
            _startPosition = position;
        }

        private void OnPointerUp(Vector3 endPosition)
        {
            if(!_canScroll)
            {
                return;
            }
            
            Vector3 swipeDirection = endPosition - _startPosition;
            
            if (invertControl)
            {
                swipeDirection = -swipeDirection;
            }
            
            _targetPosition = new Vector3(
                transform.position.x + swipeDirection.x * swipeSensitivity,
                transform.position.y + swipeDirection.y * swipeSensitivity,
                transform.position.z
            );
            
            _targetPosition = ClampPositionWithinBounds(_targetPosition);
        }
        
        private Vector3 ClampPositionWithinBounds(Vector3 targetPos)
        {
            if(targetPos == Vector3.zero)
            {
                return targetPos;
            }

            return new Vector3(
                Mathf.Clamp(targetPos.x, _bottomLeftBoundary.position.x, _topRightBoundary.position.x),
                Mathf.Clamp(targetPos.y, _bottomLeftBoundary.position.y, _topRightBoundary.position.y),
                transform.position.z);
        }
        
        private void LateUpdate()
        {
            transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _velocity, smoothTime);
        }
        
        private void OnLevelComplete()
        {
            _canScroll = false;
            _bottomLeftBoundary = null;
            _topRightBoundary = null;
            _targetPosition = _defaultPosition;
            
            _clickHandler.PointerDownEvent -= OnPointerDown;
            _clickHandler.PointerUpEvent -= OnPointerUp;
            
            EventsController.Unsubscribe<EventModels.Game.NodeTapped>(OnNodeTapped);
            EventsController.Unsubscribe<EventModels.Game.PlayerScrolled>(OnPlayerScrolled);
        }
    }
}