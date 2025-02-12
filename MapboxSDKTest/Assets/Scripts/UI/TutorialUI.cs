using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class TutorialUI : MonoBehaviour
    {
        public List<GameObject> pages;
        public bool showTutorial;
        private int _currentPage;

        private bool _tapped;
        private bool _showTutorialOld;

        public void Update()
        {
            // Stop if the tutorial is not displayed
            if (!showTutorial) return;
            
            //Is this the frame where we started to show the tutorial?
            if(showTutorial != _showTutorialOld)
                pages[_currentPage].SetActive(true);
            
            _showTutorialOld = showTutorial;

            // Is the user not touching the screen?
            if (Input.touchCount == 0)
            {
                // Reset tapped
                _tapped = false;
                return;
            }
            
            // If the user has already tapped, don't run the code again.
            if (_tapped) return;
            
            // Only the first time should this run, and so we must update tapped so that next frame this code is not run
            _tapped = true;
            
            //Stop showing this page
            pages[_currentPage].SetActive(false);

            // Is this the last page?
            if (_currentPage == pages.Count - 1)
            {
                _currentPage = 0;
                showTutorial = false;
                return;
            }
            
            // Show the next frame
            pages[++_currentPage].SetActive(true);
        }
    }
}