using System;
using Gameplay.Hud;
using Interface;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class GameplaySceneController : MonoBehaviour, ISceneController
    {
        public GameplayManager gameplayManager;
        public GameplayHud gameplayHud;
        public void Init()
        {
            gameplayHud.Init();
            gameplayManager.Init();
        }

        public void OnDestroy()
        {
            gameplayHud = null;
            gameplayManager = null;
        }
    }
}