using System;
using System.Collections.Generic;
using Facade.Sound;
using Gameplay.Enum;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Gameplay.View
{
    public class PieceView : BaseView, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        public PieceId Id => _id;
        public PlayerId OwnerId => _ownerId;

        public List<PieceViewHelper> pieceViewHelpers;
        public CanvasGroup canvasGroup;
        
        private PieceId _id;
        private PlayerId _ownerId;
        private bool _isDragging;
        private Vector3 _startPosition = new();
        private bool _draggable = true;
        private SoundManager _soundManager;

        private void Start()
        {
            _startPosition = rectTransform.localPosition;
        }

        public void Init(PieceId pieceId, PlayerId ownerId, Vector2 size)
        {
            _soundManager = ApplicationManager.Instance.SoundManager;
            
            _id = pieceId;
            _ownerId = ownerId;
            gameObject.SetActive(true);
            foreach (PieceViewHelper pieceViewHelper in pieceViewHelpers)
            {
                if (pieceViewHelper.pieceId == _id && pieceViewHelper.ownerId == _ownerId)
                {
                    pieceViewHelper.image.gameObject.SetActive(true);
                    pieceViewHelper.image.rectTransform.sizeDelta = size;
                }
                else
                {
                    pieceViewHelper.image.gameObject.SetActive(false);
                }
            }
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_draggable)
            {
                eventData.pointerDrag = null;
                return;
            }

            gameObject.transform.SetAsLastSibling();
            canvasGroup.blocksRaycasts = false;
            _isDragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_draggable)
            {
                return;
            }

            if (_isDragging)
            {
                transform.position = eventData.position;
            }
        }

        public void SetPosition(int targetRow, int targetColumn, Vector3 newPosition, Transform parentTransform)
        {
            if (gameObject.transform.parent != parentTransform)
            {
                gameObject.transform.SetParent(parentTransform, false);
            }

            row = targetRow;
            column = targetColumn;
            canvasGroup.blocksRaycasts = true;
            _isDragging = false;
            rectTransform.localPosition = newPosition;
            _startPosition = newPosition;
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentTransform.GetComponent<RectTransform>());
        }

        public void UpdateView(bool locked)
        {
            _draggable = !locked;
        }
        
        public void ResetPosition()
        {
            _soundManager.PlaySound(SoundId.ButtonClick);
            SetPosition(row, column, _startPosition, gameObject.transform.parent);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ResetPosition();
        }

        public void OnDrop(PointerEventData eventData)
        {
            ResetPosition();
        }

        public void OnDestroy()
        {
            _soundManager = null;
        }
    }
}