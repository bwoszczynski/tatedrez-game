using System;
using System.Collections.Generic;
using DG.Tweening;
using Facade;
using Gameplay.Enum;
using UnityEngine;

namespace Gameplay.View
{
    public class PiecesViewManager
    {
        private Dictionary<PlayerId, List<PieceView>> _pieces = new();
        private CustomActionManager _actionManager;
        
        public void Init()
        {
            _actionManager = ApplicationManager.Instance.CustomActionManager;
            _actionManager.OnGameplayStateChanged += OnGameplayStateChangedHandler;
        }

        private void OnGameplayStateChangedHandler(GameplayState state)
        {
            UpdateViews(state == GameplayState.Player1Move ? PlayerId.Player1 : PlayerId.Player2);
        }

        public void AddPiecesToPlayer(PlayerId ownerId, List<PieceView> pieces)
        {
            _pieces.Add(ownerId, pieces);
        }
        
        public void UpdateViews(PlayerId currentPlayer)
        {
            foreach (KeyValuePair<PlayerId, List<PieceView>> keyValue in _pieces)
            {
                bool lockView = keyValue.Key != currentPlayer;

                foreach (PieceView piece in keyValue.Value)
                {
                    piece.UpdateView(lockView);
                }
            }
        }

        public List<Vector2Int> GetPiecesCoordinates(PlayerId ownerId)
        {
            List<PieceView> pieces = GetPiecesByPlayer(ownerId);
            List<Vector2Int> coords = new();
            
            foreach (PieceView piece in pieces)
            {
                coords.Add(new (piece.row, piece.column));
            }

            return coords;
        }

        public void ShowWonAnim(PlayerId ownerId, Action callback)
        {
            List<PieceView> pieces = GetPiecesByPlayer(ownerId);

            int count = pieces.Count;
            
            for (int i = 0; i < count; i++)
            {
                pieces[i].rectTransform.DOScale(1.3f, 0.2f).SetDelay(i * 0.08f).SetEase(Ease.InBack);
            }


            for (int i = 0; i < count; i++)
            {
                if (i < count - 1)
                {
                    pieces[i].rectTransform.DOScale(1, 0.2f).SetDelay(0.2f + i * 0.08f).SetEase(Ease.OutBack);
                }
                else
                {
                    pieces[i].rectTransform.DOScale(1, 0.2f).SetDelay(0.2f + i * 0.08f).SetEase(Ease.OutBack).OnComplete(() =>callback?.Invoke());
                }
                
            }
        }
        
        public List<PieceView> GetPiecesByPlayer(PlayerId ownerId)
        {
            return _pieces[ownerId];
        }
        
        public void Clear()
        {
            _actionManager.OnGameplayStateChanged -= OnGameplayStateChangedHandler;
            _actionManager = null;
        }
    }
}