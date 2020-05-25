using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CompilingPrinciple
{
    enum TokenType
    {
        identifier, digit, keyword, operation, delimiter,
    }
    class Token
    {
        private TokenType tokenType;
        private int typeCode;
        private string value;
        private int line;

        public Token(TokenType tokenType, string value, int typeCode, int line)
        {
            this.tokenType = tokenType;
            this.value = value;
            this.typeCode = typeCode;
            this.line = line;
        }

        public Token()
        {
            tokenType = TokenType.delimiter;
            value = " ";
            typeCode = 409;
            this.line = -1;
        }

        public string getTokenTypeName()
        {
            switch (tokenType)
            {
                case TokenType.identifier: return "identifier";
                case TokenType.digit: return "digit";
                case TokenType.keyword: return "keyword";
                case TokenType.operation: return "operation";
                case TokenType.delimiter: return "delimiter";
                default: return "unknown Tokentype";
            }
        }

        public string toString()
        {
            return line + ":\t" +  getTokenTypeName() +  "\t" + value + "\t" + typeCode;
        }

        public string getValue()
        {
            return this.value;
        }

        public int getTypeCode()
        {
            return this.typeCode;
        }

        public int getLine()
        {
            return this.line;
        }

    }

    class WrongToken
    {
        private string value;
        private int line;
        private string describe;

        public WrongToken(string value, int line, string describe)
        {
            this.value = value;
            this.line = line;
            this.describe = describe;
        }

        public string toString()
        {
            return line + ":\t" + value + "\t" + describe;
        }

        public string getValue()
        {
            return this.value;
        }

        public int getLine()
        {
            return this.line;
        }

        public string getDescribe()
        {
            return this.describe;
        }
    }
}
