using UnityEngine;
using UnityEngine.UI;
using EclipseProtocol.Managers;

namespace EclipseProtocol.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button campaignButton;
        [SerializeField] private Button survivalButton;
        [SerializeField] private Button mapEditorButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private CanvasGroup transitionGroup;
        [SerializeField] private float transitionDuration = 1f;

        private void Awake()
        {
            campaignButton.onClick.AddListener(() => StartMode(GameMode.Campaign));
            survivalButton.onClick.AddListener(() => StartMode(GameMode.Survival));
            mapEditorButton.onClick.AddListener(() => StartMode(GameMode.MapEditor));
            quitButton.onClick.AddListener(QuitGame);
        }

        private void StartMode(GameMode mode)
        {
            if (transitionGroup != null)
            {
                StartCoroutine(FadeAndLoad(mode));
            }
            else
            {
                GameModeManager.Instance.LoadGameMode(mode);
            }
        }

        private System.Collections.IEnumerator FadeAndLoad(GameMode mode)
        {
            float elapsed = 0f;
            transitionGroup.blocksRaycasts = true;

            while (elapsed < transitionDuration)
            {
                elapsed += Time.deltaTime;
                transitionGroup.alpha = Mathf.Clamp01(elapsed / transitionDuration);
                yield return null;
            }

            GameModeManager.Instance.LoadGameMode(mode);
        }

        private void QuitGame()
        {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
        }
    }
}
