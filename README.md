
# Projeto de Quiz em Unity - Antidoto
Este é um projeto de um Quiz desenvolvido em Unity. O projeto consiste em um jogo de perguntas e respostas, usando uma arquitetura de gerenciamento de jogo, perguntas e interface.

## Estrutura do Código
### GameManager.cs
Este script gerencia o fluxo do jogo. Aqui estão as principais funcionalidades:

- Awake(): Inicializa a instância do GameManager como um Singleton para acesso global.
- StartGame(): Inicia o jogo e toca as animações de transição.
- ResetGame(): Reinicia o jogo quando todas as perguntas foram respondidas.

### QuestionManager.cs
O QuestionManager controla as perguntas, respostas e interações entre elas. Principais funcionalidades:

- Awake(): Inicializa o número total de perguntas no início do jogo.
- SelectQuestionIndex(int index): Mostra a pergunta selecionada no jogo.
- SetNextQuestion(): Define a próxima pergunta ou reinicia o jogo se todas as perguntas foram respondidas.

### UIManager.cs
Este script gerencia a interface do usuário. Suas funcionalidades principais incluem:

- SetGame(): Desabilita o botão de início do jogo e toca as animações iniciais.
- DisplayQuestion(int questionIndex, int totalNumberOfQuestions, QuestionData currentQuestion, QuestionData question): Mostra a pergunta atual no jogo.
- CreateAnswerButtons(QuestionData question): Instancia os botões de resposta com base nos dados da pergunta.

### ButtonController.cs
Este script controla o comportamento dos botões na interface:

- Awake(): Configura o som de clique nos botões para reproduzir um som ao serem clicados.

## Decisões de Design e Otimizações
Singleton Pattern: O GameManager foi implementado como singleton para garantir acesso global e evitar múltiplas instâncias.
Corrotinas (Coroutines): Em vários métodos, como no GameManager e QuestionManager, corrotinas são usadas para controlar o fluxo de execução e sincronização de eventos.

## Notas Adicionais
O projeto segue a convenção de nomenclatura, comentários explicativos e uso de corrotinas para otimizar o desempenho e a lógica do jogo.
Esse é um resumo geral do projeto e seus scripts. Sinta-se à vontade para explorar mais detalhes em cada script para um entendimento mais aprofundado.
As questões são facilmente configuradas dentro do objeto "Question Manager" da cena Main.