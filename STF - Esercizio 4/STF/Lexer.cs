using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace STF
{
    [Flags]
    public enum TokenType
    {
        TableBegin = 1,  TableEnd = 2,
        RowBegin   = 4,  RowEnd = 8,
        FieldBegin = 16, FieldEnd = 32,
        Identifier = 64, Number = 128,
        Unrecognized = 256
    }

    public class Token
    {
        public readonly TokenType Type;
        public readonly string Attribute;

        public Token(TokenType name, string attribute)
        {
            this.Type = name;
            this.Attribute = attribute;
        }
    }

    public class Lexer
    {
        private Regex RETableBegin  = new Regex("<table>"   , RegexOptions.IgnoreCase);
        private Regex RETableEnd    = new Regex("</table>"  , RegexOptions.IgnoreCase);
        private Regex RERowBegin    = new Regex("<tr>"      , RegexOptions.IgnoreCase);
        private Regex RERowEnd      = new Regex("</tr>"     , RegexOptions.IgnoreCase);
        private Regex REFieldBegin  = new Regex("<td>"      , RegexOptions.IgnoreCase);
        private Regex REFieldEnd    = new Regex("</td>"     , RegexOptions.IgnoreCase);
        private Regex REIdentifier  = new Regex("^@?[a-zA-Z_]+(\\(\\))?$");
        private Regex RENumber      = new Regex("^[-+]?[0-9]+(\\.[0-9]+)?$|^[-+]?\\.[0-9]+$");
        // Regex non esaustive! non supportano Ddf e notazione esponenziale
        // Es. identifier non esclude C# keywords, ad esempio. ma prende result()

        public List<Token> Tokenize(string fixtureHTML)
        {
            // fixtureHTML = fixtureHTML.Replace(".", ","); // CultureInvariant
            fixtureHTML = fixtureHTML.Replace("<", " <").Replace(">", "> ");
            
            string[] lexemes = fixtureHTML.Split(new char[] { ' ', '\n', '\r', '\t' },
                StringSplitOptions.RemoveEmptyEntries);
            
            List<Token> tokens = new List<Token>();

            foreach (string l in lexemes)
            {
                TokenType currentTokenType;
                string attribute = string.Empty;

                if (RETableBegin.Match(l).Success)      currentTokenType = TokenType.TableBegin;
                else if (RETableEnd.Match(l).Success)   currentTokenType = TokenType.TableEnd;
                else if (RERowBegin.Match(l).Success)   currentTokenType = TokenType.RowBegin;
                else if (RERowEnd.Match(l).Success)     currentTokenType = TokenType.RowEnd;
                else if (REFieldBegin.Match(l).Success) currentTokenType = TokenType.FieldBegin;
                else if (REFieldEnd.Match(l).Success)   currentTokenType = TokenType.FieldEnd;
                else if (RENumber.Match(l).Success)     { currentTokenType = TokenType.Number; attribute = l; }
                else if (REIdentifier.Match(l).Success) { currentTokenType = TokenType.Identifier; attribute = l; }
                else                                    { currentTokenType = TokenType.Unrecognized; }
                
                tokens.Add(new Token(currentTokenType, attribute));
            }

            return tokens;
        }
    }

}
