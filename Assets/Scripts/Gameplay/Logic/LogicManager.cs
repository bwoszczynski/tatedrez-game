using System.Collections.Generic;
using Gameplay.Board;
using Gameplay.Enum;
using UnityEngine;

namespace Gameplay.Logic
{
    public class LogicManager
    {
        private BoardSettings _boardSettings;
        private static List<List<List<int>>> _winPatterns = new();
        private List<List<int>> _currentBoard = new();
        
        public LogicManager(BoardSettings settings)
        {
            _boardSettings = settings;

            PrepareBoard();
            FindWinningPatterns();
        }

        private void PrepareBoard()
        {
            for (int i = 0; i < _boardSettings.rows; i++)
            {
                _currentBoard.Add(new());
                for (int j = 0; j < _boardSettings.columns; j++)
                {
                    _currentBoard[i].Add(0);
                }
            }
        }
        
        private void FindWinningPatterns()
        {
            if (_winPatterns.Count > 0)
            {
                return;
            }
            
            List<List<int>> singleWinPattern = new();
            //columns
            for (int i = 0; i < _boardSettings.rows; i++)
            {
                singleWinPattern = new();

                for (int j = 0; j < _boardSettings.rows; j++)
                {
                    singleWinPattern.Add(new List<int>());
                    for (int k = 0; k < _boardSettings.columns; k++)
                    {
                        singleWinPattern[j].Add(i == j ? 1 : 0);
                    }
                }
                
                _winPatterns.Add(singleWinPattern);
            }
            
            //rows
            for (int i = 0; i < _boardSettings.columns; i++)
            {
                singleWinPattern = new();

                for (int j = 0; j < _boardSettings.rows; j++)
                {
                    singleWinPattern.Add(new List<int>());
                    for (int k = 0; k < _boardSettings.columns; k++)
                    {
                        singleWinPattern[j].Add(i == k ? 1 : 0);
                    }
                }
                
                _winPatterns.Add(singleWinPattern);
            }

            if (_boardSettings.rows == _boardSettings.columns)
            {
                //diagonal
                singleWinPattern = new();

                for (int j = 0; j < _boardSettings.rows; j++)
                {
                    singleWinPattern.Add(new List<int>());
                    for (int k = 0; k < _boardSettings.columns; k++)
                    {
                        singleWinPattern[j].Add(j == k ? 1 : 0);
                    }
                }

                _winPatterns.Add(singleWinPattern);

                //diagonal #2
                singleWinPattern = new();

                for (int j = 0; j < _boardSettings.rows; j++)
                {
                    singleWinPattern.Add(new List<int>());
                    for (int k = 0; k < _boardSettings.columns; k++)
                    {
                        singleWinPattern[j].Add(j + k == _boardSettings.rows - 1 ? 1 : 0);
                    }
                }

                _winPatterns.Add(singleWinPattern);
            }
        }
        
        public bool ValidateMove(int startRow, int startColumn, PieceId pieceID, int targetRow, int targetColumn, GameplayMode mode, bool checkOnly = false)
        {
            switch (mode)
            {
                case GameplayMode.Placement:
                {
                    bool validMove = ValidatePlacementMove(startRow, startColumn, targetRow, targetColumn);

                    if (!checkOnly)
                    {
                        if (validMove)
                        {
                            FillBoardWithPiece(startRow, startColumn, targetRow, targetColumn);
                        }
                    }

                    return validMove;
                }
                case GameplayMode.Dynamic:
                {
                    bool validMove = ValidateDynamicMove(startRow, startColumn, pieceID, targetRow, targetColumn);

                    if (!checkOnly)
                    {
                        if (validMove)
                        {
                            FillBoardWithPiece(startRow, startColumn, targetRow, targetColumn);
                        }
                    }

                    return validMove;
                }
            }
            return false;
        }

        private bool ValidatePlacementMove(int startRow, int startColumn, int targetRow, int targetColumn)
        {
            if (startRow == -1 && startColumn == -1 && _currentBoard[targetRow][targetColumn] == 0)
            {
                return true;
            }

            return false;
        }

