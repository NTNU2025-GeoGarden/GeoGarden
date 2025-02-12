using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [System.Serializable]
    public struct TutorialPage
    {
        public GameObject page;
        public bool barrier;
        public GameObject needsToBeActive;
    }
    
    public class TutorialUI : MonoBehaviour
    {
        public List<TutorialPage> pages;
        public bool showTutorial;
        public GameObject barrier;

        private int _currentPage;
        private bool _tapped;
        private bool _showTutorialOld;

        private float _timeout = 1f;

        public void Update()
        {
            // Stop if the tutorial is not displayed
            if (!showTutorial) return;
            
            if (pages[_currentPage].needsToBeActive != null){
                if (pages[_currentPage].needsToBeActive.activeSelf)
                {
                    pages[_currentPage].page.SetActive(false);
                    pages[++_currentPage].page.SetActive(true);
                    return;
                }
            }
            
            //Is this the frame where we started to show the tutorial?
            if(showTutorial != _showTutorialOld)
                pages[_currentPage].page.SetActive(true);
            
            _showTutorialOld = showTutorial;
            
            barrier.SetActive(pages[_currentPage].barrier);
            
            if (_timeout > 0)
            {
                _timeout -= Time.deltaTime;
                return;
            }

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
            
            // Is this the last page?
            if (_currentPage == pages.Count - 1)
            {
                showTutorial = false;
                _showTutorialOld = false;
                
                barrier.SetActive(false);
                pages[_currentPage].page.SetActive(false);
                
                _currentPage = 0;
                return;
            }

            if (!pages[_currentPage].barrier)
            {
                PointerEventData eventData = new(EventSystem.current)
                {
                    position = Input.mousePosition
                };
                
                List<RaycastResult> results = new();
                
                EventSystem.current.RaycastAll(eventData, results);
                
                if (pages[_currentPage].needsToBeActive == null)
                {
                    if(results.Any(r => r.gameObject.transform.CompareTag("Blocker")))
                        return;
                    
                    pages[_currentPage].page.SetActive(false);
                    pages[++_currentPage].page.SetActive(true);
                }
                
                return;
            }
            
            //Stop showing this page
            pages[_currentPage].page.SetActive(false);

            // Show the next frame
            pages[++_currentPage].page.SetActive(true);
        }
    }
}