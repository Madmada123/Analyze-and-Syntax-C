using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static LabbAnalyz.MainForm;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;


namespace LabbAnalyz
{
    public partial class MainForm : Form
    {


        public MainForm()
        {
            InitializeComponent();
            SyntBox = new TextBox();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SourceCodeTextBox.Text = File.ReadAllText(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading the file: " + ex.Message);
                }
            }
        }

        private void AnalyzeButton_Click(object sender, EventArgs e)
        {
            string sourceCode = SourceCodeTextBox.Text;

            Lexer lexer = new Lexer(sourceCode);
            List<Token> tokens = lexer.Analyze();

            ResultsTextBox1.Clear();

            foreach (Token token in tokens)
            {
                ResultsTextBox1.AppendText(token.ToString() + Environment.NewLine);
            }
        }

        private void SyntaxAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                SyntBox.Text = "Starting syntax analysis...";
                PerformSyntaxAnalysis();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private void AppendToken(RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText(text + "\n"); // Добавление новой строки для каждого токена
            box.SelectionColor = box.ForeColor; // Возвращение к стандартному цвету текста
        }



        private Color GetColorForTokenType(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.KEYWORD:
                    return Color.Blue;
                case TokenType.IDENTIFIER:
                    return Color.Green;
                case TokenType.LITERAL:
                    return Color.Orange;
                case TokenType.OPERATOR:
                    return Color.Red;
                case TokenType.DELIMITER:
                    return Color.Purple;
                default:
                    return Color.Black;
            }
        }



        private static readonly Dictionary<(int, string), (string, int)> parseTable = new Dictionary<(int, string), (string, int)>
        {
            {(0, "void"), ("shift", 1)},
            {(1, "main"), ("shift", 2)},
            {(2, "("), ("shift", 3)},
            {(3, ")"), ("shift", 4)},
            {(4, "{"), ("shift", 5)},
            {(5, "int"), ("shift", 6)},
            {(5, "float"), ("shift", 6)},
            {(5, "double"), ("shift", 6)},
            {(6, "id"), ("shift", 7)},
            {(7, ","), ("shift", 6)},
            {(7, ";"), ("reduce", 2)},
            {(5, "for"), ("shift", 8)},
            {(5, "}"), ("shift", 9)},
            // добавьте остальные переходы и действия в таблицу
        };



        private void PerformSyntaxAnalysis()
        {


            
            
                try
                {
                    string sourceCode = SourceCodeTextBox.Text;
                    Lexer lexer = new Lexer(sourceCode);
                    List<Token> tokens = lexer.Analyze();
                    Stack<int> stateStack = new Stack<int>();
                    stateStack.Push(0);

                    int tokenIndex = 0;

                    ResultsTextBox1.Clear(); // Очистить текстовое поле перед началом анализа

                    while (tokenIndex < tokens.Count)
                    {
                        Token currentToken = tokens[tokenIndex];
                        int currentState = stateStack.Peek();

                        // Логирование текущего состояния и токена
                        ResultsTextBox1.AppendText($"CurrentState: {currentState}, Token: {currentToken.Value}\n");

                        if (parseTable.TryGetValue((currentState, currentToken.Value), out var action))
                        {
                            if (action.Item1 == "shift")
                            {
                                stateStack.Push(action.Item2);
                                tokenIndex++;
                            }
                            else if (action.Item1 == "reduce")
                            {
                                // Реализуйте правила свёртки на основе грамматики
                            }
                        }
                        else
                        {
                            throw new Exception($"Ошибка синтаксического анализа: неожиданный токен '{currentToken.Value}' в состоянии {currentState}");
                        }
                    }

                    SyntBox.Text = "Синтаксический анализ завершен успешно!";
                }
                catch (Exception ex)
                {
                    SyntBox.Text = $"Ошибка синтаксического анализа: {ex.Message}";
                }
            } 


        public enum TokenType
        {
            KEYWORD,
            IDENTIFIER,
            LITERAL,
            OPERATOR,
            DELIMITER,
            COMMENT
        }

        public class Token
        {
            public TokenType Type { get; }
            public string Value { get; }

            public Token(TokenType type, string value)
            {
                Type = type;
                Value = value;
            }

            public override string ToString()
            {
                return $"{Type} -- {Value}";
            }
        }


        public class Lexer
        {
            private readonly string _sourceCode;

            public Lexer(string sourceCode)
            {
                _sourceCode = sourceCode;
            }

            public List<Token> Analyze()
            {
                List<Token> tokens = new List<Token>();

                // Обновленные шаблоны для токенов
                string keywordPattern = "\\b(void|main|int|float|double|for)\\b";
                string identifierPattern = "[a-zA-Z_][a-zA-Z0-9_]{0,7}"; // Ограничение на 8 символов
                string literalPattern = "\\b\\d+\\.?\\d*\\b"; // Для числовых литералов
                string operatorPattern = @"(\+|\-|\*|\/|\<|\>|\<=|\>=|\=\=|\!\=|\=\=|\|\||\&\&)";
                string delimiterPattern = @"([{}();,])"; // Убрал \s+, чтобы не включать пробелы как токены
                string commentPattern = "//.*|/\\*[^*]*\\*/"; // Поддержка однострочных и многострочных комментариев

                // Комбинация всех шаблонов в один
                string pattern = $"{keywordPattern}|{identifierPattern}|{literalPattern}|{operatorPattern}|{delimiterPattern}|{commentPattern}";

                // Сопоставление токенов с использованием регулярных выражений
                MatchCollection matches = Regex.Matches(_sourceCode, pattern);

                foreach (Match match in matches)
                {
                    if (match.Success)
                    {
                        if (Regex.IsMatch(match.Value, keywordPattern))
                        {
                            tokens.Add(new Token(TokenType.KEYWORD, match.Value));
                        }
                        else if (Regex.IsMatch(match.Value, identifierPattern))
                        {
                            tokens.Add(new Token(TokenType.IDENTIFIER, match.Value));
                        }
                        else if (Regex.IsMatch(match.Value, literalPattern))
                        {
                            tokens.Add(new Token(TokenType.LITERAL, match.Value));
                        }
                        else if (Regex.IsMatch(match.Value, operatorPattern))
                        {
                            tokens.Add(new Token(TokenType.OPERATOR, match.Value));
                        }
                        else if (Regex.IsMatch(match.Value, delimiterPattern))
                        {
                            tokens.Add(new Token(TokenType.DELIMITER, match.Value));
                        }
                        else if (Regex.IsMatch(match.Value, commentPattern))
                        {
                            tokens.Add(new Token(TokenType.COMMENT, match.Value));
                        }
                    }
                }

                return tokens;
            }


            private void AppendToken(RichTextBox box, string text, Color color)
            {
                box.SelectionStart = box.TextLength;
                box.SelectionLength = 0;
                box.SelectionColor = color;
                box.AppendText(text + "\n");
                box.SelectionColor = box.ForeColor;
            }

            public class Parser
            {
                private Stack<int> stateStack = new Stack<int>();
                private Stack<string> symbolStack = new Stack<string>();

                // Пример таблицы действий, это должно быть заполнено в соответствии с вашей грамматикой
                private Dictionary<int, Dictionary<string, (string Action, int Number)>> actionTable;

                // Пример таблицы переходов
                private Dictionary<int, Dictionary<string, int>> gotoTable;

                public Parser()
                {
                    // Инициализация таблиц (это просто пример, заполните своими данными)
                    InitializeTables();
                }
                /*
                public void Parse(List<Token> tokens)
                {
                    stateStack.Push(0); // Начальное состояние
                    tokens.Add(new Token("EOF", "$")); // Конец файла или ввода

                    int index = 0;
                    while (index < tokens.Count)
                    {
                        var currentToken = tokens[index];
                        int currentState = stateStack.Peek();
                        if (!actionTable.ContainsKey(currentState) || !actionTable[currentState].ContainsKey(currentToken.Type))
                        {
                            Console.WriteLine("Syntax error!");
                            return;
                        }

                        var action = actionTable[currentState][currentToken.Type];

                        if (action.Action == "SHIFT")
                        {
                            Console.WriteLine($"Shift on {currentToken.Type}");
                            stateStack.Push(action.Number);
                            symbolStack.Push(currentToken.Type);
                            index++; // Переход к следующему токену
                        }
                        else if (action.Action == "REDUCE")
                        {
                            Console.WriteLine($"Reduce using rule {action.Number}");
                            // Здесь нужно узнать, сколько символов сворачиваем
                            int numberOfSymbols = GetNumberOfSymbolsToPop(action.Number);
                            for (int i = 0; i < numberOfSymbols; i++)
                            {
                                symbolStack.Pop();
                                stateStack.Pop();
                            }

                            string nonTerminal = GetNonTerminalForRule(action.Number);
                            symbolStack.Push(nonTerminal);
                            stateStack.Push(gotoTable[stateStack.Peek()][nonTerminal]);
                        }
                        else if (action.Action == "ACCEPT")
                        {
                            Console.WriteLine("Parsing completed successfully!");
                            return;
                        }
                    }
                }
                */
            

                private void InitializeTables()
                {
                    // Здесь нужно инициализировать actionTable и gotoTable с вашими данными
                }

                private int GetNumberOfSymbolsToPop(int ruleNumber)
                {
                    // Возвращаем количество символов для свертки
                    return 2; // Пример
                }

                private string GetNonTerminalForRule(int ruleNumber)
                {
                    // Возвращаем нетерминал, который становится результатом свертки
                    return "S"; // Пример
                }

            }
        }
    }
}