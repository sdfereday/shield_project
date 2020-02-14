﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Game.Constants;

namespace Game.Dialogue
{
    // How do we know who is speaking?
    public class DialogueManager : MonoBehaviour
    {
        public delegate void StartedAction();
        public static event StartedAction OnConversationStarted;

        public delegate void CompleteAction();
        public static event CompleteAction OnConversationComplete;

        public delegate void NextAction(DialogueNode nodeData);
        public static event NextAction OnNext;

        public GameObject DialogueBox;
        public Text NameField;
        public Text DialogueField;
        public GameObject ButtonContainer;
        public GameObject NextButtonPrefab;
        public GameObject ChoiceButtonPrefab;
        public bool debug = false;

        private DialogueIterator chatIterator;
        private bool WaitingForChoices { get; set; }
        public bool IsActive { get; private set; }
        public bool ExitScheduled { get; private set; }

        // TODO: Set up a simple animation slide in, as this is a bit crap.
        private void Awake()
        {
            DialogueBox.SetActive(false);
            ClearButtons();
        }

        private void ClearButtons()
        {
            if (ButtonContainer.transform.childCount > 0)
            {
                foreach (Transform child in ButtonContainer.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }


        // TODO: Belongs in an effects helper
        private IEnumerator TypeSentence(DialogueNode node)
        {
            OnNext?.Invoke(node);

            NameField.text = node.ActorName;
            DialogueField.text = "";

            foreach (char letter in node.Text.ToCharArray())
            {
                DialogueField.text += letter;
                yield return null;
            }

            // Used later for setting first highlighted button
            var buttonCache = new List<GameObject>();

            // TODO: I don't think we should instantiate buttons, maybe just have them on standby but hidden. Way
            // more performant than adding event triggers all the time.
            if (node.HasChoices)
            {
                WaitingForChoices = true;
                // Load choices available
                Log("Choices available: " + node.Choices.Count);

                // TODO: You'll have to somehow pass things with the nodes here. Perhaps make
                // a small class to pass, or, some sort of event listener?
                // Or however many you need...
                node.Choices.ForEach(choice =>
                {
                    GameObject ButtonObj = Instantiate(ChoiceButtonPrefab, ButtonContainer.transform.position, Quaternion.identity, ButtonContainer.transform);
                    buttonCache.Add(ButtonObj);

                    ButtonObj.transform.Find("Text").GetComponent<Text>()
                        .text = choice.Text;
                    
                    /* onClick is deprecated */
                    ButtonObj.GetComponent<Button>()
                        .onClick.AddListener(() =>
                        {
                            WaitingForChoices = false;
                            Next(choice.To);
                        });
                });

            }
            else if(NextButtonPrefab != null)
            {
                GameObject ButtonObj = Instantiate(NextButtonPrefab, ButtonContainer.transform.position, Quaternion.identity, ButtonContainer.transform);
                buttonCache.Add(ButtonObj);

                ButtonObj.transform.Find("Text").GetComponent<Text>()
                    .text = "Next";

                /* onClick is deprecated */
                ButtonObj.GetComponent<Button>()
                    .onClick.AddListener(() => Next());
            }

            if (buttonCache.Count > 0)
            {
                // Apparently you have to null it before it'll pick up the actual button after that.
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(buttonCache[0]);
            }
        }

        private void OnChatComplete()
        {
            IsActive = false;
            ExitScheduled = false;
            DialogueBox.SetActive(false);
            OnConversationComplete?.Invoke();
        }

        public void StartDialogue(string startChatId, List<DialogueNode> chatData)
        {
            if (chatData.Count == 0)
            {
                throw new UnityException(GlobalConsts.LIST_WAS_EMPTY + transform.name);
            }

            List<DialogueNode> parsedChat = new List<DialogueNode>(chatData);          
            chatIterator = new DialogueIterator(parsedChat);

            OnConversationStarted?.Invoke();

            IsActive = true;
            DialogueBox.SetActive(true);
            Next(startChatId);
        }

        public void Next(string nodeId = null)
        {
            if (WaitingForChoices || !IsActive) return;

            if (ExitScheduled)
            {
                OnChatComplete();
                return;
            }

            ClearButtons();

            DialogueNode node = chatIterator.GoToNext(nodeId);

            if (node == null)
            {
                Debug.LogError("Chat quit unexpectedly. Couldn't find a node to display.");
                OnChatComplete();
                return;
            }

            Log(node.Text);
            DialogueField.text = node.Text;

            ExitScheduled = node.IsLast;

            StopAllCoroutines();
            StartCoroutine(TypeSentence(node));
        }
        
        public void SetNameField(string name) => NameField.text = name;

        public void Log(string t)
        {
            if (debug) Debug.Log(t);
        }
    }
}