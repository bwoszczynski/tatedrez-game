using System;
using System.Collections.Generic;
using Facade;
using Facade.Sound;
using Gameplay.Board;
using Gameplay.Enum;
using Gameplay.Logic;
using Gameplay.View;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        public Transform gameplayContainer;
        public BoardSettings boardSettings;
        public BoardBuilder boardBuilder;

        private CustomActionManager _customActionManager;
        private SoundManager _soundManager;
        private LogicManager _logicManager;
        private GameplayStateManager _stateManager = new ();
        private PiecesViewManager _piecesViewManager = new ();
        
        public void Init()
        {
            _customActionManager = ApplicationManager.Instance.CustomActionManager;
            _soundManager = ApplicationManager.Instance.SoundManager;
            
            _stateManager.Init();
            
            _logicManager = new LogicManager(boardSettings);
            
            boardBuilder.PrepareBoard(boardSettings);
            
            foreach (FieldView field in boardBuilder.fields)
            {
                field.SetDroppedCallback(OnPieceDroppedCallback);
            }
            
            _piecesViewManager.Init();
            _piecesViewManager.AddPiecesToPlayer(PlayerId.Player1, boardBuilder.player1PiecesHelper.pieces);
            _piecesViewManager.AddPiecesToPlayer(PlayerId.Player2, boardBuilder.player2PiecesHelper.pieces);

            _stateManager.SetMode(GameplayMode.Placement);
            
            DrawStartPlayer();
        }

        private void DrawStartPlayer()
        {
            int rand = Random.Range(0, 2);
            
            _stateManager.SetState(rand == 0 ? GameplayState.Player1Move : GameplayState.Player2Move);
        }
        
        private void OnPieceDroppedCallback(PieceView piece, FieldView field)
        {
            bool validMove = _logicManager.ValidateMove(piece.row, piece.column, piece.Id, field.row, field.column, _stateManager.CurrentMode);

            if (validMove)
            {
                piece.SetPosition(field.row, field.column, field.rectTransform.localPosition, gameplayContainer);

                List<Vector2Int> coords = _piecesViewManager.GetPiecesCoordinates(_stateManager.GetCurrentPlayer());
                bool gameEnded = _logicManager.CheckEndGame(coords);

                if (gameEnded)
                {
                    _piecesViewManager.UpdateViews(PlayerId.None);
                    _piecesViewManager.ShowWonAnim(_stateManager.GetCurrentPlayer(), ShowEndGamePopup);
                    _soundManager.PlaySound(SoundId.LevelWon);
                }
                else
                {
                    SwitchModeToDynamic();

                    GameplayState nextState = _stateManager.GetNextState();

                    PlayerId nextPlayer = _stateManager.GetPlayerForState(nextState);
                    List<PieceView> pieces = _piecesViewManager.GetPiecesByPlayer(nextPlayer);

                    foreach (PieceView nextPlayerPiece in pieces)
                    {
                        bool areMovesAvailable = _logicManager.CheckAvailableMoves(nextPlayer, nextPlayerPiece.row, nextPlayerPiece.column, nextPlayerPiece.Id, _stateManager.CurrentMode);

                        if (areMovesAvailable)
                        {
                            _stateManager.SetState(nextState);
                            break;
                        }
                    }
                }
                
            }
            else
            {
                piece.ResetPosition();
            }
        }

        private void ShowEndGamePopup()
        {
            _customActionManager.OnGameEnded(_stateManager.GetCurrentPlayer());
        }
        
        private void SwitchModeToDynamic()
        {
            if (_stateManager.CurrentMode == GameplayMode.Placement)
            {
                bool piecesOnBoard = ArePlayerPiecesOnBoard(PlayerId.Player1);
                if (piecesOnBoard)
                {
                    piecesOnBoard = ArePlayerPiecesOnBoard(PlayerId.Player2);

                    if (piecesOnBoard)
                    {
                        _stateManager.SetMode(GameplayMode.Dynamic);
                    }
                }
            }
        }

        private bool ArePlayerPiecesOnBoard(PlayerId ownerId)
        {
            List<Vector2Int> coords = _piecesViewManager.GetPiecesCoordinates(ownerId);
            foreach (Vector2Int pieceCoords in coords)
            {
                if (pieceCoords.x == -1 || pieceCoords.y == -1)
                {
                    return false;
                }
            }

            return true;
        }
        
        
        private void OnDestroy()
        {
            _soundManager = null;
            _customActionManager = null;
            _logicManager = null;
            
            _stateManager.Clear();
            _stateManager = null;
            
            _piecesViewManager.Clear();
            _piecesViewManager = null;
            
            boardBuilder = null;
        }
    }
}