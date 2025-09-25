using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace EclipseProtocol.MapEditor
{
    public class MapEditorController : MonoBehaviour
    {
        [System.Serializable]
        public class PaletteEntry
        {
            public string name;
            public GameObject prefab;
            public Sprite icon;
        }

        [SerializeField] private List<PaletteEntry> palette = new();
        [SerializeField] private Transform paletteRoot;
        [SerializeField] private LayerMask placementMask;
        [SerializeField] private float gridSize = 1f;
        [SerializeField] private UnityEvent<GameObject> onObjectPlaced;
        [SerializeField] private UnityEvent<GameObject> onObjectRemoved;

        private PlayerInput input;
        private int currentPaletteIndex;
        private Camera editorCamera;
        private readonly List<GameObject> placedObjects = new();

        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            editorCamera = Camera.main;
        }

        private void OnEnable()
        {
            input.actions["NextPalette"].performed += OnNextPalette;
            input.actions["PrevPalette"].performed += OnPrevPalette;
            input.actions["Place"].performed += OnPlace;
            input.actions["Delete"].performed += OnDelete;
        }

        private void OnDisable()
        {
            input.actions["NextPalette"].performed -= OnNextPalette;
            input.actions["PrevPalette"].performed -= OnPrevPalette;
            input.actions["Place"].performed -= OnPlace;
            input.actions["Delete"].performed -= OnDelete;
        }

        private void OnNextPalette(InputAction.CallbackContext context)
        {
            currentPaletteIndex = (currentPaletteIndex + 1) % palette.Count;
            UpdatePaletteUI();
        }

        private void OnPrevPalette(InputAction.CallbackContext context)
        {
            currentPaletteIndex = (currentPaletteIndex - 1 + palette.Count) % palette.Count;
            UpdatePaletteUI();
        }

        private void OnPlace(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            if (TryGetPlacementPosition(out Vector3 position, out Quaternion rotation))
            {
                GameObject instance = Instantiate(palette[currentPaletteIndex].prefab, position, rotation, paletteRoot);
                placedObjects.Add(instance);
                onObjectPlaced?.Invoke(instance);
            }
        }

        private void OnDelete(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            if (TryGetHoveredObject(out GameObject hovered))
            {
                placedObjects.Remove(hovered);
                onObjectRemoved?.Invoke(hovered);
                Destroy(hovered);
            }
        }

        private bool TryGetPlacementPosition(out Vector3 position, out Quaternion rotation)
        {
            Ray ray = editorCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, placementMask))
            {
                position = SnapToGrid(hit.point);
                rotation = Quaternion.identity;
                return true;
            }

            position = default;
            rotation = default;
            return false;
        }

        private bool TryGetHoveredObject(out GameObject hovered)
        {
            Ray ray = editorCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 500f))
            {
                hovered = hit.collider.gameObject;
                return placedObjects.Contains(hovered);
            }

            hovered = null;
            return false;
        }

        private Vector3 SnapToGrid(Vector3 position)
        {
            position /= gridSize;
            position = new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), Mathf.Round(position.z));
            position *= gridSize;
            return position;
        }

        private void UpdatePaletteUI()
        {
            // Hook into UI Toolkit/UGUI to update selection state.
        }

        public void ExportLayout(string filePath)
        {
            var serialisable = new MapLayoutData
            {
                entries = new List<MapLayoutData.Entry>()
            };

            foreach (GameObject instance in placedObjects)
            {
                serialisable.entries.Add(new MapLayoutData.Entry
                {
                    prefabGuid = instance.name,
                    position = instance.transform.position,
                    rotation = instance.transform.rotation
                });
            }

            string json = JsonUtility.ToJson(serialisable, true);
            System.IO.File.WriteAllText(filePath, json);
        }
    }
}
