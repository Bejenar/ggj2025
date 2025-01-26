using System;
using System.Linq;
using _Project.Source.Artifacts;
using Engine.Math;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Source.Util
{
    public class CheatPanel : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("Selected cards: " + G.battleScene.selected.Select(ic => ic.card.color.ToString()).Aggregate((a, b) => a + "," + b));
            }
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                G.battleScene.SendIt();
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                G.battleScene.EnterShop();
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                G.battleScene.MoveOutGameplayPanels(true);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                G.battleScene.MoveOutGameplayPanels(false);
            }
            
            if (Input.GetKeyDown(KeyCode.G))
            {
                G.state.gold += 100;
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                G.battleScene.ShowDeck();
            }
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                var depression = new ColorScoreModifier(3, CardColor.NO_COLOR, "<color=#ded4d3>Depression</color>");
                var bulkUp = new AdditionalScore(2, "<color=#00B6F7>Bubblemancer</color>");
                var burningPassion = new ColorScoreModifier(10, CardColor.RED, "Burning Passion".Color("#EA4345"));
                var goldOnMonochrome = new AddGoldOnCombo(5, new MonochromeCombo(), "Greed".Color("#ded4d3"));
                var multPerDiscard = new MultPerDiscard(0.5f, "Tactician".Color("#EA4345"));
                
                G.state.artifacts.Add(depression);
                G.state.artifacts.Add(bulkUp);
                G.state.artifacts.Add(burningPassion);
                G.state.artifacts.Add(goldOnMonochrome);
                G.state.artifacts.Add(multPerDiscard);
            }
            
            if (Input.GetKeyDown(KeyCode.P))
            {
                G.particles.AttackAnimation();
            }
            
            if (Input.GetKeyDown(KeyCode.O))
            {
                G.particles.PlayBubbles();
            }
        }
    }
}