        private bool ValidateDynamicMove(int startRow, int startColumn, PieceId pieceID, int targetRow, int targetColumn)
        {
            if (_currentBoard[targetRow][targetColumn] == 0)
            {
                switch (pieceID)
                {
                    case PieceId.Bishop:
                    {
                        bool validMove =  ValidateBishopMove(startRow, startColumn, targetRow, targetColumn);
                        return validMove;
                    }
                    case PieceId.Knight:
                    {
                        bool validMove =  ValidateKnightMove(startRow, startColumn, targetRow, targetColumn);
                        return validMove;
                    }
                    case PieceId.Rook:
                    {
                        bool validMove =  ValidateRookMove(startRow, startColumn, targetRow, targetColumn);
                        return validMove;
                    }
                }
                return true;
            }

            return false;
        }

        private bool ValidateBishopMove(int startRow, int startColumn, int targetRow, int targetColumn)
        {
            if (Mathf.Abs(startRow - targetRow) == Mathf.Abs(startColumn - targetColumn))
            {
                int lastRow = targetRow;
                int firstRow = startRow;
                int lastColumn = targetColumn;
                int firstColumn = startColumn;
                
                if (startRow > targetRow)
                {
                    lastRow = startRow;
                    firstRow = targetRow;
                }

                if (startColumn > targetColumn)
                {
                    lastColumn = startColumn;
                    firstColumn = targetColumn;
                }
                
                for (int i = firstRow + 1; i < lastRow; i++)
                {
                    for (int j = firstColumn + 1; j < lastColumn; j++)
                    {
                        if (_currentBoard[i][j] == 1)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ValidateKnightMove(int startRow, int startColumn, int targetRow, int targetColumn)
        {
            return (Mathf.Abs(startRow - targetRow) == 1 && Mathf.Abs(startColumn - targetColumn) == 2) 
                   || (Mathf.Abs(startRow - targetRow) == 2 && Mathf.Abs(startColumn - targetColumn) == 1);
        }

        private bool ValidateRookMove(int startRow, int startColumn, int targetRow, int targetColumn)
        {
            if (targetRow == startRow && targetColumn != startColumn)
            {
                int firstColumn = startColumn;
                int lastColumn = targetColumn;
                
                if (startColumn > targetColumn)
                {
                    firstColumn = targetColumn;
                    lastColumn = startColumn;
                }
                
                for (int i = firstColumn + 1; i < lastColumn; i++)
                {
                    if (_currentBoard[targetRow][i] == 1)
                    {
                        return false;
                    }
                }
                return true;
            }

            if (targetColumn == startColumn && targetRow != startRow)
            {
                int firstRow = startRow;
                int lastRow = targetRow;
                
                if (startRow > targetRow)
                {
                    firstRow = targetRow;
                    lastRow = startRow;
                }
                
                for (int i = firstRow + 1; i < lastRow; i++)
                {
                    if (_currentBoard[i][targetColumn] == 1)
                    {
                        return false;
                    }
                }
                
                return true;
            }
            
            
            return false;
        }

        private void FillBoardWithPiece(int startRow, int startColumn, int targetRow, int targetColumn)
        {
            if (startRow != -1 && startColumn != -1)
            {
                _currentBoard[startRow][startColumn] = 0;
            }
            _currentBoard[targetRow][targetColumn] = 1;
        }
        
        public bool CheckEndGame(List<Vector2Int> piecesCoords)
        {
            foreach (var winPattern in _winPatterns)
            {
                bool gameEnded = true;
                foreach (Vector2Int coordinates in piecesCoords)
                {
                    if (coordinates.x == -1 || coordinates.y == -1)
                    {
                        gameEnded = false;
                        continue;
                    }
                    
                    if (winPattern[coordinates.x][coordinates.y] == 0)
                    {
                       gameEnded = false;
                       break;
                    }
                }

                if (gameEnded)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CheckAvailableMoves(PlayerId playerId, int startRow, int startColumn, PieceId pieceID, GameplayMode mode)
        {
            for (int i = 0; i < _boardSettings.rows; i++)
            {
                for (int j = 0; j < _boardSettings.columns; j++)
                {
                    if (_currentBoard[i][j] == 0)
                    {
                        bool validMove = ValidateMove(startRow, startColumn, pieceID, i, j, mode, true);
                        if (validMove)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}