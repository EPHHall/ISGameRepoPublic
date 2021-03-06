using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS.UI
{
    public class PauseScreen : MonoBehaviour
    {
        public static GameObject currentScreen;
        public GameObject mainScreen;

        public GameObject[] screens;

        public void Toggle()
        {
            if (currentScreen == null)
            {
                currentScreen = mainScreen;
                mainScreen.SetActive(true);

                mainScreen.transform.localPosition = Vector2.zero;
            }
            else
            {
                foreach (GameObject screen in screens)
                {
                    screen.SetActive(false);
                }

                currentScreen = null;
           }
        }

        public void ChangeScreen(GameObject otherScreen)
        {
            currentScreen.SetActive(false);
            otherScreen.SetActive(true);
            currentScreen = otherScreen;

            currentScreen.transform.localPosition = Vector2.zero;
        }
    }
}
