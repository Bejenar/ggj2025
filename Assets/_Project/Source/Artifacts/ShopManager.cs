using System.Collections.Generic;
using _Project.Data;
using _Project.Source.Util;
using _Project.Source.Village.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Engine.Math;
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

        public List<ShopItemView> _shopItemViews = new List<ShopItemView>();

        public void UpdateShopViews()
        {
            _shopItemViews.ForEach(si => si.UpdateView());
        }

        public void RemoveShopItem(ShopItemView shopItemView)
        {
            _shopItemViews.Remove(shopItemView);

            if (_shopItemViews.Count == 0)
            {
                G.battleScene.nextLevelButton.gameObject.SetActive(false);
                G.battleScene.NextLevel();
            }
        }

        public async UniTask EnterShop()
        {
            _shopItemViews.Clear();

            G.battleScene.nextLevelButton.gameObject.SetActive(true);
            await shopPanel.DOLocalMove(Vector3.zero, 1.0f).SetEase(Ease.InOutBack);

            foreach (var randomItem in G.shopManager.GetRandomItems(3))
            {
                var itemView = Instantiate(shopItemPrefab, shopPanelContainer);
                itemView.transform.localScale = Vector3.zero;
                itemView.Init(randomItem.Item2, randomItem.Item1);
                _shopItemViews.Add(itemView);
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
            _shopItemViews.Clear();
        }

        public async UniTask DissolveShopItems()
        {
            foreach (var itemView in _shopItemViews)
            {
                itemView.Dissolve();
            }

            await UniTask.CompletedTask;
        }

        private void Start()
        {
            G.shopManager = this;

            var depression = new ColorScoreModifier(4, CardColor.NO_COLOR, "<color=#ded4d3>Depression</color>");
            var blue = new ColorScoreModifier(3, CardColor.BLUE, "I'm <color=#00B6F7>Blue</color>");
            var bulkUp = new AdditionalScore(2, "<color=#00B6F7>Bubblemancer</color>");
            var burningPassion = new ColorScoreModifier(10, CardColor.RED, "Burning Passion".Color("#EA4345"));
            var goldOnMonochrome = new AddGoldOnCombo(5, new MonochromeCombo(), "Greed".Color("#ded4d3"));
            var multPerDiscard = new MultPerDiscard(0.5f, "Tactician".Color("#EA4345"));

            shopItems.Add(0, new ArtifactShopItem(depression, 5, 6));
            shopItems.Add(1, new ArtifactShopItem(burningPassion, 10, 2));
            shopItems.Add(2,
                new PermanentlyAddHandSizeShopItem(4, 1, 6, "Hand Size+", "Permanently increase hand size by 1"));
            shopItems.Add(3,
                new PermanentlyAddHandSizeShopItem(8, 2, 3, "Hand Size++", "Permanently increase hand size by 2"));
            shopItems.Add(4,
                new PermanentlyAddMaxPlaysShopItem(5, 1, 4, "Plays+", "Permanently increase amount of plays by 1"));
            shopItems.Add(5,
                new PermanentlyAddMaxPlaysShopItem(10, 2, 2, "Plays++", "Permanently increase amount of plays by 2"));
            shopItems.Add(6,
                new PermanentlyAddMaxDiscardsShopItem(4, 1, 5, "Discards+",
                    "Permanently increase amount of discards by 1"));
            shopItems.Add(7,
                new PermanentlyAddMaxDiscardsShopItem(8, 2, 3, "Discards++",
                    "Permanently increase amount of discards by 2"));
            shopItems.Add(8, new ArtifactShopItem(goldOnMonochrome, 7, 3));
            shopItems.Add(9, new ArtifactShopItem(multPerDiscard, 7, 3));
            shopItems.Add(10,
                new AddGoldShopItem(10, 2, "<color=#FABC76>Risky investment</color>",
                    "Get <color=#FABC76>$10</color> if this is your <b>only</b> purchase of the round"));
            shopItems.Add(11, new AddCardsToDeckShopItem(5, 2, 3, CardColor.RED, "We drink your blood".Color(CardColor.RED.ToColor())));
            shopItems.Add(12, new AddCardsToDeckShopItem(4, 1, 3, CardColor.YELLOW, "Sunny Pasion".Color(CardColor.YELLOW.ToColor())));
            shopItems.Add(13, new AddCardsToDeckShopItem(3, 1, 3, CardColor.BLUE, "Lapis Lazuli".Color(CardColor.BLUE.ToColor())));
            shopItems.Add(14, new AddCardsToDeckShopItem(3, 1, 3, CardColor.GREEN, "Touch Grass".Color(CardColor.GREEN.ToColor())));
            shopItems.Add(15, new AddCardsToDeckShopItem(2, 1, 5, CardColor.NO_COLOR, "Garbage Collector".Color(CardColor.NO_COLOR.ToColor())));
            shopItems.Add(16, new ArtifactShopItem(bulkUp, 10, 1));
            shopItems.Add(17, new ArtifactShopItem(blue, 5, 1));

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

        public List<(int, IShopItem)> GetRandomItems(int amount)
        {
            var items = new List<(int, IShopItem)>();
            var uniqueIds = new HashSet<int>();

            while (items.Count < amount && uniqueIds.Count < ItemBag.Count)
            {
                var (id, item) = GetRandomItem();
                if (uniqueIds.Add(item.GetHashCode()))
                {
                    items.Add((id, item));
                }
            }

            return items;
        }

        public (int, IShopItem) GetRandomItem()
        {
            var id = ItemBag[Random.Range(0, ItemBag.Count)];
            return (id, shopItems[id]);
        }

        public void ClearBag(int id)
        {
            ItemBag.RemoveAll(x => x == id);
        }
    }
}