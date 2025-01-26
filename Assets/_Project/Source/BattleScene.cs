using System.Collections.Generic;
using System.Linq;
using _Project.Data;
using _Project.Source.Artifacts;
using _Project.Source.Challenges;
using _Project.Source.Util;
using _Project.Source.Village.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityUtils;

namespace _Project.Source
{
    public class BattleScene : MonoBehaviour
    {
        public Canvas canvas;
        public Transform cardsParent;
        public Transform cardSpawnPosition;
        public PlayerHand playerHand;
        public BagView deckBag;
        public GameObject disableInputPanel;
        public CardZone sendZone;
        public Transform scoreTextSpawnPosition;
        public Transform artifactTextSpawnPosition;

        public Transform challengesContainer;
        public Transform victoryPopup;
        public CardZone moveOutZone;
        
        public DeckView deckView;

        [Header("Texts")]
        public TMP_Text handSizeText;

        public TMP_Text handCountText;
        public TMP_Text discardCountText;
        public TMP_Text goldText;
        public TMP_Text scoreText;
        public TMP_Text comboText;


        [Header("Prefabs")]
        public InteractiveCard cardPrefab;

        public PopupText popupTextPrefab;
        public ChallengeView challengeViewPrefab;

        [Header("Shop")]
        public Transform[] panelsToMoveToTopWhileShopping;
        public Transform[] panelsToMoveToRightWhileShopping;
        public Transform[] panelsToMoveToBottomWhileShopping;
        public Button nextLevelButton;
        private Dictionary<Transform, Vector3> _originalPanelPositions = new Dictionary<Transform, Vector3>();


        // Battle state
        public Deck _deck;
        private int hands;
        private int discards;
        private int gold => G.state.gold;
        private float score;
        private ICombo _currentCombo;
        private List<ChallengeView> _challenges;

        //State properties
        public float Score => score;

        public readonly List<InteractiveCard> selected = new List<InteractiveCard>();

        private async void Start()
        {
            G.audio.Play<GameplayMusic>();
            G.fader.FadeOut();
            G.battleScene = this;

            SetupShop();
            SetupBattleState();

            await DealCards();
        }

        private void OnDestroy()
        {
        }

        private void SetupBattleState()
        {
            _deck = G.state.deck.DeepCopy();
            _deck.Shuffle();
            hands = G.state.maxPlays;
            discards = G.state.maxDiscards;
            _currentCombo = new EmptyCombo();
            selected.Clear();


            SetupChallenges();

            // G.state.artifacts.Add(new ColorScoreModifier(10, CardColor.NO_COLOR, "White Supremacy"));

            UpdateHandSize();
            UpdateHandsView();
            UpdateDiscardsView();
            UpdateGoldView(true);
            UpdateScoreView(false, true);
            UpdateComboView();
        }

        private void SetupShop()
        {
            _originalPanelPositions.Clear();
            foreach (var panel in panelsToMoveToTopWhileShopping)
            {
                _originalPanelPositions.Add(panel, panel.localPosition);
            }

            foreach (var panel in panelsToMoveToRightWhileShopping)
            {
                _originalPanelPositions.Add(panel, panel.localPosition);
            }

            foreach (var panel in panelsToMoveToBottomWhileShopping)
            {
                _originalPanelPositions.Add(panel, panel.localPosition);
            }

            nextLevelButton.onClick.AddListener(NextLevel);
        }

        private void SetupChallenges()
        {
            _challenges = new List<ChallengeView>();

            var challenges = G.levelManager.GetChallengesFromCurrentLevel();

            foreach (var ch in challenges)
            {
                var tasks = G.levelManager.GetTasksFromChallenge(ch);
                var cv = Instantiate(challengeViewPrefab, challengesContainer);
                cv.Init(tasks);
                _challenges.Add(cv);
            }
        }

        public void ShowDeck()
        {
            deckView.gameObject.SetActive(true);
            deckView.InitDeck();
        }

        public void HideDeck()
        {
            deckView.gameObject.SetActive(false);
        }

        private async UniTask DealCards()
        {
            Debug.Log("Dealing cards");
            for (int i = 0; i < G.state.maxHandSize; i++)
            {
                await DrawCard();
            }

            DisableInput(false);
        }

