using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpLexer
{
    public class Tokenizer
    {
        private const int IDENTIFIRY_MAX_LENGTH = 300;
        private const int MAX_INTEGER_NUM_LENGTH = 10;
        private const int MAX_DOUBLE_NUM_PRECISION = 17;

        private List<Token> _result;

        private readonly Dictionary<TokenKind, string> _tokenKindToValue = new Dictionary<TokenKind, string>()
        {
            { TokenKind.ACCESS_LEVEL_INTERNAL, "internal" },
            { TokenKind.ACCESS_LEVEL_PRIVATE, "private" },
            { TokenKind.ACCESS_LEVEL_PUBLIC, "public" },
            { TokenKind.ASSIGNMENT, "=" },
            { TokenKind.BLOCK_COMMENT_END, "*/" },
            { TokenKind.BLOCK_COMMENT_START, "/*" },
            { TokenKind.BOOL, "boolean" },
            { TokenKind.BOOLFALSE, "false" },
            { TokenKind.BOOLTRUE, "true" },
            { TokenKind.CHAR, "char" },
            { TokenKind.CLASS, "class" },
            { TokenKind.COLON, ":" },
            { TokenKind.COMMA, "," },
            { TokenKind.COMPARISON, "==" },
            { TokenKind.CONST, "const" },
            { TokenKind.DIVISION, "/" },
            { TokenKind.DOUBLE, "double" },
            { TokenKind.ELSE, "else" },
            { TokenKind.FIGURE_BRACKET_CLOSE, "}" },
            { TokenKind.FIGURE_BRACKET_OPEN, "{" },
            { TokenKind.FLOAT, "float" },
            { TokenKind.IF, "if" },
            { TokenKind.INTEGER, "int" },
            { TokenKind.LESS, "<" },
            { TokenKind.LESS_OR_EQUAL, "<=" },
            { TokenKind.SUB, "-" },
            { TokenKind.MORE, ">" },
            { TokenKind.MORE_OR_EQUAL, ">=" },
            { TokenKind.MULTIPLY, "*" },
            { TokenKind.NOT_EQUAL, "!=" },
            { TokenKind.NOT, "!" },
            { TokenKind.ONE_LINE_COMMENT, "//" },
            { TokenKind.SUM, "+" },
            { TokenKind.POINT, "." },
            { TokenKind.ROUND_BRACKET_CLOSE, ")" },
            { TokenKind.ROUND_BRACKET_OPEN, "(" },
            { TokenKind.SEMICOLON, ";" },
            { TokenKind.SPACE, " " },
            { TokenKind.TRIANGULAR_BRACKET_CLOSE, ">" },
            { TokenKind.TRIANGULAR_BRACKET_OPEN, "<" },
            { TokenKind.VOID, "void" },
            { TokenKind.WHILE, "while" }
        };

        public Tokenizer()
        {
            _result = new List<Token>();
        }

        public Tokenizer Tokenize( List<string> lines )
        {
            int lineNumber = 1;
            int columnNumber = 0;

            if ( lines.Count == 0 )
            {
                _result.Add( new Token( TokenKind.END_FILE, String.Empty, lineNumber, columnNumber ) );
                return this;
            }

            for ( int i = 0; i < lines.Count; i++ )
            {
                string line = lines[i];

                columnNumber = 0;

                TokenKind tokenType = TokenKind.END_FILE;
                string token = String.Empty;

                for ( int startIndex = 0; startIndex < line.Length; startIndex++ )
                {
                    columnNumber = startIndex;
                    switch ( line[startIndex] )
                    {
                        case '.':
                            tokenType = TokenKind.POINT;
                            token = _tokenKindToValue[TokenKind.POINT];
                            break;
                        case ',':
                            tokenType = TokenKind.COMMA;
                            token = _tokenKindToValue[TokenKind.COMMA];
                            break;
                        case '>':
                            if ( startIndex + 1 < line.Length )
                            {
                                if ( line[startIndex + 1] == '=' )
                                {
                                    tokenType = TokenKind.MORE_OR_EQUAL;
                                    token = _tokenKindToValue[TokenKind.MORE_OR_EQUAL];
                                    startIndex++;
                                    break;
                                }
                            }
                            tokenType = TokenKind.MORE;
                            token = _tokenKindToValue[TokenKind.MORE];
                            break;
                        case '<':
                            if ( startIndex + 1 < line.Length )
                            {
                                if ( line[startIndex + 1] == '=' )
                                {
                                    tokenType = TokenKind.LESS_OR_EQUAL;
                                    token = _tokenKindToValue[TokenKind.LESS_OR_EQUAL];
                                    startIndex++;
                                    break;
                                }
                            }
                            tokenType = TokenKind.LESS;
                            token = _tokenKindToValue[TokenKind.LESS];
                            break;
                        case ';':
                            tokenType = TokenKind.SEMICOLON;
                            token = _tokenKindToValue[TokenKind.SEMICOLON];
                            break;
                        case ':':
                            tokenType = TokenKind.COLON;
                            token = _tokenKindToValue[TokenKind.COLON];
                            break;
                        case '(':
                            tokenType = TokenKind.ROUND_BRACKET_OPEN;
                            token = _tokenKindToValue[TokenKind.ROUND_BRACKET_OPEN];
                            break;
                        case ')':
                            tokenType = TokenKind.ROUND_BRACKET_CLOSE;
                            token = _tokenKindToValue[TokenKind.ROUND_BRACKET_CLOSE];
                            break;
                        case '{':
                            tokenType = TokenKind.FIGURE_BRACKET_OPEN;
                            token = _tokenKindToValue[TokenKind.FIGURE_BRACKET_OPEN];
                            break;
                        case '}':
                            tokenType = TokenKind.FIGURE_BRACKET_CLOSE;
                            token = _tokenKindToValue[TokenKind.FIGURE_BRACKET_CLOSE];
                            break;
                        case '+':
                            tokenType = TokenKind.SUM;
                            token = _tokenKindToValue[TokenKind.SUM];
                            break;
                        case '-':
                            tokenType = TokenKind.SUB;
                            token = _tokenKindToValue[TokenKind.SUB];
                            break;
                        case '*':
                            if ( ( startIndex + 1 < line.Length ) && ( line[startIndex + 1] == '/' ) )
                            {
                                    tokenType = TokenKind.BLOCK_COMMENT_END;
                                    token = _tokenKindToValue[TokenKind.BLOCK_COMMENT_END];
                                    startIndex++;
                                    break;
                            }
                            tokenType = TokenKind.MULTIPLY;
                            token = _tokenKindToValue[TokenKind.MULTIPLY];
                            break;
                        case '/':
                            if ( startIndex + 1 >= line.Length )
                            {
                                break;
                            }
                            if ( line[startIndex + 1] == '/' )
                            {
                                tokenType = TokenKind.ONE_LINE_COMMENT;
                                token = _tokenKindToValue[TokenKind.ONE_LINE_COMMENT];
                                startIndex = line.Length;
                                startIndex++;
                                break;
                            }
                            if ( line[startIndex + 1] == '*' )
                            {
                                tokenType = TokenKind.BLOCK_COMMENT_START;
                                token = _tokenKindToValue[TokenKind.BLOCK_COMMENT_START];
                                startIndex++;
                                break;
                            }
                            if ( line[startIndex - 1] == '*' )
                            {
                                tokenType = TokenKind.DIVISION;
                                token = _tokenKindToValue[TokenKind.DIVISION];

                            }
                            break;
                        case ' ':
                            tokenType = TokenKind.SPACE;
                            token = _tokenKindToValue[TokenKind.SPACE];
                            break;
                        case '!':
                            if ( ( startIndex + 1 < line.Length ) && ( line[startIndex + 1] == '=' ) )
                            {
                                tokenType = TokenKind.NOT_EQUAL;
                                token = _tokenKindToValue[TokenKind.NOT_EQUAL];
                                startIndex++;
                                break;
                            }
                            tokenType = TokenKind.NOT;
                            token = _tokenKindToValue[TokenKind.NOT];
                            break;
                        case '=':
                            if ( ( startIndex + 1 < line.Length ) && ( line[startIndex + 1] == '=' ) )
                            {
                                tokenType = TokenKind.COMPARISON;
                                token = _tokenKindToValue[TokenKind.COMPARISON];
                                startIndex++;
                                break;
                            }
                            tokenType = TokenKind.ASSIGNMENT;
                            token = _tokenKindToValue[TokenKind.ASSIGNMENT];
                            break;
                        case '"':
                            tokenType = TokenKind.STRING_CONST;
                            token = "";
                            break;
                        default:
                            if ( Char.IsLetter( line[startIndex] ) || line[startIndex] == '_' )
                            {
                                string id = String.Empty;
                                while ( Char.IsLetterOrDigit( line[startIndex] ) || line[startIndex] == '_' )
                                {
                                    id += line[startIndex];
                                    startIndex++;

                                    if ( startIndex >= line.Length )
                                    {
                                        break;
                                    }
                                }

                                token = id;
                                switch ( token )
                                {
                                    case "true":
                                        tokenType = TokenKind.BOOLTRUE;
                                        break;
                                    case "false":
                                        tokenType = TokenKind.BOOLFALSE;
                                        break;
                                    case "class":
                                        tokenType = TokenKind.CLASS;
                                        break;
                                    case "const":
                                        tokenType = TokenKind.CONST;
                                        break;
                                    case "public":
                                        tokenType = TokenKind.ACCESS_LEVEL_PUBLIC;
                                        break;
                                    case "private":
                                        tokenType = TokenKind.ACCESS_LEVEL_PRIVATE;
                                        break;
                                    case "internal":
                                        tokenType = TokenKind.ACCESS_LEVEL_INTERNAL;
                                        break;
                                    case "boolean":
                                        tokenType = TokenKind.BOOL;
                                        break;
                                    case "string":
                                        tokenType = TokenKind.STRING;
                                        break;
                                    case "char":
                                        tokenType = TokenKind.CHAR;
                                        break;
                                    case "double":
                                        tokenType = TokenKind.DOUBLE;
                                        break;
                                    case "float":
                                        tokenType = TokenKind.FLOAT;
                                        break;
                                    case "while":
                                        tokenType = TokenKind.WHILE;
                                        break;
                                    case "void":
                                        tokenType = TokenKind.VOID;
                                        break;
                                    case "if":
                                        tokenType = TokenKind.IF;
                                        break;
                                    case "else":
                                        tokenType = TokenKind.ELSE;
                                        break;
                                    case "int":
                                        tokenType = TokenKind.INTEGER;
                                        break;
                                    default:
                                        if ( id.Length <= IDENTIFIRY_MAX_LENGTH )
                                        {
                                            tokenType = TokenKind.ID;
                                        }
                                        else
                                        {
                                            tokenType = TokenKind.ERROR;
                                        }
                                        break;
                                }
                            }
                            else if ( Char.IsDigit( line[startIndex] ) )
                            {

                                string num = string.Empty;

                                while ( Char.IsDigit( line[startIndex] ) || line[startIndex] == '.' )
                                {
                                    num += line[startIndex];
                                    startIndex++;
                                    if ( startIndex == line.Length )
                                    {
                                        break;
                                    }
                                }

                                if ( num.Length == 1 && num == "0" )
                                {
                                    tokenType = TokenKind.INTEGER_NUM;
                                }
                                else if ( NumIsDouble( num ) )
                                {
                                    tokenType = TokenKind.DOUBLE_NUM;
                                }
                                else if ( NumIsInt( num ) )
                                {
                                    tokenType = TokenKind.INTEGER_NUM;
                                }
                                else
                                {
                                    tokenType = TokenKind.ERROR;
                                }
                            }
                            else
                            {
                                tokenType = TokenKind.ERROR;
                                token = line[startIndex].ToString();
                            }
                            break;
                    }

                    if ( tokenType == TokenKind.STRING_CONST )
                    {
                        int lineNimberStart = lineNumber;
                        startIndex++;
                        int stringConstStart = startIndex;

                        int stringConstEnd = -1;
                        while ( stringConstEnd == -1 )
                        {
                            stringConstEnd = line.IndexOf( '\"', startIndex );
                            if ( stringConstEnd == -1 )
                            {
                                for ( int j = startIndex; j < line.Length; j++ )
                                {
                                    token += line[j];
                                }
                                if ( ( i + 1 ) == lines.Count )
                                {
                                    tokenType = TokenKind.ERROR;
                                    token = '\"'.ToString();
                                    break;
                                }
                                line = lines[i + 1];
                                startIndex = 0;
                                i++;
                                lineNumber++;
                            }
                            else
                            {
                                for ( int j = startIndex; j < stringConstEnd; j++ )
                                {
                                    token += line[j];
                                }
                                startIndex = stringConstEnd;
                            }
                        }

                        _result.Add( new Token( tokenType, token, lineNimberStart, columnNumber ) );
                    }
                    else
                    {
                        _result.Add( new Token( tokenType, token, lineNumber, columnNumber ) );
                        if ( startIndex < line.Length )
                        {
                            if ( line[startIndex] == '<' )
                            {
                                _result.Add( new Token( TokenKind.TRIANGULAR_BRACKET_OPEN, _tokenKindToValue[TokenKind.TRIANGULAR_BRACKET_OPEN], lineNumber, columnNumber ) );
                            }
                            if ( line[startIndex] == '>' )
                            {
                                _result.Add( new Token( TokenKind.TRIANGULAR_BRACKET_CLOSE, _tokenKindToValue[TokenKind.TRIANGULAR_BRACKET_CLOSE], lineNumber, startIndex ) );
                            }
                            if ( line[startIndex] == ',' )
                            {
                                _result.Add( new Token( TokenKind.COMMA, _tokenKindToValue[TokenKind.COMMA], lineNumber, startIndex ) );
                            }
                        }
                    }
                }
                lineNumber++;
            }

            //delete comments
            while ( true )
            {
                int blockCommentStartIndex = _result.FindIndex( p => p.Kind == TokenKind.BLOCK_COMMENT_START );
                int blockCommentEndIndex = _result.FindIndex( p => p.Kind == TokenKind.BLOCK_COMMENT_END );
                if ( blockCommentStartIndex == -1 && blockCommentEndIndex == -1 )
                {
                    break;
                }
                if ( blockCommentEndIndex == -1 )
                {
                    Token errorToken = new Token( TokenKind.ERROR, String.Empty, _result[blockCommentStartIndex].Line, _result[blockCommentStartIndex].Column );
                    _result.RemoveRange( blockCommentStartIndex, _result.Count - 1 - blockCommentStartIndex );
                    _result.Add( errorToken );
                    break;
                }
                else
                {
                    _result.RemoveRange( blockCommentStartIndex, blockCommentEndIndex - blockCommentStartIndex );
                    break;
                }
            }

            return this;
        }

        public List<Token> GetResult()
        {
            return _result.Where( t => t.Kind != TokenKind.SPACE 
                && t.Kind != TokenKind.ONE_LINE_COMMENT
                && t.Kind != TokenKind.BLOCK_COMMENT_END 
                && t.Kind != TokenKind.BLOCK_COMMENT_START ).ToList();
        }

        private static bool NumIsInt( string num )
        {
            if ( num.Length > MAX_INTEGER_NUM_LENGTH || num[0] == '0' )
            {
                return false;
            }

            foreach ( char ch in num )
            {
                if ( !Char.IsDigit( ch ) )
                {
                    return false;
                }
            }

            return true;
        }

        private static bool NumIsDouble( string num )
        {
            if ( !num.Contains( "." ) )
            {
                return false;
            }

            if ( num.Length - num.IndexOf( '.' ) > MAX_DOUBLE_NUM_PRECISION )
            {
                return false;
            }

            for ( int i = 0; i < num.Length; i++ )
            {
                if ( i != num.IndexOf( '.' ) )
                {
                    if ( !Char.IsDigit( num[i] ) )
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}