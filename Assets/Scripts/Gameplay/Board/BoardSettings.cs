using System.Collections.Generic;
using Gameplay.Enum;
using UnityEngine;

namespace Gameplay.Board
{
    public class BoardSettings : MonoBehaviour
    {
        public int boardWidth;
        public int boardHeight;
        public int rows;
        public int columns;
        public List<PieceId> player1Pieces;
        public List<PieceId> player2Pieces;
    }
}