using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilingPrinciple
{
    class WordAnalyzer
    {
        private List<string> keywords;
        private List<string> operators;
        private List<string> delimiters;

        private List<Token> tokens;
        private List<WrongToken> wrongtokens;

        public WordAnalyzer()
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
            string[] delimiterStr = {
                "(", ")", "[", "]", "{", "}", ",", ";", "=", " ", "\n", "//", "/*", "*/"
            };

            keywords = new List<string>(keywordStr);
            operators = new List<string>(operatorStr);
            delimiters = new List<string>(delimiterStr);
            tokens = new List<Token>();
            wrongtokens = new List<WrongToken>();
    }

        public void WordAnalyse(string contents)
        {
            tokens = new List<Token>();
            wrongtokens = new List<WrongToken>();

            TokenAnalyse(contents);
        }

        public void TokenAnalyse(string contents)
        {
            int line = 1;
            for (int i = 0; i < contents.Length; i++)
            {
                //注释判断
                if (i < contents.Length - 1 && contents[i] == '/')
                {
                    int j = i + 1;
                    if (contents[j] == '/')
                    {
                        tokens.Add(new Token(TokenType.delimiter, "//", 400 + delimiters.IndexOf("//"), line));
                        while (true)
                        {
                            j++;
                            if (j == contents.Length)
                                return;
                            else if (contents[j] == '\n')
                            {
                                line++;
                                j++;
                                i = j;
                                break;
                            }
                        }
                    }
                    else if (contents[j] == '*')
                    {
                        tokens.Add(new Token(TokenType.delimiter, "/*", 400 + delimiters.IndexOf("/*"), line));
                        while (true)
                        {
                            j++;
                            if (j == contents.Length)
                            {
                                wrongtokens.Add(new WrongToken("/*", line, "invalid comment '/*' no */ found"));
                                tokens.RemoveAt(tokens.Count - 1);
                                return;
                            }
                            else if (contents[j] == '\n')
                                line++;
                            else if (contents[j] == '*' && contents[j + 1] == '/')
                            {
                                tokens.Add(new Token(TokenType.delimiter, "*/", 400 + delimiters.IndexOf("*/"), line));
                                j++;
                                i = j;
                                break;
                            }
                        }
                    }
                }

                //如果是界符
                if (isDelimiter(contents[i])) 
                {
                    if (contents[i] == '\n')//换行符不计入token表内，且递增行数
                        line++;
                    else if (contents[i] != ' ')//界符空格不纳入token表内
                        tokens.Add(new Token(TokenType.delimiter, contents[i].ToString(), 400 + delimiters.IndexOf(contents[i].ToString()), line));
                }
                //如果是字母，进一步判断是否是合法token以及区分关键字还是标识符
                else if (isLetter(contents[i]))
                {
                    bool error = false;
                    StringBuilder sb = new StringBuilder();
                    sb.Append(contents[i]);
                    while (true)
                    {
                        i++;
                        if (i >= contents.Length) 
                            break;
                        if (isDelimiter(contents[i]) || isOperator(contents[i])) //界符或操作符出现时，将之前看作一个token，分开处理
                            break;
                        if (!(isLetter(contents[i]) || isDigit(contents[i]))) //出现字母或数字以外的字符，错误token
                        {
                            error = true;
                            break;
                        }
                        sb.Append(contents[i]);
                    }

                    if (error) //得到错误token，将错误token加入错误列表
                    {
                        sb.Append(contents[i]);
                        wrongtokens.Add(new WrongToken(sb.ToString(), line, "invalid identifier " + sb.ToString() + " found"));
                    }
                    //如果是关键字
                    else if (isKeyword(sb.ToString()))
                    {
                        i--;
                        tokens.Add(new Token(TokenType.keyword, sb.ToString(), 200 + keywords.IndexOf(sb.ToString()), line));
                    }
                    //不是关键字，则为标识符
                    else
                    {
                        i--;
                        tokens.Add(new Token(TokenType.identifier, sb.ToString(), 0, line));
                    } 
                }
                else if (isDigit(contents[i]))
                {
                    //首位为数字，进一步判断是否为整数
                    bool error = false;
                    StringBuilder sb = new StringBuilder();
                    sb.Append(contents[i]);
                    while (true)
                    {
                        i++;
                        if (i >= contents.Length)
                            break;
                        if (isDelimiter(contents[i]) || isOperator(contents[i])) //界符或操作符出现时，将之前看作一个token，分开处理
                            break;
                        if (!isDigit(contents[i])) //出现数字以外的字符，错误token
                        {
                            error = true;
                            break;
                        }
                        sb.Append(contents[i]);
                    }
                    
                    if (error) //得到错误token，将错误token加入错误列表
                    {
                        sb.Append(contents[i]);
                        wrongtokens.Add(new WrongToken(sb.ToString(), line, "invalid digit " + sb.ToString() + " found"));
                    }
                    //得到正确digit token
                    else
                    {
                        i--;
                        tokens.Add(new Token(TokenType.digit, sb.ToString(), 1, line));
                    }
                }
                //如果是运算符
                else if (isOperator(contents[i]))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(contents[i]);
                    //判断是否和下一个字符组成另一个运算符
                    if (i + 1 < contents.Length && isOperator(contents[i + 1]))
                    {
                        sb.Append(contents[++i]);
                        if (isOperator(sb.ToString()))
                        {
                            tokens.Add(new Token(TokenType.operation, sb.ToString(), 300 + operators.IndexOf(sb.ToString()), line));
                        }
                        //得到不存在的运算符，加入错误列表
                        else
                        {
                            wrongtokens.Add(new WrongToken(sb.ToString(), line, "invalid operator " + sb.ToString() + " found"));
                        }
                    }
                    tokens.Add(new Token(TokenType.operation, sb.ToString(), 300 + operators.IndexOf(sb.ToString()), line));
                }
                //不是上述的任何一种格式的单词
                else
                {
                    wrongtokens.Add(new WrongToken(contents[i].ToString(), line, "invalid word " + contents[i].ToString() + " found"));
                }
                    
            }
            
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
            return delimiters.Contains(ch.ToString());
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

        public string getTokensInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--------------------token表信息...--------------------\n");
            foreach (Token token in tokens)
            {
                sb.Append(token.toString());
                sb.Append("\n");
            }
            return sb.ToString();
        }

        public string getWrongTokensInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--------------------词法分析错误信息...--------------------\n");
            sb.Append("词法分析结束 - " + wrongtokens.Count + " error(s)\n");
            foreach (WrongToken token in wrongtokens)
            {
                sb.Append(token.toString());
                sb.Append("\n");
            }
            return sb.ToString();
        }

    }
}
