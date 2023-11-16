// Criado por: Sergio Murillo da Costa Faria

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class QuestionManager : MonoBehaviour
{
    #region === Variables ==============================================================================================
    
    [SerializeField, Tooltip("Array que contêm as informações das perguntas, inclúi alternativas e respostas")]
    private QuestionData[] questionSets;

    [SerializeField, Tooltip("Referência ao Gerenciador da interface")]
    private UIManager uiManager;

    public static UnityAction OnNextQuestion;
    public static UnityAction OnCorrectAnswer;
    
    private int _currentIndex; // Representa a posição da pergunta atual
    private QuestionData _currentQuestion; // Informações da quest]ao atual
    
    public int CorrectAnswers { get; private set; } // Quantas respostas corretas o jogador teve
    public int QuestionsTotal { get; private set; } // Representa o total de perguntas dentro do "questionSets"

    #endregion =========================================================================================================
    
    private void Awake()
    {
        QuestionsTotal = questionSets.Length;
    }

    private void OnEnable()
    {
        OnCorrectAnswer += AddScore;
        OnNextQuestion += SetNextQuestion;
    }

    private void OnDisable()
    {
        OnCorrectAnswer -= AddScore;
        OnNextQuestion -= SetNextQuestion;
    }

    /// <summary>
    /// Aumenta o contador de respostas corretas do jogador
    /// </summary>
    private void AddScore()
    {
        CorrectAnswers++;
    }
    
    /// <summary>
    /// Seleciona a questão por index e mostra no jogo.
    /// </summary>
    /// <param name="index">número da questão no array.</param>
    public void SelectQuestionIndex(int index)
    {
        uiManager.QuestionCanvas.SetActive(true);
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
        uiManager.DisplayQuestion(_currentIndex, QuestionsTotal, _currentQuestion, question);
    }

    
    /// <summary>
    /// Define a próxima pergunta ou reseta o jogo se todas as perguntas foram respondidas.
    /// </summary>
    private void SetNextQuestion()
    {
        StartCoroutine(NextQuestionCoroutine());
    }

    /// <summary>
    /// Exclui botões anteriores, espera por alguns segundos para mostrar o texto de correto ou incorreto e verifica
    /// se existe mais questões, caso não existam o jogo é reiniciado
    /// </summary>
    /// <returns></returns>
    private IEnumerator NextQuestionCoroutine()
    {
        uiManager.ClearButtons();
        yield return new WaitForSeconds(uiManager.Delay);
        _currentIndex++;
        if (_currentIndex < QuestionsTotal)
        {
            uiManager.DisplayQuestion(_currentIndex, QuestionsTotal, _currentQuestion, questionSets[_currentIndex]);
        }
        else
        {
            GameManager.Instance.ResetGame();
        }
    }
}