using UnityEngine.InputSystem;
using AZE.AdvancedFirstPerson;
using Ink.UnityIntegration;
using UnityEngine.UI;
using Ink.Runtime;
using UnityEngine;

namespace Game.UI
{
        public class InkDialogUI : MonoBehaviour
        {
                [SerializeField] private GameObject panel;
                [SerializeField] private Text dialogueText;
                [SerializeField] private Transform choicesContainer;
                [SerializeField] private Button choiceButtonTemplate;
                [SerializeField] private InputActionReference clickAction;
                [SerializeField] private InputActionReference advanceAction;
                [SerializeField] private PlayerInputHandler inputHandler;
                [SerializeField] private GameObject interactionPrompt;

                private Story story;
                private bool showingChoices;

                public bool IsOpen => story != null;

                private void Awake()
                {
                        panel.SetActive(false);
                        ShowInteractionPrompt(false);
                        clickAction.action.canceled += TryAdvance;
                        advanceAction.action.canceled += TryAdvance;
                }

                private void TryAdvance(InputAction.CallbackContext context)
                {
                        if (story == null)
                                return;

                        if (showingChoices)
                                return;
                        Advance();
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

                private void Advance()
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
                                Hide();
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
                        clickAction.action.Enable();
                        advanceAction.action.Enable();
                        inputHandler.enabled = false;
                        Cursor.lockState = CursorLockMode.None;
                }

                private void Hide()
                {
                        story = null;
                        showingChoices = false;
                        ClearChoices();
                        panel.SetActive(false);
                        clickAction.action.Disable();
                        advanceAction.action.Disable();
                        inputHandler.enabled = true;
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
