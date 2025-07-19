using System;
using UnityEditor;
using EditorAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Misc {
    public class ForceObjectsToGround : MonoBehaviour {
#if UNITY_EDITOR
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private float minScale = 1.0f;
        [SerializeField] private float maxScale = 1.0f;

        public bool OnlyRotate90;
        
        [Button("Move Children To Ground")]
        private void MoveChildrenToGround() {
            Action<Transform> move = (Transform t) => {
                RaycastHit hit;
                if (Physics.Raycast(new Ray(t.position + Vector3.up * 10, Vector3.down), out hit, 20, groundLayerMask)) {
                    t.transform.position = hit.point;
                    EditorUtility.SetDirty(t.gameObject);
                }
            };
            ApplyToAllChildren(move);
        }

        [Button("Rotate Children")]
        private void RotateChildren() {
            Action<Transform> rotate = (Transform t) => {
                if (OnlyRotate90) {
                    t.transform.Rotate(new Vector3(
                        0,
                        Random.Range(0, 4) * 90,
                        0
                    ));
                }
                else {
                    t.transform.Rotate(new Vector3(
                        0,
                        Random.Range(0, 360),
                        0
                    ));
                }
                EditorUtility.SetDirty(t.gameObject);
            };
            ApplyToAllChildren(rotate);
        }

        [Button("Scale Children")]
        private void ScaleChildren() {
            Action<Transform> scale = (Transform t) => {
                float randomScale = Random.Range(minScale, maxScale);
                t.transform.localScale = Vector3.one * randomScale;
                EditorUtility.SetDirty(t.gameObject);
            };
            ApplyToAllChildren(scale);
        }

        private void ApplyToAllChildren(Action<Transform> action) {
            for (int i = 0; i < transform.childCount; i++) {
                var childTransform = transform.GetChild(i);
                action(childTransform);
            }
        }
#endif
    }
}