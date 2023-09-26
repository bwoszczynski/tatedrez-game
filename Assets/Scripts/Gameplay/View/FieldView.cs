using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameplay.View
{
    public class FieldView : BaseView, IDropHandler
    {
        public Color lightColor;
        public Color darkColor;
        public Image background; 
        private Action<PieceView, FieldView> _pieceDroppedCallback;
        
        public void SetDroppedCallback(Action<PieceView, FieldView> pieceDroppedCallback)
        {
            _pieceDroppedCallback = pieceDroppedCallback;
        }

        public void Init(int rowNum, int columnNum, bool isLight)
        {
            row = rowNum;
            column = columnNum;
            gameObject.SetActive(true);
            background.color = isLight ? lightColor : darkColor;
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            PieceView pieceView = eventData.pointerDrag.GetComponent<PieceView>();
            _pieceDroppedCallback.Invoke(pieceView, this);
        }
    }
}