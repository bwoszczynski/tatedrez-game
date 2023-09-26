using System;
using System.Collections.Generic;
using Const;
using DG.Tweening;
using Facade;
using Facade.Sound;
using Gameplay.Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Hud
{
    public class GameplayHud : MonoBehaviour
    {
        private static string wonInfo = "Player {0} Wins!";
        private static Dictionary<GameplayState, string> infoDictionary = new()
        {
            { GameplayState.Player1Move, "PLAYER 1- PLACE YOUR PIECE"},
            { GameplayState.Player2Move, "PLAYER 2- PLACE YOUR PIECE"}
        };
        
        private CustomActionManager _actionManager;
        private SceneTransitionManager _sceneTransitionManager;
        private SoundManager _soundManager;

        public TextMeshProUGUI infoLabel;
        public TextMeshProUGUI wonLabel;
        public RectTransform wonInfoContainer;
        public Button mainMenuButton;
        
        public void Init()
        {
            _sceneTransitionManager = ApplicationManager.Instance.SceneTransitionManager;
            _soundManager = ApplicationManager.Instance.SoundManager;
            
            _actionManager = ApplicationManager.Instance.CustomActionManager;
            _actionManager.OnGameplayStateChanged += OnGameplayStateChangedHandler;
            _actionManager.OnGameEnded += OnGameEndedHandler;
            
            mainMenuButton.onClick.AddListener(OnMainMenuButtonHandler);
        }

        private void OnMainMenuButtonHandler()
        {
            _soundManager.PlaySound(SoundId.ButtonClick);
            _sceneTransitionManager.LoadScene(SceneId.MAIN_MENU);
        }

        private void OnGameEndedHandler(PlayerId player)
        {
            wonInfoContainer.gameObject.SetActive(true);
            wonInfoContainer.localScale = Vector3.zero;
            wonInfoContainer.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            _soundManager.PlaySound(SoundId.PopupIn);

            wonLabel.text = string.Format(wonInfo, (int)player);
        }

        private void OnGameplayStateChangedHandler(GameplayState state)
        {
            if (infoDictionary.TryGetValue(state, out string info))
            {
                infoLabel.text = info;
            }
        }

        private void OnDestroy()
        {
            if (_actionManager != null)
            {
                _actionManager.OnGameplayStateChanged -= OnGameplayStateChangedHandler;
                _actionManager.OnGameEnded -= OnGameEndedHandler;
            }
            
            mainMenuButton.onClick.RemoveListener(OnMainMenuButtonHandler);
            
            mainMenuButton = null;
            _actionManager = null;
            _sceneTransitionManager = null;
            _soundManager = null;
        }
    }
}