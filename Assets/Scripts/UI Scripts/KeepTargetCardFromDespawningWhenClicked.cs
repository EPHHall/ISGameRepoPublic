﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SS.UI
{
    public class KeepTargetCardFromDespawningWhenClicked : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public StatsCard card;

        public bool pointerIsOver;

        public void OnPointerEnter(PointerEventData eventData)
        {
            card.pointerIsOver = true;
            pointerIsOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            card.pointerIsOver = false;
            pointerIsOver = false;
        }

        private void OnDisable()
        {
            card.pointerIsOver = false;
        }

        private void OnDestroy()
        {
            card.pointerIsOver = false;
        }
    }
}