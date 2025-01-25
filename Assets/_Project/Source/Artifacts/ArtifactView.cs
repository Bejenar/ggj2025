using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Source.Artifacts
{
    public class ArtifactView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public TMP_Text nameText;
        public TMP_Text description;

        private IArtifact _item;

        public void Init(IArtifact artifact)
        {
            _item = artifact;
            UpdateView();
        }

        public void UpdateView()
        {
            nameText.text = _item.Name;
            description.text = _item.Description;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOKill(true);
            transform.DOScale(Vector3.one * 1.1f, 0.1f).SetEase(Ease.OutBack);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOKill(true);
            transform.DOScale(Vector3.one, 0.1f);
        }
    }
}