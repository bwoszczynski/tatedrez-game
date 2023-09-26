using System;
using Const;
using Facade;
using Facade.Sound;
using Interface;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuSceneController : MonoBehaviour, ISceneController
    {
        public Button startGameButton;

        private SceneTransitionManager _sceneTransitionManager;
        private SoundManager _soundManager;
        
        public void Init()
        {
            _sceneTransitionManager = ApplicationManager.Instance.SceneTransitionManager;
            _soundManager = ApplicationManager.Instance.SoundManager;
            
            startGameButton.onClick.AddListener(OnStartGameButtonHandler);
        }

        public void OnStartGameButtonHandler()
        {
            _soundManager.PlaySound(SoundId.ButtonClick);
            _sceneTransitionManager.LoadScene(SceneId.GAMEPLAY);
        }

        private void OnDestroy()
        {
            startGameButton.onClick.RemoveListener(OnStartGameButtonHandler);
            _sceneTransitionManager = null;
            _soundManager = null;
        }
    }
}