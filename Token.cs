using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sci_cal
{
    public enum TokenType
    {
        Number,
        Operator,
        LeftParenthesis,
        RightParenthesis,
        Function
    }

    public class Token
    {
        public TokenType Type { get; set; }

        public string Value { get; set; }


        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
