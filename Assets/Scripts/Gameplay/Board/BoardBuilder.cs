using System;
using System.Collections.Generic;
using Gameplay.Enum;
using Gameplay.View;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Board
{
    public class BoardBuilder : MonoBehaviour
    {
        public BoardElementsFactory elementsFactory;
        public GridLayoutGroup boardParent;
        public PlayerPiecesHelper player1PiecesHelper;
        public PlayerPiecesHelper player2PiecesHelper;
        
        public List<FieldView> fields = new ();

        private BoardSettings _boardSettings;
        private int _elementWidth;
        private int _elementHeight;
        
        public void PrepareBoard(BoardSettings boardSettings)
        {
            _boardSettings = boardSettings;
            _elementWidth = _boardSettings.boardWidth / _boardSettings.rows;
            _elementHeight = _boardSettings.boardHeight / _boardSettings.columns;
            
            PrepareFields();
            PreparePieces(player1PiecesHelper, _boardSettings.player1Pieces);
            PreparePieces(player2PiecesHelper, _boardSettings.player2Pieces);
        }

        private void PreparePieces(PlayerPiecesHelper playerPieces, List<PieceId> pieceIds)
        {
            playerPieces.piecesParent.cellSize = new Vector2(_elementWidth, _elementHeight);
            
            foreach (PieceId pieceId in pieceIds)
            {
                PieceView piece = elementsFactory.GetPieceFromPool();
                piece.transform.SetParent(playerPieces.piecesParent.transform,false);
                piece.Init(pieceId, playerPieces.playerId, playerPieces.piecesParent.cellSize);
                
                playerPieces.pieces.Add(piece);
            }
        }

        private void PrepareFields()
        {
            boardParent.cellSize = new Vector2(_elementWidth, _elementHeight);
            
            for (int i = 0; i < _boardSettings.rows; i++)
            {
                for (int j = 0; j < _boardSettings.columns; j++)
                {
                    FieldView field = elementsFactory.GetFieldFromPool();
                    field.transform.SetParent(boardParent.transform, false);
                    field.Init(i, j, i%2 == 0 ? j%2 == 0 : j%2 != 0);
                    
                    fields.Add(field);
                }
            }
        }

        private void OnDestroy()
        {
            foreach (FieldView field in fields)
            {
                elementsFactory.ReturnFieldToPool(field);
            }

            foreach (PieceView pieceView in player1PiecesHelper.pieces)
            {
                elementsFactory.ReturnPieceToPool(pieceView);
            }
            
            foreach (PieceView pieceView in player2PiecesHelper.pieces)
            {
                elementsFactory.ReturnPieceToPool(pieceView);
            }
        }
    }
}