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
        public SentenceAnalyzer()
        {

        }

        public void ExpressionAnalyse(string str)
        {
            expressionAnalyzer = new ExpressionAnalyzer(str);
            expressionAnalyzer.Analyse();
        }

        public string getExpressionWrongInfo()
        {
            return expressionAnalyzer.getWrongInfo();
        }

        public string getExpressionArgsInfo()
        {
            return expressionAnalyzer.getArgsInfo();
        }
    }

    class ExpressionAnalyzer
    {
        private string expression;
        private char sym;
        private int p = 0;
        private List<string> wrongInfo;
        private StringBuilder argsInfo;

        public ExpressionAnalyzer(string expression)
        {
            this.expression = expression;
            sym = this.expression[p];
            wrongInfo = new List<string>();
            argsInfo = new StringBuilder();
            argsInfo.Append("--------------------表达式参数信息...--------------------\n");
        }

        public string getWrongInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--------------------表达式错误信息...--------------------\n");
            sb.Append("errors: " + wrongInfo.Count + '\n');
            for (int i = 0; i < wrongInfo.Count; i++)
                sb.Append(wrongInfo[i] + '\n');
            return sb.ToString();
            
        }

        public string getArgsInfo()
        {
            return argsInfo.ToString();
        }

        public void NextToken()
        {
            if (char.IsDigit(expression[p]))
            {
                StringBuilder digit = new StringBuilder();
                while (p < expression.Length && char.IsDigit(expression[p]))
                {
                    digit.Append(expression[p]);
                    p++;
                }
                argsInfo.Append(digit.ToString() + "\tdigit\n");
                if (p >= expression.Length)
                {
                    return;
                }
            }
            else
            {
                argsInfo.Append(expression[p].ToString() + "\toperator\n");
                p++;
                if (p >= expression.Length)
                {
                    Error("No arg found after operator " + expression[p-1]);
                    sym = ' ';
                    return;
                }
            }
            sym = expression[p];
        }

        public void Error(string str)
        {
            this.wrongInfo.Add(str);
        }

        public void Analyse()
        {
            E();
        }

        public void E()
        {
            //E→TE’
            E1();
        }

        public void E1()
        {
            //E’→+TE’ | -TE’
            if (sym == '+' || sym == '-')
            {
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
            if (sym == '*' || sym == '/')
            {
                NextToken();
                F();
                T1();
            }
        }

        public void F()
        {
            //F→(E) | i
            if (char.IsDigit(sym))
            {
                NextToken();
            }
            else
            {
                if (sym == '(')
                {
                    NextToken();
                    E();
                    if (sym == ')')
                    {
                        NextToken();
                    }
                    else
                    {
                        Error("brackets not fit");
                    }
                }
                else
                {
                    Error("invalid input " + sym + " found");
                }
            }
        }
    }
}
