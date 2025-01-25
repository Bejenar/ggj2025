using System;
using System.Linq;
using _Project.Source.Artifacts;
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
                var depression = new ColorScoreModifier(100, CardColor.NO_COLOR, "Depression");
                
                G.state.artifacts.Add(depression);
                G.state.artifacts.Add(depression);
                G.state.artifacts.Add(depression);
                G.state.artifacts.Add(depression);
                G.state.artifacts.Add(depression);
                G.state.artifacts.Add(depression);
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