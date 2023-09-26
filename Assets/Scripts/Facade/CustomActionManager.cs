using System;
using Gameplay.Enum;
using Interface;
using UnityEngine;

namespace Facade
{
    public class CustomActionManager : MonoBehaviour
    {
        public Action<GameplayState> OnGameplayStateChanged;
        public Action<PlayerId> OnGameEnded;
    }
}