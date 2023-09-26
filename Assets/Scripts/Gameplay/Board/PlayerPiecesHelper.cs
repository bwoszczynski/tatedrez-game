using System;
using System.Collections.Generic;
using Gameplay.Enum;
using Gameplay.View;
using UnityEngine.UI;

namespace Gameplay.Board
{
    [Serializable]
    public class PlayerPiecesHelper
    {
        public PlayerId playerId;
        public GridLayoutGroup piecesParent;
        public List<PieceView> pieces = new ();

    }
}