        public async void Discard()
        {
            if (selected.Count == 0) return;

            DisableInput(true);
            discards--;
            UpdateDiscardsView();

            while (selected.Count > 0)
            {
                var interactiveCard = selected.First();
                G.audio.Play<ButtonHighSFX>();
                await moveOutZone.TryClaimAsync(interactiveCard);
                await UniTask.WaitForSeconds(0.1f);
                selected.Remove(interactiveCard);
            }

            await UniTask.WaitForSeconds(0.5f);
            UpdateComboView();
            selected.Clear();
            moveOutZone.objects.ForEach(o => Destroy(o.gameObject));
            moveOutZone.objects.Clear();

            await DrawUntilFullHand();

            DisableInput(false);
        }

        private async UniTask DrawUntilFullHand()
        {
            while (playerHand.objects.Count < G.state.maxHandSize && _deck.Count > 0)
            {
                await DrawCard();
            }
        }

        private async UniTask DrawCard()
        {
            var card = _deck.DrawCard();
            if (card == null)
            {
                Debug.LogWarning("No more cards in deck to draw");
                return;
            }

            var interactiveCard = Instantiate(cardPrefab, cardSpawnPosition.transform.position,
                Quaternion.identity, cardsParent);
            interactiveCard.Init(card);
            deckBag.transform.DOKill(true);
            deckBag.transform.DOPunchRotation(new Vector3(0, 0, 10), 0.1f);
            G.audio.Play<ButtonHighSFX>();
            await playerHand.TryClaimAsync(interactiveCard);
        }

        public void Select(InteractiveCard card)
        {
            G.battleScene.selected.Add(card);
            EvaluateCombo();
        }

        public void Deselect(InteractiveCard card)
        {
            G.battleScene.selected.Remove(card);
            EvaluateCombo();
        }

        private async void EvaluateCombo()
        {
            _currentCombo = await G.scoreManager.EvaluateComboType(selected.Select(ic => ic.card).ToList());
            UpdateComboView();
        }


