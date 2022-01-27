using System;
using System.Collections.Generic;
using System.IO;

namespace CSharpLexer
{
    public class Token
    {
        private TokenKind _kind;
        private string _value;
        private int _line;
        private int _column;

        public Token(TokenKind type, string token, int line, int column)
        {
            _value = token;
            _kind = type;
            _line = line;
            _column = column;
        }
            
        public override string ToString()
        {
            return $"TokenKind: <{_kind}> | Value: <'{_value}'> | Line: <{_line}>, Col: <{_column}>";
        }

        public TokenKind Kind => _kind;
        public int Line => _line;
        public int Column => _column;
    }
}