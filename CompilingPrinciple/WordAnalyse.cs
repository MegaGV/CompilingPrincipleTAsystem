using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilingPrinciple
{
    class WordAnalyse
    {
        private List<string> keywords;
        private List<string> operators;
        private List<char> delimiters;

        private List<Token> tokens;
        private List<WrongToken> wrongtokens;

        public WordAnalyse()
        {
            Initial();
        }

        public void Initial()
        {
            string[] keywordStr = {
                "auto", "short", "int", "long", "float", "double", "char", "struct",
                "union","enmu", "typedef", "const", "unsigned", "signed", "extern","register",
                "static", "volatile", "void", "if", "else", "switch", "case", "for",
                "do", "while", "goto", "continue","break", "default", "sizeof", "return"
            };
            string[] operatorStr = {
                "&&", "||", "!", "~", "++", "--", "+", "-", "*", "/", "%",
                ">>", "<<", "<", ">", "<=", ">=", "==", "!=",
                "+=", "-=", "*=", "/=", "%="
            };
            char[] delimiterStr = {
                '(', ')', '[', ']', '{', '}', ',', ';', '=', ' ', '\n'
            };

            keywords = new List<string>(keywordStr);
            operators = new List<string>(operatorStr);
            delimiters = new List<char>(delimiterStr);
            tokens = new List<Token>();
            wrongtokens = new List<WrongToken>();
    }

        public void Analyse(string contents)
        {
            tokens = new List<Token>();
            wrongtokens = new List<WrongToken>();

            string[] words = contents.Split(new char[] {' '});
            int line = 1;
            foreach (string word in words)
            {
                line = addToken(word, line);
            }
            
        }

        public int addToken(string word, int line)
        {
            for (int i = 0; i < word.Length; i++)
            {
                bool error = false;
                if (isDelimiter(word[i])){
                    if (word[i] == '\n')
                        line++;
                    else
                        tokens.Add(new Token(TokenType.delimiter, word[i].ToString(), 400 + delimiters.IndexOf(word[i]), line));
                }
                else if (isLetter(word[i]))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(word[i]);
                    while (true)
                    {
                        i++;
                        if (i >= word.Length) 
                            break;
                        if (isDelimiter(word[i]))
                            break;
                        if (!(isLetter(word[i]) || isDigit(word[i])))
                        {
                            error = true;
                            break;
                        }
                        sb.Append(word[i]);
                    }

                    if (error)
                    {
                        sb.Append(word[i]);
                        System.Console.WriteLine("WARNING, invalid identifier " + sb.ToString() + " found");
                        wrongtokens.Add(new WrongToken(sb.ToString(), line));
                    }
                    //如果是关键字
                    else if (isKeyword(sb.ToString()))
                    {
                        i--;
                        tokens.Add(new Token(TokenType.keyword, sb.ToString(), 200 + keywords.IndexOf(sb.ToString()), line));
                    }
                    //不是保留字，则为标识符，需要保存值
                    else
                    {
                        i--;
                        tokens.Add(new Token(TokenType.identifier, sb.ToString(), 0, line));
                    } 
                }
                else if (isDigit(word[i]))
                { //首位为数字，即为整数
                    StringBuilder sb = new StringBuilder();
                    sb.Append(word[i]);
                    while (true)
                    {
                        i++;
                        if (i >= word.Length)
                            break;
                        if (isDelimiter(word[i]))
                            break;
                        if (!isDigit(word[i]))
                        {
                            error = true;
                            break;
                        }
                        sb.Append(word[i]);
                    }
                    
                    if (error)
                    {
                        sb.Append(word[i]);
                        System.Console.WriteLine("WARNING, invalid digit " + sb.ToString() + " found");
                        wrongtokens.Add(new WrongToken(sb.ToString(), line));
                    }
                    else
                    {
                        i--;
                        tokens.Add(new Token(TokenType.digit, sb.ToString(), 1, line));
                    }
                }
                else if (isOperator(word[i]))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(word[i]);
                    if (i + 1 < word.Length && isOperator(word[i + 1]))
                    {
                        sb.Append(word[++i]);
                        if (isOperator(sb.ToString()))
                        {
                            tokens.Add(new Token(TokenType.operation, sb.ToString(), 300 + operators.IndexOf(sb.ToString()), line));
                        }
                        else
                        {
                            System.Console.WriteLine("WARNING, invalid operator" + sb.ToString() + " found");
                            wrongtokens.Add(new WrongToken(sb.ToString(), line));
                        }
                    }
                    tokens.Add(new Token(TokenType.operation, sb.ToString(), 300 + operators.IndexOf(sb.ToString()), line));
                }
                else
                {
                    System.Console.WriteLine("WARNING, invalid word" + word[i].ToString() + " found");
                    wrongtokens.Add(new WrongToken(word[i].ToString(), line));
                }
                    
            }
            return line;
        }

        public List<Token> getTokens()
        {
            return this.tokens;
        }

        public List<WrongToken> getWrongTokens()
        {
            return this.wrongtokens;
        }

        public bool isLetter(char ch)
        {
            return char.IsLetter(ch);
        }

        public bool isDigit(char ch)
        {
            return char.IsDigit(ch);
        }

        public bool isDelimiter(char ch)
        {
            return delimiters.Contains(ch);
        }

        public bool isOperator(string str)
        {
            return operators.Contains(str);
        }

        public bool isOperator(char ch)
        {
            return operators.Contains(ch.ToString());
        }

        public bool isKeyword(string str)
        {
            return keywords.Contains(str);
        }

        

    }
}
