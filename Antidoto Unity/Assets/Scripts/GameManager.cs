// Criado por: Sergio Murillo da Costa Faria

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region === Variables ==============================================================================================

    public static GameManager Instance; // Instância do GameManager para acesso global.
    
    [SerializeField, Tooltip("Referência so Gerenciador de perguntas")]
    private QuestionManager questionManager;
    
    [SerializeField, Tooltip("Referência ao Gerenciador da interface")] 
    private UIManager uiManager;

    #endregion =========================================================================================================

    #region === Methods ================================================================================================

    /// <summary>
    /// Inicializa a instância do GameManager já que ele é um Singleton.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Inicia o jogo e toca as animações de transição.
    /// </summary>
    public void StartGame()
    {
        uiManager.SetGame();
    }

    public void ResetGame()
    {
        StartCoroutine(ResetGameCoroutine());
    }

    /// <summary>
    /// Reinicia o jogo.
    /// </summary>
    private IEnumerator ResetGameCoroutine()
    {
        var finalMessage =
            $"Você acertou {questionManager.CorrectAnswers} perguntas de {questionManager.QuestionsTotal}\n";
        if (questionManager.CorrectAnswers == questionManager.QuestionsTotal) finalMessage += "Parabéns!";
        else finalMessage += "Boa sorte na próxima vez :)";
        uiManager.QuestionText.text = finalMessage;
        yield return new WaitForSeconds(uiManager.Delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion =========================================================================================================
}