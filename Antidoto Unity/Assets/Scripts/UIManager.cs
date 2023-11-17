// Criado por: Sergio Murillo da Costa Faria

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Animation Settings")] [SerializeField, Tooltip("O botão que inicia o jogo")]
    private GameObject startButton;
    
    [SerializeField, Tooltip("O botão para sair do jogo")]
    private GameObject exitButton;

    [SerializeField, Tooltip("Componente Animator do título")]
    private Animator titleAnimator;

    [SerializeField, Tooltip("Componente Animator do ícone")]
    private Animator iconAnimator;

    [SerializeField, Tooltip("Componente Animator do contador")]
    private Animator counterAnimator;

    [SerializeField, Tooltip("O trigger responsável por ativar as animações iniciais")]
    private string startTrigger;
    
    [Header("Question Settings")] [SerializeField, Tooltip("Prefab para os botões de resposta")]
    private GameObject buttonPrefab;

    [SerializeField, Tooltip("Painel que contêm os botões de resposta")]
    private GameObject buttonsPanel;

    [SerializeField, Tooltip("Canvas onde ficarão os botões e a pergunta")]
    private GameObject questionsCanvas;

    [SerializeField, Tooltip("Texto com a pergunta")]
    private TMP_Text questionText;
    
    [Header("Counter")] [SerializeField, Tooltip("Texto que mostra o número da questão atual")]
    private TMP_Text counterText;

    [Header("Debug")] [SerializeField, Tooltip("Tempo de espera entre troca de textos")]
    private int delay;

    private readonly List<GameObject> _buttons = new(); // Lista responsável por organizar botões instanciados

    public TMP_Text QuestionText => questionText;
    public GameObject QuestionCanvas => questionsCanvas;
    public int Delay => delay;
    
    /// <summary>
    /// Desabilita o botão de start game e da play nas animações iniciais.
    /// </summary>
    public void SetGame()
    {
        startButton.SetActive(false);
        exitButton.SetActive(false);
        titleAnimator.SetTrigger(startTrigger);
        iconAnimator.SetTrigger(startTrigger);
        counterAnimator.SetTrigger(startTrigger);
    }

    /// <summary>
    /// Mostra a pergunta atual no jogo.
    /// </summary>
    /// <param name="question">A pergunta a ser exibida.</param>
    /// <param name="currentQuestion">Informações da questão atual</param>
    /// <param name="questionIndex">Número da questão atual</param>
    /// <param name="totalNumberOfQuestions">Número do total de questões dentro do set</param>
    public void DisplayQuestion(int questionIndex, int totalNumberOfQuestions, QuestionData currentQuestion, QuestionData question)
    {
        currentQuestion = question;
        counterText.text = $"{questionIndex + 1}/{totalNumberOfQuestions}";
        questionText.gameObject.SetActive(true);
        questionText.text = question.Question;

        CreateAnswerButtons(question);
    }
    
    /// <summary>
    /// Cria botões de resposta com base nos dados da pergunta.
    /// </summary>
    /// <param name="question">Os dados da pergunta.</param>
    private void CreateAnswerButtons(QuestionData question)
    {
        ClearButtons();

        for (var i = 0; i < question.Answers.Length; i++)
        {
            var newButton = InstantiateButton(i, question.Answers[i]);
            SetupButtonListeners(newButton, i, question);
        }
    }
    
    /// <summary>
    /// Instancia um botão de resposta.
    /// </summary>
    /// <param name="index">O índice do botão.</param>
    /// <param name="answerText">O texto da resposta.</param>
    /// <returns>O botão instanciado.</returns>
    private GameObject InstantiateButton(int index, string answerText)
    {
        var newButton = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, buttonsPanel.transform);
        _buttons.Add(newButton);
        newButton.name = $"Option {index}";
        newButton.GetComponentInChildren<TMP_Text>().text = answerText;
        return newButton;
    }
    
    /// <summary>
    /// Configura os ouvintes dos botões de resposta.
    /// </summary>
    /// <param name="button">O botão a ser configurado.</param>
    /// <param name="index">O índice do botão.</param>
    /// <param name="question">A pergunta.</param>
    private void SetupButtonListeners(GameObject button, int index, QuestionData question)
    {
        var buttonComponent = button.GetComponent<Button>();
        if (index == question.CorrectAnswer)
        {
            buttonComponent.onClick.AddListener(() =>
            {
                QuestionManager.OnNextQuestion.Invoke();
                questionText.text = question.CorrectText;
                QuestionManager.OnCorrectAnswer.Invoke();
            });
        }
        else
        {
            buttonComponent.onClick.AddListener(() =>
            {
                questionText.text = question.WrongText;
                QuestionManager.OnNextQuestion.Invoke();
            });
        }
    }
    
    /// <summary>
    /// Limpa os botões de resposta.
    /// </summary>
    public void ClearButtons()
    {
        foreach (var button in _buttons)
        {
            Destroy(button);
        }

        _buttons.Clear();
    }
}