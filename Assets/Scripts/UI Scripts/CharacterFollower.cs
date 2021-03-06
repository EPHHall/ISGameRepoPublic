using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SS.Character;

namespace SS.UI
{
    public class CharacterFollower : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Space(5)]
        [Header("Don't Touch")]
        public StatsCard statsCard;
        public Transform following;
        private CharacterStats stats;
        public bool showing;

        private void Start()
        {
            statsCard = FindObjectOfType<StatsCard>();

            following = GetComponent<Follow>().toFollow;

            stats = following.GetComponent<CharacterStats>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (GameController.TurnManager.currentTurnTaker.tag == "Player")
                {
                    if (!showing)
                    {
                        if (stats.GetComponent<AI.Agent>() != null)
                        {
                            AI.Agent agent = stats.GetComponent<AI.Agent>();

                            if (agent.spells.Count > 0)
                            {
                                stats.ShowRangeOfAbilities(agent.spells[0]);
                            }
                            else
                            {
                                stats.ShowRangeOfAbilities(null);
                            }

                            showing = true;
                        }
                        else
                        {
                            stats.ShowRangeOfAbilities(null);
                        }
                    }
                    else
                    {
                        if (GameController.TurnManager.currentTurnTaker.tag == "Player")
                        {
                            GameController.TurnManager.currentTurnTaker.GetComponent<PlayerMovement.SS_PlayerMoveRange>().SpawnRange();
                            showing = false;
                        }
                    }
                }
            }
            else
            {
                if (statsCard.gameObject.activeInHierarchy && !statsCard.firstActivation)
                {
                    if (statsCard.statsToDisplay != following.GetComponent<CharacterStats>())
                    {
                        statsCard.statsToDisplay = following.GetComponent<CharacterStats>();
                    }
                    else
                    {
                        statsCard.gameObject.SetActive(false);
                    }
                }
                else
                {
                    statsCard.gameObject.SetActive(true);
                    statsCard.SetPosition(following.position.x < Camera.main.transform.position.x);
                    statsCard.SetStatsToDisplay(stats);
                    statsCard.canDeactivate = false;
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}
