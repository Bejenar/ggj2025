using System.Collections.Generic;
using System.Linq;
using _Project.Data;
using _Project.Source.Artifacts;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace _Project.Source.Village.UI
{
    public class ShopItemView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public TMP_Text price;
        public TMP_Text nameText;
        public TMP_Text description;
        
        public bool canAfford => G.state.gold >= _item.Cost;
        public bool purchased = false;
        
        private IShopItem _item;
        
        private List<Material> _materials; 
        
        private void Start()
        {
            _materials = new List<Material>(GetComponentsInChildren<Image>().Select(i =>
            {
                var mat = new Material(i.material);
                i.material = mat;
                return mat;
            }));
        }

        public void Init(IShopItem item)
        {
            _item = item;
            UpdateView();
        }
        public void UpdateView()
        {
            nameText.text = _item.Name;
            description.text = _item.Description;
            price.text = $"${_item.Cost}";
            var color = canAfford ? ColorUtils.ToRGBA(0xFABC76) : Color.gray;
            color.a = 1;
            price.color = color;
        }

        public async void OnPointerClick(PointerEventData eventData)
        {
            if (canAfford)
            {
                G.state.gold -= _item.Cost;
                G.battleScene.UpdateGoldView();
                await _item.Purchase();
                purchased = true;
                
                G.shopManager.RemoveShopItem(this);
                G.shopManager.UpdateShopViews();
                G.battleScene.UpdateGoldView();
                Dissolve();
                
                await UniTask.WaitForSeconds(1.2f);
                Destroy(gameObject);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (purchased) return;
            
            if (canAfford)
            {
                transform.DOKill(true);
                transform.DOScale(Vector3.one * 1.1f, 0.1f).SetEase(Ease.OutBack);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (purchased) return;
            
            transform.DOKill(true);
            transform.DOScale(Vector3.one, 0.1f);
        }
        
        public void Dissolve()
        {
            Debug.Log("Dissolving");

            price.transform.DOScale(Vector3.zero, 0.2f);
            nameText.transform.DOScale(Vector3.zero, 0.2f);
            description.transform.DOScale(Vector3.zero, 0.2f);
            G.audio.Play<BurningSFX>();
            foreach (var mat in _materials)
            {
                mat.DOFloat(0, "_Dissolve", 1f).OnComplete(() => Destroy(gameObject));
            }
        }
    }
}