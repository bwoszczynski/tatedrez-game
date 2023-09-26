using System.Collections.Generic;
using Gameplay.View;
using Tools;
using UnityEngine;

namespace Gameplay.Board
{
    public class BoardElementsFactory : MonoBehaviour
    {
        public ElementsPool<FieldView> fieldsPool;
        public ElementsPool<PieceView> piecesPool;

        public FieldView GetFieldFromPool()
        {
            return fieldsPool.GetElementFromPool();
        }
        
        public PieceView GetPieceFromPool()
        {
            return piecesPool.GetElementFromPool();
        }

        public void ReturnFieldToPool(FieldView field)
        {
            fieldsPool.ReturnToPool(field);
        }

        public void ReturnPieceToPool(PieceView piece)
        {
            piecesPool.ReturnToPool(piece);
        }
    }
}