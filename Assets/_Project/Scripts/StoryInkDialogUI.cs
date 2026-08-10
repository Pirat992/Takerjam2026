using UnityEngine.SceneManagement;
using Ink.UnityIntegration;
using UnityEngine.UI;
using Ink.Runtime;
using UnityEngine;

namespace Game
{
    public class StoryInkDialogUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Text dialogueText;
        [SerializeField] private Transform choicesContainer;
        [SerializeField] private Button choiceButtonTemplate;
        [SerializeField] private GameObject interactionPrompt;
        [SerializeField] private InkFile inkFile;
        [SerializeField] private string startKnot;

        private Story story;
        private bool showingChoices;

        private void Start()
        {
            Show();
            StartStory(inkFile, startKnot);
        }

        public void StartStory(InkFile inkFile, string startKnot)
        {
            if (inkFile == null || !inkFile.isCompiled)
            {
                Hide();
                return;
            }

            story = new Story(inkFile.storyJson);
            if (!string.IsNullOrEmpty(startKnot))
                story.ChoosePathString(startKnot);

            Show();
            Advance();
        }

        public void Advance()
        {
            if (story == null)
                return;

            ClearChoices();
            showingChoices = false;

            if (story.canContinue)
            {
                dialogueText.text = story.Continue();
            }
            else if (story.currentChoices.Count > 0)
            {
                showingChoices = true;
                foreach (Choice choice in story.currentChoices)
                {
                    Button button = CreateChoiceButton(choice.text);
                    button.onClick.AddListener(() =>
                    {
                        story.ChooseChoiceIndex(choice.index);
                        Advance();
                    });
                }
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        public void ShowInteractionPrompt(bool show)
        {
            if (interactionPrompt != null)
                interactionPrompt.SetActive(show);
        }

        private void Show()
        {
            panel.SetActive(true);
            ShowInteractionPrompt(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Hide()
        {
            story = null;
            showingChoices = false;
            ClearChoices();
            panel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void ClearChoices()
        {
            if (choicesContainer == null)
                return;
            foreach (Transform child in choicesContainer)
                Destroy(child.gameObject);
        }

        private Button CreateChoiceButton(string choiceText)
        {
            Button button = Instantiate(choiceButtonTemplate, choicesContainer);
            button.gameObject.SetActive(true);
            button.onClick.RemoveAllListeners();
            button.GetComponentInChildren<Text>().text = choiceText;
            return button;
        }
    }
}