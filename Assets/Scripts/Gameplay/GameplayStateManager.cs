using System.Collections.Generic;
using Facade;
using Gameplay.Enum;

namespace Gameplay
{
    public class GameplayStateManager
    {
        private static Dictionary<GameplayState, PlayerId> _playerByStateDictionary = new()
        {
            { GameplayState.Player1Move, PlayerId.Player1 },
            { GameplayState.Player2Move, PlayerId.Player2 }
        };
        
        private CustomActionManager _actionManager;
        private GameplayState _state;
        private GameplayMode _mode;
        
        public GameplayState CurrentState => _state;
        public GameplayMode CurrentMode => _mode;
        
        public void Init()
        {
            _actionManager = ApplicationManager.Instance.CustomActionManager;
        }

        public void SetMode(GameplayMode mode)
        {
            _mode = mode;
        }
        
        public void SetState(GameplayState state)
        {
            _state = state;
            _actionManager.OnGameplayStateChanged?.Invoke(_state);
        }

        public PlayerId GetCurrentPlayer()
        {
            return _playerByStateDictionary[_state];
        }

        public PlayerId GetPlayerForState(GameplayState state)
        {
            return _playerByStateDictionary[state];
        }

        public GameplayState GetNextState()
        {
            GameplayState nextState = _state == GameplayState.Player1Move
                ? GameplayState.Player2Move
                : GameplayState.Player1Move;

            return nextState;
        }
        
        public void Clear()
        {
            _actionManager = null;
        }
    }
}