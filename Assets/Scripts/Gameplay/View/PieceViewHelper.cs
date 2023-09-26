using System;
using Gameplay.Enum;
using UnityEngine.UI;

namespace Gameplay.View
{
    [Serializable]
    public class PieceViewHelper
    {
        public PieceId pieceId;
        public PlayerId ownerId;
        public Image image;
    }
}