        public async void SendIt()
        {
            if (selected.Count == 0) return;

            DisableInput(true);

            hands--;
            UpdateHandsView();

            foreach (var interactiveCard in selected)
            {
                var card = interactiveCard.card;
                var addScore = card.baseScore;

                G.audio.Play<ButtonHighSFX>();
                sendZone.TryClaim(interactiveCard);
                UpdateHandSize();
                await UniTask.WaitForSeconds(0.1f);

                foreach (var cardValueModifier in G.state.artifacts.OfType<ICardValueModifier>())
                {
                    addScore = await cardValueModifier.ModifyCardValue(card, addScore);
                }

                score += addScore;
                ShowText(scoreTextSpawnPosition.position, $"+{addScore}");
                UpdateScoreView(true);

                await UniTask.WaitForSeconds(0.5f);
            }

            sendZone.objects.ForEach(o => Destroy(o.gameObject));
            sendZone.objects.Clear();

            ShowText(scoreTextSpawnPosition.position, $"Combo  x{_currentCombo.Multiplier:F1}");
            score *= _currentCombo.Multiplier;
            UpdateScoreView(true);

            await G.particles.AttackAnimation();

            // Evaluate challenge cleared
            foreach (var challengeView in _challenges)
            {
                challengeView.Evaluate();
                challengeView.UpdateView();

                if (challengeView.completed)
                {
                    await UniTask.WaitForSeconds(0.5f);
                    challengeView.Dissolve();
                }
            }
            
            score = 0;
            UpdateScoreView();
            UpdateComboView();

            _challenges.RemoveAll(cv => cv.completed);

            if (_challenges.Count == 0)
            {
                await UniTask.WaitForSeconds(1f);
                Debug.Log("All challenges completed");
                await Victory();
                return;
            }

            selected.Clear();
            await DrawUntilFullHand();

            if (hands == 0)
            {
                Debug.Log("No more hands, you lost");
                G.main.InitRunState();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            DisableInput(false);
        }

        private async UniTask Victory()
        {
            G.state.level++;
            G.audio.Play<WinSFX>();
            await victoryPopup.DOScale(1, 0.5f).SetEase(Ease.OutBack);
            AddReward();
            await UniTask.WaitForSeconds(1.5f);
            await victoryPopup.DOScale(0, 0.5f).SetEase(Ease.InBack);
            EnterShop();
        }

        public void AddReward()
        {
            var reward = G.levelManager.GetLevelReward();
            var remainingHands = hands;
            G.state.gold += reward + remainingHands;
            UpdateGoldView();
        }

        public void DisableInput(bool toggle)
        {
            disableInputPanel.SetActive(toggle);
        }

        private PopupText _popupText;
        private PopupText _popupArtifactText;

        public void ShowText(Vector3 position, string text, bool isArtifactText = false)
        {
            var popup = isArtifactText ? _popupArtifactText : _popupText;
            text = isArtifactText ? $"<size=70%>{text}</size>" : text;

            if (popup != null)
            {
                popup.Kill();
                Destroy(popup.gameObject);
            }

            popup = Instantiate(popupTextPrefab, canvas.transform);
            popup.transform.position = position;
            popup.SetText(text);
            Debug.Log(text);

            if (isArtifactText)
                _popupArtifactText = popup;
            else
                _popupText = popup;
        }

        public void UpdateHandSize()
        {
            handSizeText.text = playerHand.objects.Count + "/" + G.state.maxHandSize;
        }

        public void UpdateHandsView()
        {
            handCountText.text = hands.ToString();
        }

        public void UpdateDiscardsView()
        {
            discardCountText.text = discards.ToString();
        }

        public void UpdateGoldView(bool noAnimation = false)
        {
            int.TryParse(goldText.text.Substring(1), out var currentGold);

            if (Mathf.Approximately(currentGold, gold)) return;
            
            if (noAnimation)
            {
                goldText.text = $"${gold}";
                Debug.Log("dfdsssdd");
                return;
            }
            
            G.audio.Play<CoinsSFX>();
            var t = DOTween.To(() => currentGold, x =>
            {
                currentGold = x;
                goldText.text = $"${currentGold}";
            }, gold, 0.75f);
        }

        public void UpdateScoreView(bool punch = false, bool noAnimation = false)
        {
            int.TryParse(scoreText.text, out var currentScore);

            if (Mathf.Approximately(currentScore, score)) return;
            if (noAnimation)
            {
                scoreText.text = score.ToString("F0");
                return;
            }

            var t = DOTween.To(() => currentScore, x =>
            {
                currentScore = x;
                scoreText.text = currentScore.ToString();
            }, (int)score, 0.75f);

            if (punch)
            {
                scoreText.transform.DOKill(true);
                scoreText.transform.DOPunchScale(Vector3.one * 0.5f, 0.1f);
            }
        }

        public void UpdateComboView()
        {
            if (!comboText.text.Equals(_currentCombo.Name))
            {
                comboText.text = _currentCombo.Name;
                comboText.transform.DOShakeRotation(0.2f, 10);
            }
        }

        public async UniTask MoveOutRemainingCards()
        {
            while (playerHand.objects.Count > 0)
            {
                var interactiveCard = playerHand.objects.First();
                await moveOutZone.TryClaimAsync(interactiveCard);
            }
            
            await UniTask.WaitForSeconds(1f);

            playerHand.objects.Clear();
            moveOutZone.objects.ForEach(o => Destroy(o.gameObject));
            moveOutZone.objects.Clear();
        }
        
        

        public async void EnterShop()
        {
            await MoveOutRemainingCards();
            MoveOutGameplayPanels();
            await UniTask.WaitForSeconds(0.5f);

            await G.shopManager.EnterShop();
            
            DisableInput(false);
        }

        public async void NextLevel()
        {
            await UniTask.WaitForSeconds(0.2f);
            await G.shopManager.CloseShop();

            MoveOutGameplayPanels(false);
            await UniTask.WaitForSeconds(1f);
            SetupBattleState();
            await DealCards();
        }

        public void MoveOutGameplayPanels(bool moveOut = true)
        {
            foreach (var panel in panelsToMoveToTopWhileShopping)
            {
                panel.DOLocalMove(GetOffScreenPosition(panel, Vector2.up, moveOut), 1.0f).SetEase(Ease.InOutBack);
            }

            foreach (var panel in panelsToMoveToRightWhileShopping)
            {
                panel.DOLocalMove(GetOffScreenPosition(panel, Vector2.right, moveOut), 1.0f).SetEase(Ease.InOutBack);
            }

            foreach (var panel in panelsToMoveToBottomWhileShopping)
            {
                panel.DOLocalMove(GetOffScreenPosition(panel, Vector2.down, moveOut), 1.0f).SetEase(Ease.InOutBack);
            }
        }

        private Vector3 GetOffScreenPosition(Transform panel, Vector2 direction, bool outside = true)
        {
            var canvasRect = canvas.GetComponent<RectTransform>();
            var canvasSize = canvasRect.sizeDelta;
            if (!outside)
            {
                return _originalPanelPositions[panel];
            }

            var dirX = direction.x == 0 ? panel.transform.localPosition.x : 0;
            var dirY = direction.y == 0 ? panel.transform.localPosition.y : 0;

            return new Vector3((direction.x * canvasSize.x) + dirX, (direction.y * canvasSize.y) + dirY, 0);
        }
    }
}