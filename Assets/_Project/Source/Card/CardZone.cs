using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Source.Util
{
    public class CardZone : MonoBehaviour
    {
        public List<InteractiveCard> objects = new List<InteractiveCard>();
        public float spacing = 1f;
        public Rect capture;

        public bool isGrid = false;
        public float heightSpacing = 1f;
        public int maxColumns = 1;
        public int maxRows = 1;
        public bool canDrag;
        List<InteractiveCard> alignedSet = new List<InteractiveCard>();
        
        private float AdjustedSpacing => spacing/1920f * Screen.width;
        private float AdjustedHeightSpacing => heightSpacing/1920f * Screen.width;
        private Rect AdjustedCapture;
        async void Start()
        {
            await UniTask.WaitWhile(() => G.main == null);
            G.main.OnReleaseDrag += OnReleaseDrag;
            AdjustCaptureRect();
        }
        
        void AdjustCaptureRect()
        {
            AdjustedCapture = capture;
            float scaleFactor = Screen.width / 1920f;
            AdjustedCapture.width *= scaleFactor;
            AdjustedCapture.height *= scaleFactor;
        }
        
        void Update()
        {
            alignedSet.Clear();
            for (var index = 0; index < objects.Count; index++)
            {
                var o = objects[index];
                if (!o.draggable.isDragging )
                {
                    alignedSet.Add(o);
                }
            }

            for (var i = 0; i < alignedSet.Count; i++)
            {
                var targetPos = GetTargetPos(i, alignedSet);
                alignedSet[i].moveable.targetPosition = targetPos;
                alignedSet[i].order = i;
            }
        }
        
        Vector3 GetTargetPos(int i, List<InteractiveCard> setToWatch)
        {
            if (isGrid)
            {
                int row = i / maxColumns;
                int column = i % maxColumns;
                float xOffset = (column - (maxColumns / 2f - 0.5f)) * AdjustedSpacing;
                float yOffset = (row - (maxRows / 2f - 0.5f)) * AdjustedHeightSpacing;
                return transform.position + Vector3.right * xOffset + Vector3.down * yOffset;
            }

            if (AdjustedSpacing < 1)
            {
                float totalOffset = 0f;

                // Calculate total offset by summing half the width of the current object and the previous objects' widths
                for (int j = 0; j < i; j++)
                {
                    totalOffset += setToWatch[j].Width;
                }

                // Offset the current object by half of its own width for proper centering
                totalOffset += setToWatch[i].Width / 2f;

                // Calculate the current object position centered around the full set
                float centeredOffset = totalOffset - (GetTotalSetWidth(setToWatch) / 2f);
    
                // Return the new target position, taking into account spacing
                return transform.position + Vector3.right * centeredOffset;
            }
        
            var offset = i * AdjustedSpacing - (setToWatch.Count / 2f - 0.5f) * AdjustedSpacing;
            var targetPos = transform.position + Vector3.right * offset;
            return targetPos;
        }

        float GetTotalSetWidth(List<InteractiveCard> setToWatch)
        {
            float totalWidth = 0f;
            foreach (var obj in setToWatch)
            {
                totalWidth += obj.Width;
            }
            return totalWidth;
        }
        
        public bool IsOverlap(InteractiveCard obj)
        {
            AdjustCaptureRect();
            AdjustedCapture.center = transform.position;
            return AdjustedCapture.Contains(obj.transform.position);
        }

        void OnReleaseDrag(InteractiveCard card)
        {
            if (card == null) return;
        
            if (IsOverlap(card))
            {
                TryClaim(card);
            }
        }
        
        public virtual void TryClaim(InteractiveCard toClaim)
        {
            if (!CanClaim(toClaim)) return;
            
            Claim(toClaim);
        }
        
        public virtual void Claim(InteractiveCard toClaim)
        {
            if (toClaim.zone != null)
                toClaim.zone.Release(toClaim);

            toClaim.zone = this;

            var pos = 0;
            // var insertFilters = G.main.interactor.FindAll<IFilterInsertPos>();
            // foreach (var inf in insertFilters)
            //     pos = inf.OverrideIndex(pos, this, toClaim);
            objects.Insert(pos, toClaim);
        }
        
        public async UniTask TryClaimAsync(InteractiveCard toClaim)
        {
            await UniTask.WaitForSeconds(0.1f);
            TryClaim(toClaim);
        }
        
        public void Release(InteractiveCard toClaim)
        {
            if (objects.Contains(toClaim))
                objects.Remove(toClaim);
        }
        
        public virtual bool CanClaim(InteractiveCard card)
        {
            return true;
        }
        
        void OnDrawGizmos()
        {
            if (capture.size != Vector2.zero)
            {
                Gizmos.color = new Color(0.5f, 0, 0, 0.25f);
                Gizmos.DrawCube(transform.position, capture.size);
                Gizmos.DrawWireCube(transform.position, capture.size);
            }
            else
            {
                Gizmos.color = new Color(0.5f, 0, 0, 0.25f);
                Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
                Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 1));
            }
        }
        
        protected void AdjustSiblingIndex(InteractiveCard toClaim)
        {
            var front = objects.FirstOrDefault();
            if (front != null)
            {
                var frontSibIdx = front.transform.GetSiblingIndex();
                var toClaimSibIdx = toClaim.transform.GetSiblingIndex();
                Debug.Log(frontSibIdx + " " + toClaimSibIdx);
                if (frontSibIdx > toClaimSibIdx)
                {
                    toClaim.transform.SetSiblingIndex(frontSibIdx);
                }
            }
        }
    }
}