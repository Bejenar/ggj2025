using System;
using System.Collections.Generic;
using _Project.Data;
using _Project.Source.Util;
using _Project.Source.Village.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityUtils;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace _Project.Source.Artifacts
{
    public class ShopManager : MonoBehaviour
    {
        public Dictionary<int, IShopItem> shopItems = new Dictionary<int, IShopItem>();
        public List<int> ItemBag = new List<int>();
        
        
        public Transform shopPanel;
        public ShopItemView shopItemPrefab;
        public Transform shopPanelContainer;

        private List<ShopItemView> _shopItems = new List<ShopItemView>();

        public void UpdateShopViews()
        {
            _shopItems.ForEach(si => si.UpdateView());
        }
        
        public void RemoveShopItem(ShopItemView shopItemView)
        {
            _shopItems.Remove(shopItemView);
            
            if (_shopItems.Count == 0)
            {
                G.battleScene.NextLevel();
            }
        }
        
        public async UniTask EnterShop()
        {
            _shopItems.Clear();

            await shopPanel.DOLocalMove(Vector3.zero, 1.0f).SetEase(Ease.InOutBack);

            foreach (var randomItem in G.shopManager.GetRandomItems(3))
            {
                var itemView = Object.Instantiate(shopItemPrefab, shopPanelContainer);
                itemView.transform.localScale = Vector3.zero;
                itemView.Init(randomItem);
                _shopItems.Add(itemView);
                G.audio.Play<ButtonSFX>();
                await itemView.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
            }
        }

        public async UniTask CloseShop()
        {
            await DissolveShopItems();
            
            await UniTask.WaitForSeconds(1f);
            await shopPanel.DOLocalMove(new Vector3(0, -1500, 0), 1.0f).SetEase(Ease.InOutBack);
            shopPanelContainer.DestroyChildren();
            _shopItems.Clear();
        }

        public async UniTask DissolveShopItems()
        {
            foreach (var itemView in _shopItems)
            {
                itemView.Dissolve();
            }

            await UniTask.CompletedTask;
        }

        private void Start()
        {
            G.shopManager = this;
            
            var depression = new ColorScoreModifier(2, CardColor.NO_COLOR, "Depression");
            var burningPassion = new ColorScoreModifier(10, CardColor.RED, "Burning Passion");

            shopItems.Add(0, new ArtifactShopItem(depression, 5, 5));
            shopItems.Add(1, new ArtifactShopItem(burningPassion, 10, 1));
            shopItems.Add(2,
                new PermanentlyAddHandSizeShopItem(4, 1, 5, "Hand Size+", "Permanently increase hand size by 1"));
            shopItems.Add(3,
                new PermanentlyAddHandSizeShopItem(8, 2, 2, "Hand Size++", "Permanently increase hand size by 2"));
            shopItems.Add(4,
                new PermanentlyAddMaxPlaysShopItem(5, 1, 3, "Plays+", "Permanently increase amount of plays by 1"));
            shopItems.Add(5,
                new PermanentlyAddMaxPlaysShopItem(10, 2, 1, "Plays++", "Permanently increase amount of plays by 2"));
            shopItems.Add(6,
                new PermanentlyAddMaxDiscardsShopItem(4, 1, 4, "Discards+",
                    "Permanently increase amount of discards by 1"));
            shopItems.Add(7,
                new PermanentlyAddMaxDiscardsShopItem(8, 2, 2, "Discards++",
                    "Permanently increase amount of discards by 1"));
            
            InitBag();
        }

        public void InitBag()
        {
            foreach (var keyValuePair in shopItems)
            {
                var id = keyValuePair.Key;
                var item = keyValuePair.Value;
                for (int i = 0; i < item.ScarcityCoef; i++)
                    ItemBag.Add(id);
            }
        }

        public List<IShopItem> GetRandomItems(int amount)
        {
            var items = new List<IShopItem>();
            var uniqueIds = new HashSet<int>();

            while (items.Count < amount && uniqueIds.Count < ItemBag.Count)
            {
                var item = GetRandomItem();
                if (uniqueIds.Add(item.GetHashCode()))
                {
                    items.Add(item);
                }
            }

            return items;
        }

        public IShopItem GetRandomItem()
        {
            var id = ItemBag[Random.Range(0, ItemBag.Count)];
            return shopItems[id];
        }

        public void ClearBag(int id)
        {
            ItemBag.RemoveAll(x => x == id);
        }
    }
}