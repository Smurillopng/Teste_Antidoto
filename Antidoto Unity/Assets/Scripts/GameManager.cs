// Criado por: Sergio Murillo da Costa Faria

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region === Variables ==============================================================================================
    
    [Header("Animation Settings")] [SerializeField, Tooltip("O botão que inicia o jogo")]
    private GameObject startButton;

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

    [SerializeField, Tooltip("Array que contêm as informações das perguntas, inclúi alternativas e respostas")]
    private QuestionData[] questionSets;

    [Header("Counter")] [SerializeField, Tooltip("Texto que mostra o número da questão atual")]
    private TMP_Text counterText;

    [Header("Debug")] [SerializeField, Tooltip("Tempo de espera entre troca de textos")]
    private int delay;

    private readonly List<GameObject> _buttons = new(); // Lista responsável por organizar botões instanciados
    private int _currentIndex; // Representa a posição da pergunta atual
    private int _questionsTotal; // Representa o total de perguntas dentro do "questionSets"
    private int _correctAnswers;
    private QuestionData _currentQuestion;
    
    #endregion =========================================================================================================

    #region === Methods ================================================================================================
    private void Awake()
    {
        _questionsTotal = questionSets.Length;
    }

    /// <summary>
    /// Inicia o jogo e toca as animações de transição.
    /// </summary>
    public void StartGame()
    {
        startButton.SetActive(false);
        titleAnimator.SetTrigger(startTrigger);
        iconAnimator.SetTrigger(startTrigger);
        counterAnimator.SetTrigger(startTrigger);
    }

    /// <summary>
    /// Seleciona a questão por index e mostra no jogo.
    /// </summary>
    /// <param name="index">número da questão no array.</param>
    public void SelectQuestionIndex(int index)
    {
        questionsCanvas.SetActive(true);
        StartCoroutine(SetQuestions(questionSets[index]));
        _currentIndex = index;
    }

    /// <summary>
    /// Define as perguntas e respostas para exibição.
    /// </summary>
    /// <param name="question">Os dados da pergunta a ser exibida.</param>
    private IEnumerator SetQuestions(QuestionData question)
    {
        yield return new WaitForSeconds(1f);
        DisplayQuestion(question);
    }

    /// <summary>
    /// Mostra a pergunta atual no jogo.
    /// </summary>
    /// <param name="question">A pergunta a ser exibida.</param>
    private void DisplayQuestion(QuestionData question)
    {
        _currentQuestion = question;
        counterText.text = $"{_currentIndex + 1}/{_questionsTotal}";
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
            buttonComponent.onClick.AddListener(Correct);
        }
        else
        {
            buttonComponent.onClick.AddListener(Wrong);
        }
    }

    /// <summary>
    /// Limpa os botões de resposta.
    /// </summary>
    private void ClearButtons()
    {
        foreach (var button in _buttons)
        {
            Destroy(button);
        }

        _buttons.Clear();
    }

    /// <summary>
    /// Define a próxima pergunta ou reseta o jogo se todas as perguntas foram respondidas.
    /// </summary>
    private void SetNextQuestion()
    {
        _currentIndex++;
        if (_currentIndex < _questionsTotal)
        {
            DisplayQuestion(questionSets[_currentIndex]);
        }
        else
        {
            StartCoroutine(ResetGame());
        }
    }

    /// <summary>
    /// Ação executada quando a resposta está errada.
    /// </summary>
    private void Wrong()
    {
        questionText.text = _currentQuestion.WrongText;
        ShowResults();
    }

    /// <summary>
    /// Ação executada quando a resposta está correta.
    /// </summary>
    private void Correct()
    {
        questionText.text = _currentQuestion.CorrectText;
        _correctAnswers++;
        ShowResults();
    }

    /// <summary>
    /// Mostra os resultados após responder a uma pergunta.
    /// </summary>
    private void ShowResults()
    {
        ClearButtons();

        Invoke(nameof(SetNextQuestion), delay);
    }

    /// <summary>
    /// Reinicia o jogo.
    /// </summary>
    private IEnumerator ResetGame()
    {
        var finalMessage = $"Você acertou {_correctAnswers} perguntas de {_questionsTotal}\n";
        if (_correctAnswers == _questionsTotal) finalMessage += "Parabéns!";
        else finalMessage += "Boa sorte na próxima vez :)";
        questionText.text = finalMessage;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    #endregion =========================================================================================================
}