// Criado por: Sergio Murillo da Costa Faria

using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private Button _buttonComponent; // Referência ao componente de Botão
    private AudioSource _audioSource; // AudioSource responsável por tocar o som de click
    
    private void Awake()
    {
        _buttonComponent = GetComponent<Button>();
        _audioSource = GameObject.FindWithTag("ButtonSound").GetComponent<AudioSource>();
        _buttonComponent.onClick.AddListener(_audioSource.Play);
    }
}