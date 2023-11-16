using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private Button _buttonComponent;
    private AudioSource _audioSource;
    
    private void Awake()
    {
        _buttonComponent = GetComponent<Button>();
        _audioSource = GameObject.FindWithTag("ButtonSound").GetComponent<AudioSource>();
        _buttonComponent.onClick.AddListener(_audioSource.Play);
    }
}