using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HotForgeStudio.HorrorBox
{
    public class OnBehaviourHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler,
        IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public event Action<GameObject> MouseUpTriggered;

        public event Action<GameObject> MouseDownTriggered;

        public event Action<Collider2D> Trigger2DEntered;

        public event Action<Collider2D> Trigger2DExited;

        public event Action<Collider> TriggerEntered;

        public event Action<Collider> TriggerExited;

        public event Action<Collider> TriggerStaying;        

        public event Action<GameObject> Destroying;

        public event Action<PointerEventData> PointerEntered;

        public event Action<PointerEventData> PointerExited;

        public event Action<GameObject> Updating;

        public event Action<PointerEventData, GameObject> DragUpdated;

        public event Action<PointerEventData, GameObject> DragBegan;

        public event Action<PointerEventData, GameObject> DragEnded;

        public event Action<GameObject> OnParticleCollisionEvent;

        public event Action<PointerEventData> PointerDown;

        public event Action<PointerEventData> PointerUp;

        public void OnBeginDrag(PointerEventData eventData)
        {
            DragBegan?.Invoke(eventData, gameObject);
        }

        public void OnDrag(PointerEventData eventData)
        {
            DragUpdated?.Invoke(eventData, gameObject);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DragEnded?.Invoke(eventData, gameObject);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEntered?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExited?.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PointerDown?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerUp?.Invoke(eventData);
        }

        private void Update()
        {
            Updating?.Invoke(gameObject);
        }

        private void OnMouseUp()
        {
            MouseUpTriggered?.Invoke(gameObject);
        }

        private void OnMouseDown()
        {
            MouseDownTriggered?.Invoke(gameObject);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            Trigger2DExited?.Invoke(collider);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            Trigger2DEntered?.Invoke(collider);
        }

        private void OnTriggerExit(Collider collider)
        {
            TriggerExited?.Invoke(collider);
        }

        private void OnTriggerEnter(Collider collider)
        {
            TriggerEntered?.Invoke(collider);
        }

        private void OnTriggerStay(Collider collider)
        {
            TriggerStaying?.Invoke(collider);
        }

        private void OnDestroy()
        {
            Destroying?.Invoke(gameObject);
        }

        private void OnParticleCollision(GameObject other)
        {
            OnParticleCollisionEvent?.Invoke(other);
        }
    }
}
