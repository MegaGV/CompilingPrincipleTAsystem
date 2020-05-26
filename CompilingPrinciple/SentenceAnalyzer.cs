using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace CompilingPrinciple
{
    class SentenceAnalyzer
    {
        private ExpressionAnalyzer expressionAnalyzer;
        private BoolExpressionAnalyzer boolexpressionAnalyzer;
        private AssignmentAnalyzer assignmentAnalyzer;
        private IfsAnalyzer ifsAnalyzer;
        private ForAnalyzer forAnalyzer;
        private WhileAnalyzer whileAnalyzer;
        private DoWhileAnalyzer doWhileAnalyzer;
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

        public void forAnalyse()
        {
            forAnalyzer = new ForAnalyzer(tokens);
            forAnalyzer.Analyse();
        }
        public string getForWrongInfo()
        {
            return "--------------------for语句错误信息...--------------------\n" +
                "errors: " + forAnalyzer.getWrongCount() + '\n' +
                forAnalyzer.getWrongInfo();
        }
        public string getForArgsInfo()
        {
            return "--------------------for语句参数信息...--------------------\n" +
                forAnalyzer.getArgsInfo();
        }

        public void whileAnalyse()
        {
            whileAnalyzer = new WhileAnalyzer (tokens);
            whileAnalyzer.Analyse();
        }
        public string getWhileWrongInfo()
        {
            return "--------------------while语句错误信息...--------------------\n" +
                "errors: " + whileAnalyzer.getWrongCount() + '\n' +
                whileAnalyzer.getWrongInfo();
        }
        public string getWhileArgsInfo()
        {
            return "--------------------while语句参数信息...--------------------\n" +
                whileAnalyzer.getArgsInfo();
        }

        public void doWhileAnalyse()
        {
            doWhileAnalyzer = new DoWhileAnalyzer(tokens);
            doWhileAnalyzer.Analyse();
        }
        public string getDoWhileWrongInfo()
        {
            return "--------------------dowhile语句错误信息...--------------------\n" +
                "errors: " + doWhileAnalyzer.getWrongCount() + '\n' +
                doWhileAnalyzer.getWrongInfo();
        }
        public string getDoWhileArgsInfo()
        {
            return "--------------------dowhile语句参数信息...--------------------\n" +
                doWhileAnalyzer.getArgsInfo();
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

        protected void NextToken()
        {
            p++;
            if (p >= tokens.Count)
                token = new Token();
            else
                token = tokens[p];
        }

        protected void Error(string str)
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
                E’→+TE’ | -TE’ | ε
                T→FT’
                T’→*FT’ | / FT’ | ε
                F→(E) | i
            */
            E();
        }

        private void E()
        {
            //E→TE’
            T();
            E1();
        }

        private void E1()
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

        private void T()
        {
            //T→FT’
            F();
            T1();
        }

        private void T1()
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

        private void F()
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
             * BE -> BT BE'
             * BE'-> || BE |  ε
             * BT -> BF BT'
             * BT'-> && BT | ε
             * BF -> ! BF | (BE) | AE rop AE | i BF'
             * BF'-> rop i | ε
             */
            BE();
        }

        private void BE()
        {
            //BE -> BT BE'
            BT();
            BE1();
        }

        private void BE1()
        {
            //BE'-> || BE |  ε
            if (token.getValue().Equals("||"))
            {
                argsInfo.Append("||\toperator\n");
                NextToken();
                BE();
            }
        }

        private void BT()
        {
            //BT -> BF BT'
            BF();
            BT1();
        }

        private void BT1()
        {
            //BT'-> && BT | ε
            if (token.getValue().Equals("&&"))
            {
                argsInfo.Append("&&\toperator\n");
                NextToken();
                BT();
            }
        }

        private void BF()
        {
            //BF -> ! BF | (BE) | AE rop AE | i BF'
            string value = token.getValue();
            string type = token.getTokenTypeName();
            if (value.Equals("!"))
            {
                argsInfo.Append("!\toperator\n");
                NextToken();
                BF();
            }
            else if (value.Equals("("))
            {
                argsInfo.Append("(\tdelimeter\n");
                NextToken();
                BE();
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
            else if (type.Equals("digit"))
            {
                argsInfo.Append(token.getValue() + "\tdigit\n");
                NextToken();
                string rop = token.getValue();
                if (rop == ">" || rop == "<" || rop == ">=" || rop == "<=" || rop == "==")
                {
                    argsInfo.Append(rop + "\toperator\n");
                    NextToken();
                    if (token.getTokenTypeName().Equals("digit"))
                    {
                        argsInfo.Append(token.getValue() + "\tdigit\n");
                        NextToken();
                    }
                    else
                        Error("No fit digit found after " + rop);
                }
                else
                    Error("No operator found after digit");
            }
            else if (type.Equals("identifier"))
            {
                argsInfo.Append(token.getValue() + "\tidentifier\n");
                NextToken();
                BF1();
            }
            else
            {
                Error("invalid input " + token.getValue() + " found");
            }
        }

        private void BF1()
        {
            //BF'-> rop i | ε
            string rop = token.getValue();
            if (rop == ">" || rop == "<" || rop == ">=" || rop == "<=" || rop == "==")
            {
                argsInfo.Append(rop + "\toperator\n");
                NextToken();
                if (token.getTokenTypeName().Equals("identifier"))
                {
                    argsInfo.Append(token.getValue() + "\tidentifier\n");
                    NextToken();
                }
                else
                    Error("No fit identifier found after " + rop);
            }
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

        private void S()
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

        private new string getWrongInfo()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < wrongInfo.Count; i++)
                sb.Append(wrongInfo[i] + '\n');
            sb.Append(expressionAnalyzer.getWrongInfo());
            return sb.ToString();

        }

        private new int getWrongCount()
        {
            return wrongInfo.Count + expressionAnalyzer.getWrongCount();
        }

        private new string getArgsInfo()
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

        private void S()
        {
            if (token.getValue().Equals("if"))
            {
                argsInfo.Append("if\tkeyword\n");
                NextToken();
                if (token.getValue().Equals("("))
                {
                    E();
                    if (token.getValue().Equals("{"))
                    {
                        C();
                        if (p < tokens.Count) 
                        {
                            if (token.getValue().Equals("else"))
                            {
                                argsInfo.Append("else\tkeyword\n");
                                NextToken();
                                if (token.getValue().Equals("{"))
                                {
                                    C();
                                    if (p > tokens.Count)
                                        Error("Over function body part");
                                }
                                else
                                    Error("Wrong function body found");
                            }
                            else
                                Error("No 'else' found at the second part");
                        }
                    }
                    else
                        Error("Wrong function body found");
                }
                else
                    Error("No boolexpression found");
            }
            else
                Error("No 'if' found at the begining");
        }

        private void E()
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
                    NextToken();
                    break;
                }
                else
                    boolTokens.Add(token);
            }
        }

        private void C()
        {
            argsInfo.Append("{\tdelimeter\n");
            while (true)
            {
                NextToken();
                if (p == tokens.Count)
                {
                    Error("Wrong function body found");
                    break;
                }
                else if (token.getValue().Equals("}"))
                {
                    argsInfo.Append("}\tdelimeter\n");
                    NextToken();
                    break;
                }
                else
                    argsInfo.Append(token.getValue() + "\t" + token.getTokenTypeName() + "\n");
            }
        }


        private new string getWrongInfo()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < wrongInfo.Count; i++)
                sb.Append(wrongInfo[i] + '\n');
            sb.Append(boolExpressionAnalyzer.getWrongInfo());
            return sb.ToString();

        }

        private new int getWrongCount()
        {
            return wrongInfo.Count + boolExpressionAnalyzer.getWrongCount();
        }

        private new string getArgsInfo()
        {
            return argsInfo.ToString() + boolExpressionAnalyzer.getArgsInfo();
        }
    }

    class ForAnalyzer : Analyzer
    {
        public ForAnalyzer(List<Token> tokens) : base(tokens) { }

        public override void Analyse()
        {
            S();
        }

        private void S()
        {
            //S -> for (E', E'', E''') S'
            if (token.getValue().Equals("for")){
                argsInfo.Append("for\tkeyword\n");
                NextToken();
                if (token.getValue().Equals("("))
                {
                    argsInfo.Append("(\tdelimeter\n");
                    NextToken();
                    E1();
                    E2();
                    E3();
                    if (token.getValue().Equals(")"))
                    {
                        argsInfo.Append(")\tdelimeter\n");
                        NextToken();
                        S1();
                    }
                    else
                        Error("brackets not fit");
                }
                else
                    Error("Wrong format which has no '('");
            }
            else
                Error("No 'for' found at the begining");
        }

        private void E1()
        {
            while (true)
            {
                if (p >= tokens.Count)
                {
                    Error("Wrong format for for's()");
                    break;
                }
                argsInfo.Append(token.getValue() + "\t" + token.getTokenTypeName() +"\n");
                NextToken();
                if (token.getValue().Equals(";"))
                {
                    break;
                }
            }
        }

        private void E2()
        {
            while (true)
            {
                if (p >= tokens.Count)
                {
                    Error("Wrong format for for's()");
                    break;
                }
                argsInfo.Append(token.getValue() + "\t" + token.getTokenTypeName() + "\n");
                NextToken();
                if (token.getValue().Equals(";"))
                {
                    break;
                }
            }
        }

        private void E3()
        {
            while (true)
            {
                if (p >= tokens.Count)
                {
                    Error("Wrong format for for's()");
                    break;
                }
                argsInfo.Append(token.getValue() + "\t" + token.getTokenTypeName() + "\n");
                NextToken();
                if (token.getValue().Equals(")"))
                {
                    break;
                }
            }
        }

        private void S1() 
        { 
            if (token.getValue().Equals("{"))
            {
                while (true)
                {
                    if (p >= tokens.Count)
                    {
                        Error("Over function body part");
                        break;
                    }
                    argsInfo.Append(token.getValue() + "\t" + token.getTokenTypeName() + "\n");
                    NextToken();
                    if (token.getValue().Equals("}"))
                    {
                        break;
                    }
                }
            }
            else
                Error("Wrong function body found");
        }
    }

    class WhileAnalyzer : Analyzer
    {
        public WhileAnalyzer(List<Token> tokens) : base(tokens) { }

        public override void Analyse()
        {
            S();
        }

        private void S()
        {
            //S -> while(E) S’
            if (token.getValue().Equals("while"))
            {
                argsInfo.Append("while\tkeyword\n");
                NextToken();
                if (token.getValue().Equals("("))
                {
                    argsInfo.Append("(\tdelimeter\n");
                    NextToken();
                    E();
                    if (token.getValue().Equals(")"))
                    {
                        argsInfo.Append(")\tdelimeter\n");
                        NextToken();
                        S1();
                    }
                    else
                        Error("Wrong format for while's()");
                }
                else
                    Error("Wrong format for while's()");
            }
            else
                Error("No 'while' found at the begining");
        }

        private void E()
        {
            while (true)
            {
                if (p >= tokens.Count)
                {
                    Error("Wrong format for while's()");
                    break;
                }
                argsInfo.Append(token.getValue() + "\t" + token.getTokenTypeName() + "\n");
                NextToken();
                if (token.getValue().Equals(")"))
                {
                    break;
                }
            }
        }

        private void S1()
        {
            if (token.getValue().Equals("{"))
            {
                argsInfo.Append("{\tdelimeter\n");
                NextToken();
                while (true)
                {
                    if (p >= tokens.Count)
                    {
                        Error("Over function body part");
                        break;
                    }
                    argsInfo.Append(token.getValue() + "\t" + token.getTokenTypeName() + "\n");
                    NextToken();
                    if (token.getValue().Equals("}"))
                    {
                        argsInfo.Append("}\tdelimeter\n");
                        NextToken();
                        break;
                    }
                }
            }
            else
                Error("Wrong function body found");
        }
    }

    class DoWhileAnalyzer : Analyzer
    {
        public DoWhileAnalyzer(List<Token> tokens) : base(tokens) { }

        public override void Analyse()
        {
            S();
        }

        private void S()
        {
            //S -> do S’ while E
            if (token.getValue().Equals("do"))
            {
                argsInfo.Append("do\tkeyword\n");
                NextToken();
                S1();
                if (token.getValue().Equals("while"))
                {
                    argsInfo.Append("while\tkeyword\n");
                    NextToken();
                    if (token.getValue().Equals("("))
                    {
                        argsInfo.Append("(\tdelimeter\n");
                        NextToken();
                        E();
                        if (token.getValue().Equals(")"))
                        {
                            argsInfo.Append(")\tdelimeter\n");
                            NextToken();
                            if (token.getValue().Equals(";"))
                            {
                                argsInfo.Append(";\tdelimeter\n");
                            }
                            else
                                Error("No ';' found at the end");
                        }
                        else
                            Error("Wrong format for while's()");
                    }
                    else
                        Error("Wrong format for while's()");
                }
                else
                    Error("No 'while' found after function");
            }
            else
                Error("No 'do' found at the begining");
        }

        private void S1()
        {
            if (token.getValue().Equals("{"))
            {
                argsInfo.Append("{\tdelimeter\n");
                NextToken();
                while (true)
                {
                    if (p >= tokens.Count)
                    {
                        Error("Over function body part");
                        break;
                    }
                    argsInfo.Append(token.getValue() + "\t" + token.getTokenTypeName() + "\n");
                    NextToken();
                    if (token.getValue().Equals("}"))
                    {
                        argsInfo.Append("}\tdelimeter\n");
                        NextToken();
                        break;
                    }
                }
            }
            else
                Error("Wrong function body found");
        }

        private void E()
        {
            while (true)
            {
                if (p >= tokens.Count)
                {
                    Error("Wrong format for while's()");
                    break;
                }
                argsInfo.Append(token.getValue() + "\t" + token.getTokenTypeName() + "\n");
                NextToken();
                if (token.getValue().Equals(")"))
                {
                    break;
                }
            }
        }
    }
}
