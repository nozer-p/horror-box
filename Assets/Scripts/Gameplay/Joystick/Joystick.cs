using HotForgeStudio.HorrorBox.Common;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HotForgeStudio.HorrorBox
{
    public class Joystick
    {
        private GameObject _selfObject;

        private float _handleRange = 1;
        private float _deadZone = 0;

        private Enumerators.AxisOptions _axisOptions;

        private bool _snapX;
        private bool _snapY;

        private RectTransform background;
        private RectTransform handle;

        private Canvas _canvas;
        private Camera _camera;

        private Vector2 _input;

        private bool _isUsing;

        private OnBehaviourHandler _onBehaviourHandler;

        public Action OnJoysticStopsBeingUsed;

        public float Horizontal
        {
            get => (_snapX) ? SnapFloat(_input.x, Enumerators.AxisOptions.Horizontal) : _input.x;
        }

        public float Vertical
        {
            get => (_snapY) ? SnapFloat(_input.y, Enumerators.AxisOptions.Vertical) : _input.y;
        }

        public Vector2 Direction
        {
            get => new Vector2(Horizontal, Vertical);
        }

        public bool IsUsing
        {
            get => _isUsing;
        }
        public float HandleRange
        {
            get => _handleRange;
        }

        public float DeadZone
        {
            get => _deadZone;
        }

        public Enumerators.AxisOptions AxisOptions
        {
            get => AxisOptions;
        }

        public bool SnapX
        {
            get => _snapX;
        }

        public bool SnapY
        {
            get => _snapY;
        }

        public bool IsActive
        {
            get => _selfObject.activeInHierarchy;
        }

        public Joystick(GameObject gameObject)
        {
            _selfObject = gameObject;

            _handleRange = 1;
            _deadZone = 0;
            _axisOptions = Enumerators.AxisOptions.Both;
            _input = Vector2.zero;

            background = _selfObject.GetComponent<RectTransform>();
            handle = _selfObject.transform.Find("Handle").GetComponent<RectTransform>();

            _onBehaviourHandler = _selfObject.GetComponent<OnBehaviourHandler>();

            _onBehaviourHandler.PointerDown += OnPointerDown;
            _onBehaviourHandler.PointerUp += OnPointerUp;
            _onBehaviourHandler.DragUpdated += OnDrag;
            _onBehaviourHandler.DragEnded += OnDragEnd;

            _canvas = GameClient.Get<IUIManager>().Canvas.GetComponent<Canvas>();

            Vector2 center = new Vector2(0.5f, 0.5f);
            background.pivot = center;
            handle.anchorMin = center;
            handle.anchorMax = center;
            handle.pivot = center;
            handle.anchoredPosition = Vector2.zero;
        }

        public void SetSnapX(bool value)
        {
            _snapX = value;
        }        
        
        public void SetSnapY(bool value)
        {
            _snapY = value;
        }

        public void SetAxisOptions(Enumerators.AxisOptions axisOptions)
        {
            _axisOptions = axisOptions;
        }

        public void SetDeadZone(float value)
        {
            _deadZone = Mathf.Abs(value);
        }

        public void SetHandleRange(float value)
        {
            _handleRange = Mathf.Abs(value);
        }

        private void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData, null);
        }

        private void OnDrag(PointerEventData eventData, GameObject gameObject)
        {
            _camera = null;
            if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
                _camera = _canvas.worldCamera;

            Vector2 position = RectTransformUtility.WorldToScreenPoint(_camera, background.position);
            Vector2 radius = background.sizeDelta / 2;
            _input = (eventData.position - position) / (radius * _canvas.scaleFactor);
            FormatInput();
            HandleInput(_input.magnitude, _input.normalized);
            handle.anchoredPosition = _input * radius * _handleRange;

            _isUsing = true;
        }

        private void OnDragEnd(PointerEventData eventData, GameObject gameObject)
        {
            _isUsing = false;

            OnJoysticStopsBeingUsed?.Invoke();
        }

        private void HandleInput(float magnitude, Vector2 normalised)
        {
            if (magnitude > _deadZone)
            {
                if (magnitude > 1)
                    _input = normalised;
            }
            else
                _input = Vector2.zero;
        }

        private void FormatInput()
        {
            if (_axisOptions == Enumerators.AxisOptions.Horizontal)
                _input = new Vector2(_input.x, 0f);
            else if (_axisOptions == Enumerators.AxisOptions.Vertical)
                _input = new Vector2(0f, _input.y);
        }

        private float SnapFloat(float value, Enumerators.AxisOptions snapAxis)
        {
            if (value == 0)
                return value;

            if (_axisOptions == Enumerators.AxisOptions.Both)
            {
                float angle = Vector2.Angle(_input, Vector2.up);
                if (snapAxis == Enumerators.AxisOptions.Horizontal)
                {
                    if (angle < 22.5f || angle > 157.5f)
                        return 0;
                    else
                        return (value > 0) ? 1 : -1;
                }
                else if (snapAxis == Enumerators.AxisOptions.Vertical)
                {
                    if (angle > 67.5f && angle < 112.5f)
                        return 0;
                    else
                        return (value > 0) ? 1 : -1;
                }
                return value;
            }
            else
            {
                if (value > 0)
                    return 1;
                if (value < 0)
                    return -1;
            }
            return 0;
        }

        private void OnPointerUp(PointerEventData eventData)
        {
            _input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
            _isUsing = false;

            OnJoysticStopsBeingUsed?.Invoke();
        }
    }
}