using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Source.Village.UI
{
    public abstract class BaseTooltip : MonoBehaviour
    {
        private RectTransform rectTransform;

        public virtual void Awake()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
        }

        public virtual void Hide()
        {
            Destroy(gameObject);
        }

        public void CalculateAnchor(Vector3 position)
        {
            var x = ChooseClosest(position.x / Screen.width, 0.2f, 0.8f);
            var y = ChooseClosest(position.y / Screen.height, 0.2f, 0.8f);
            Vector2 anchor = new Vector2(x, y);
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.pivot = anchor;
        }
        
        float ChooseClosest(float value, float a, float b)
        {
            return value;
            // return Mathf.Abs(value - a) < Mathf.Abs(value - b) ? a : b;
        }
    }
}