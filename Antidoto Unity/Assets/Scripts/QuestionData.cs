using System;
using UnityEngine;

[Serializable]
public struct QuestionData
{
    [SerializeField, TextArea] private string question;
    [SerializeField] private string[] answers;
    [SerializeField] private int correctAnswer;
    [SerializeField, TextArea] private string correctText;
    [SerializeField, TextArea] private string wrongText;

    public string Question => question;
    public string[] Answers => answers;
    public int CorrectAnswer => correctAnswer;
    public string CorrectText => correctText;
    public string WrongText => wrongText;
}