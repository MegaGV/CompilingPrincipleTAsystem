using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilingPrinciple
{
    class SentenceAnalyzer
    {
        private ExpressionAnalyzer expressionAnalyzer;
        private BoolExpressionAnalyzer boolexpressionAnalyzer;
        private AssignmentAnalyzer assignmentAnalyzer;
        private IfsAnalyzer ifsAnalyzer;
        private List<Token> tokens;

        public SentenceAnalyzer() { }

        public void setTokens(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public void expressionAnalyse()
        {
            expressionAnalyzer = new ExpressionAnalyzer(tokens);
            expressionAnalyzer.Analyse();
        }
        public string getExpressionWrongInfo()
        {
            return "--------------------表达式错误信息...--------------------\n" +
                "errors: " + expressionAnalyzer.getWrongCount() + '\n' + 
                expressionAnalyzer.getWrongInfo();
        }
        public string getExpressionArgsInfo()
        {
            return "--------------------表达式参数信息...--------------------\n" + 
                expressionAnalyzer.getArgsInfo();
        }

        public void boolExpressionAnalyse()
        {
            boolexpressionAnalyzer = new BoolExpressionAnalyzer(tokens);
            boolexpressionAnalyzer.Analyse();
        }
        public string getBoolExpressionWrongInfo()
        {
            return "--------------------布尔表达式错误信息...--------------------\n" +
                "errors: " + boolexpressionAnalyzer.getWrongCount() + '\n' +
                boolexpressionAnalyzer.getWrongInfo();
        }
        public string getBoolExpressionArgsInfo()
        {
            return "--------------------布尔表达式参数信息...--------------------\n" + 
                boolexpressionAnalyzer.getArgsInfo();
        }

        public void assignmentAnalyse()
        {
            assignmentAnalyzer = new AssignmentAnalyzer(tokens);
            assignmentAnalyzer.Analyse();
        }
        public string getAssignmentWrongInfo()
        {
            return "--------------------赋值语句错误信息...--------------------\n" +
                "errors: " + assignmentAnalyzer.getWrongCount() + '\n' +
                assignmentAnalyzer.getWrongInfo();
        }
        public string getAssignmentArgsInfo()
        {
            return "--------------------赋值语句参数信息...--------------------\n" + 
                assignmentAnalyzer.getArgsInfo();
        }

        public void ifsAnalyse()
        {
            ifsAnalyzer = new IfsAnalyzer(tokens);
            ifsAnalyzer.Analyse();
        }
        public string getIfsWrongInfo()
        {
            return "--------------------if语句错误信息...--------------------\n" +
                "errors: " + ifsAnalyzer.getWrongCount() + '\n' +
                ifsAnalyzer.getWrongInfo();
        }
        public string getIfsArgsInfo()
        {
            return "--------------------if语句参数信息...--------------------\n" + 
                ifsAnalyzer.getArgsInfo();
        }
    }

    class Analyzer
    {
        protected List<Token> tokens;
        protected Token token;
        protected int p = 0;
        protected List<string> wrongInfo;
        protected StringBuilder argsInfo;

        public Analyzer(List<Token> tokens)
        {
            this.tokens = tokens;
            if(tokens.Count != 0)
                token = tokens[p];
            wrongInfo = new List<string>();
            argsInfo = new StringBuilder();
        }

        public string getWrongInfo()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < wrongInfo.Count; i++)
                sb.Append(wrongInfo[i] + '\n');
            return sb.ToString();

        }

        public int getWrongCount()
        {
            return wrongInfo.Count;
        }

        public string getArgsInfo()
        {
            return argsInfo.ToString();
        }

        public void NextToken()
        {
            p++;
            if (p >= tokens.Count)
                token = new Token();
            else
                token = tokens[p];
        }

        public void Error(string str)
        {
            this.wrongInfo.Add(str);
        }

        public virtual void Analyse()
        {
            
        }
    }

    class ExpressionAnalyzer : Analyzer
    {
        public ExpressionAnalyzer(List<Token> tokens) : base(tokens) { }

        public override void Analyse()
        {
            /*
                E→TE’
                E’→+TE’ | -TE’
                T→FT’
                T’→*FT’ | / FT’
                F→(E) | i
            */
            E();
        }

        public void E()
        {
            //E→TE’
            T();
            E1();
        }

        public void E1()
        {
            //E’→+TE’ | -TE’
            if (token.getValue().Equals("+") || token.getValue().Equals("-"))
            {
                argsInfo.Append(token.getValue() + "\toperator\n");
                NextToken();
                T();
                E1();
            }
        }

        public void T()
        {
            //T→FT’
            F();
            T1();
        }

        public void T1()
        {
            //T’→*FT’ | / FT’
            if (token.getValue().Equals("*") || token.getValue().Equals("/"))
            {
                argsInfo.Append(token.getValue() + "\toperator\n");
                NextToken();
                F();
                T1();
            }
        }

        public void F()
        {
            //F→(E) | i
            if (token.getTokenTypeName().Equals("digit"))
            {
                argsInfo.Append(token.getValue() + "\tdigit\n");
                NextToken();
            }
            else if (token.getTokenTypeName().Equals("operation") || token.getValue().Equals("(") || token.getValue().Equals(")"))
            {
                if (token.getValue().Equals("("))
                {
                    argsInfo.Append(token.getValue() + "\tdelimiter\n");
                    NextToken();
                    E();
                    if (token.getValue().Equals(")"))
                    {
                        argsInfo.Append(token.getValue() + "\tdelimiter\n");
                        NextToken();
                    }
                    else
                    {
                        Error("brackets not fit");
                    }
                }
                else
                {
                    Error("wrong using of operator " + tokens[p].getValue() + " found");
                }
            }
            else
            {
                Error("invalid input " + token.getValue() + " found");
            }
        }
    }

    class BoolExpressionAnalyzer : Analyzer
    {
        public BoolExpressionAnalyzer(List<Token> tokens) : base(tokens) { }

        public override void Analyse()
        {
            /*
                BE -> BE or BT | BT
                BT -> BT and BF | BF
                BF -> not BF | (BE) | AE rop AE | i rop i | i
            */
            BE();
        }

        public void BE()
        {

        }
    }

    class AssignmentAnalyzer : Analyzer
    {
        private ExpressionAnalyzer expressionAnalyzer = new ExpressionAnalyzer(new List<Token>());
        public AssignmentAnalyzer(List<Token> tokens) : base(tokens) { }

        public override void Analyse()
        {
            /*
                S -> id = E
                E -> E’ + T | E’ - T |T
                T -> T’ * F | T’ / F | F
                F -> P | -P
                P -> i | (E)
            */
            S();
        }

        public void S()
        {
            if (token.getTokenTypeName().Equals("identifier"))
            {
                argsInfo.Append(token.getValue() + "\tidentifier\n");
                NextToken();
                if (token.getValue().Equals("="))
                {
                    argsInfo.Append(token.getValue() + "\toperator\n");
                    NextToken();
                    List<Token> expressiontoken = new List<Token>();
                    for (int i = p; i < tokens.Count; i++)
                        expressiontoken.Add(tokens[i]);
                    if (expressiontoken.Count == 0)
                    {
                        Error("No expression found after =");
                    }
                    else
                    {
                        expressionAnalyzer = new ExpressionAnalyzer(expressiontoken);
                        expressionAnalyzer.Analyse();
                    }
                }
                else
                {
                    Error("No assignment opeator found after identifier" + tokens[p - 1].getValue());
                }
            }
            else
            {
                Error("No identifier found at the begining");
            }
        }

        public new string getWrongInfo()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < wrongInfo.Count; i++)
                sb.Append(wrongInfo[i] + '\n');
            sb.Append(expressionAnalyzer.getWrongInfo());
            return sb.ToString();

        }

        public new int getWrongCount()
        {
            return wrongInfo.Count + expressionAnalyzer.getWrongCount();
        }

        public new string getArgsInfo()
        {
            return argsInfo.ToString() + expressionAnalyzer.getArgsInfo();
        }
    }

    class IfsAnalyzer : Analyzer
    {
        private BoolExpressionAnalyzer boolExpressionAnalyzer = new BoolExpressionAnalyzer(new List<Token>());

        public IfsAnalyzer(List<Token> tokens) : base(tokens) { }

        public override void Analyse()
        {
            /*
                S -> CS'
                C -> if E
                S -> TS''
                T -> CS' else
            */
            S();
        }

        public void S()
        {
            if (token.getValue().Equals("if"))
            {
                argsInfo.Append("if\tkeyword\n");
                NextToken();
                if (token.getValue().Equals("("))
                {
                    argsInfo.Append("(\tdelimiter\n");
                    List<Token> boolTokens = new List<Token>();
                    while (true) 
                    {
                        NextToken();
                        if (p == tokens.Count)
                        {
                            Error("Wrong boolexpression found");
                            break;
                        }
                        else if (token.getValue().Equals(")"))
                        {
                            boolExpressionAnalyzer = new BoolExpressionAnalyzer(boolTokens);
                            boolExpressionAnalyzer.Analyse();
                            argsInfo.Append(")\tdelimiter\n");
                            break;
                        }
                        else
                            boolTokens.Add(token);
                    }
                }
                else
                {
                    Error("No boolexpression found");
                }
            }
            else
            {
                Error("No 'if found' at the begining");
            }
        }



        public new string getWrongInfo()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < wrongInfo.Count; i++)
                sb.Append(wrongInfo[i] + '\n');
            sb.Append(boolExpressionAnalyzer.getWrongInfo());
            return sb.ToString();

        }

        public new int getWrongCount()
        {
            return wrongInfo.Count + boolExpressionAnalyzer.getWrongCount();
        }

        public new string getArgsInfo()
        {
            return argsInfo.ToString() + boolExpressionAnalyzer.getArgsInfo();
        }
    }
}
