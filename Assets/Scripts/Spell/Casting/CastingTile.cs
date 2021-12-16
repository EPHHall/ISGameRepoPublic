﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using SS.UI;

namespace SS.Spells
{
    public class CastingTile : Tile, IPointerEnterHandler, IPointerExitHandler
    {
        public Color defaultColor;
        public Color highlightColor;
        public Color selectedColor;

        public bool mouseIsOver;
        public bool underFollower;
        public static List<CastingTile> selectedTiles = new List<CastingTile>();
        public int maxSelections = 1;

        public List<Target> targets;

        UnityEvent selectedTilesChange;

        public StatsCard statsCard;

        public TileResources resources;

        // Start is called before the first frame update
        void Start()
        {
            selectedTilesChange = new UnityEvent();

            resources = GameObject.FindObjectOfType<TileResources>();

            SS.UI.TargetMenu targetMenu = GameObject.FindObjectOfType<SS.UI.TargetMenu>();
            if (targetMenu != null)
            {
                selectedTilesChange.AddListener(targetMenu.OnTargetTilesChange);
            }

            selectedTilesChange.AddListener(Target.ClearSelectedTargets);

            statsCard = resources.statsCard;
        }

        // Update is called once per frame
        void Update()
        {
            if (mouseIsOver)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (selectedTiles.Contains(this))
                    {
                        DeselectTile();
                    }
                    else
                    {
                        SelectTile();
                    }
                }

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    if (selectedTiles.Contains(this))
                    {}
                    else
                    {
                        SelectTile();
                    }

                    //SS.UI.TargetMenuRadial targetMenuRadial = FindObjectOfType<SS.UI.TargetMenuRadial>();
                    //targetMenuRadial.ActivateAndPlaceMenu();
                }
            }
        }

        public virtual void SelectTile()
        {
            while (selectedTiles.Count >= maxSelections)
            {
                DeselectTile(selectedTiles[0]);
            }

            selectedTiles.Add(this);
            GetComponent<SpriteRenderer>().color = selectedColor;

            selectedTilesChange.Invoke();

            if (!underFollower)
            {
                if (statsCard.gameObject.activeInHierarchy && !statsCard.firstActivation)
                {
                    statsCard.statsToDisplay = null;
                }
                else
                {
                    statsCard.gameObject.SetActive(true);
                    statsCard.SetPosition(transform.position.x < Camera.main.transform.position.x);
                    statsCard.targets = GetTargets();
                    statsCard.SetStatsToDisplay(null);
                    statsCard.canDeactivate = false;
                }
            }
        }

        public List<Target> GetTargets()
        {
            List<Target> targets = SS.Util.GetOnlyTargets.GetTargets(Physics2D.OverlapBoxAll(transform.position, new Vector2(.5f, .5f), 0));

            return targets;
        }
        public bool GetFollowers()
        {
            return SS.Util.GetFollowers.TargetsPresent(Physics2D.OverlapBoxAll(transform.position, new Vector2(.5f, .5f), 0));
        }

        public virtual void DeselectTile()
        {
            selectedTiles.Remove(this);
            GetComponent<SpriteRenderer>().color = defaultColor;

            selectedTilesChange.Invoke();
        }
        public void DeselectTile(CastingTile toRemove)
        {
            selectedTilesChange.Invoke();
            selectedTiles.Remove(toRemove);

            if(toRemove != null)
                toRemove.GetComponent<SpriteRenderer>().color = defaultColor;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.GetComponent<Target>() != null)
            {
                targets.Add(col.GetComponent<Target>());
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.GetComponent<Target>() != null)
            {
                targets.Remove(col.GetComponent<Target>());
            }
        }

        private void OnMouseOver()
        {
        }

        private void OnMouseExit()
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void PointerEntered()
        {
            if (!selectedTiles.Contains(this))
            {
                GetComponent<SpriteRenderer>().color = highlightColor;
            }

            underFollower = false;
            if (GetFollowers())
            {
                underFollower = true;
            }

            mouseIsOver = true;
        }

        public void PointerExited()
        {
            if (!selectedTiles.Contains(this))
            {
                GetComponent<SpriteRenderer>().color = defaultColor;
            }
            mouseIsOver = false;
        }
    }
}
