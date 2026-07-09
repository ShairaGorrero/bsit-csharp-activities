using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace sci_cal
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        private string expression = "";
        private bool isDrawing = false;
        private Point lastPoint;
        public Form1()
        {
            InitializeComponent();
        }

        private void Number_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            AddToExpression(btn.Text, btn.Text);

        }

        private void Operator_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string op = btn.Text.Trim();   // Remove leading/trailing spaces

            switch (op)
            {
                case "×":
                    AddToExpression("×", "*");
                    break;

                case "÷":
                    AddToExpression("÷", "/");
                    break;

                default:
                    AddToExpression(op, op);
                    break;
            }

        }

        private void AddToExpression(string displayText, string expressionText)
        {
            // Only remove the initial 0 when entering a number or decimal point
            if (screen.Text == "0" &&
                (char.IsDigit(expressionText[0]) || expressionText == "0"))
            {
                screen.Text = "";
                expression = "";
            }

            // Prevent two operators
            if (expression.Length > 0 &&
                IsOperator(expression[expression.Length - 1].ToString()) &&
                IsOperator(expressionText))
            {
                return;
            }

            // Prevent multiple decimal points
            if (expressionText == "." && CurrentNumberHasDecimal())
            {
                return;
            }

            screen.Text += displayText;
            expression += expressionText;

        }

        private bool IsOperator(string text)
        {
            return text == "+" ||
                   text == "-" ||
                   text == "*" ||
                   text == "/" ||
                    text == "^" ||
                     text == "%";
        }

        private bool CurrentNumberHasDecimal()
        {
            for (int i = expression.Length - 1; i >= 0; i--)
            {
                char c = expression[i];

                if (c == '.')
                    return true;

                if (c == '+' || c == '-' || c == '*' || c == '/' || c == '^' || c == '(' || c == ')')
                    break;
            }

            return false;
        }

        private void ClearAll()
        {
            screen.Text = "0";
            expression = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          

            // Load custom cursor if available in the application folder, otherwise use default.
            string cursorPath = System.IO.Path.Combine(Application.StartupPath, "sci-cal cursor heart.cur");
            // Fallback to alternative known filename
            if (!System.IO.File.Exists(cursorPath))
            {
                string alt = System.IO.Path.Combine(Application.StartupPath, "flower.cur");
                if (System.IO.File.Exists(alt))
                {
                    cursorPath = alt;
                }
            }

            if (System.IO.File.Exists(cursorPath))
            {
                try
                {
                    this.Cursor = new Cursor(cursorPath);
                }
                catch
                {
                    // If the cursor file is invalid or loading fails, fall back to default.
                    this.Cursor = Cursors.Default;
                }
            }
            else
            {
                // Cursor file not found in output folder. Use default cursor to avoid exception.
                this.Cursor = Cursors.Default;
            }

            MakeCircular(btn0);
            MakeCircular(btn00);
            MakeCircular(btn1);
            MakeCircular(btn2);
            MakeCircular(btn3);
            MakeCircular(btn4);
            MakeCircular(btn5);
            MakeCircular(btn6);
            MakeCircular(btn7);
            MakeCircular(btn8);
            MakeCircular(btn9);
            MakeCircular(btnDecimal);

            RoundButton(btnAdd, 20);
            RoundButton(btnSubtraction, 20);
            RoundButton(btnMultiplication, 20);
            RoundButton(btnDivide, 20);
            RoundButton(btnOpen, 20);
            RoundButton(btnClose, 20);
            RoundButton(btnSquare, 20);
            RoundButton(btnSqrt, 20);
            RoundButton(btnPower, 20);
            RoundButton(btnPercent, 20);
            RoundButton(btnSin, 20);
            RoundButton(btnCos, 20);
            RoundButton(btnTan, 20);
            RoundButton(btnPlusMinus, 20);
            RoundButton(btnDelete, 20);
            RoundButton(btnEqual, 20);
            RoundButton(btnLog, 20);
            RoundButton(btnIn, 20);
            RoundButton(btnClear, 30);



        }


        private void MakeCircular(Button btn)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, btn.Width, btn.Height);
            btn.Region = new Region(path);
        }

        private void RoundButton(Button btn, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            path.StartFigure();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();

            btn.Region = new Region(path);

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
        }

        private void btnequals_Click(object sender, EventArgs e)
        {

            try
            {
                double answer = ExpressionEvaluator.Evaluate(expression);

                screen.Text = answer.ToString();
                expression = screen.Text;
            }
            catch
            {
                screen.Text = "Error";
                expression = "";
            }
        }

        private void btnDecimal_Click(object sender, EventArgs e)
        {
            AddToExpression(".", ".");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            screen.Text = "";
            expression = "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (expression.Length == 0)
                return;

            // Delete whole scientific functions
            if (expression.EndsWith("sqrt("))
            {
                expression = expression.Substring(0, expression.Length - 5);
                screen.Text = screen.Text.Substring(0, screen.Text.Length - 2);
            }
            else if (expression.EndsWith("sin("))
            {
                expression = expression.Substring(0, expression.Length - 4);
                screen.Text = screen.Text.Substring(0, screen.Text.Length - 4);
            }
            else if (expression.EndsWith("cos("))
            {
                expression = expression.Substring(0, expression.Length - 4);
                screen.Text = screen.Text.Substring(0, screen.Text.Length - 4);
            }
            else if (expression.EndsWith("tan("))
            {
                expression = expression.Substring(0, expression.Length - 4);
                screen.Text = screen.Text.Substring(0, screen.Text.Length - 4);
            }
            else
            {
                expression = expression.Substring(0, expression.Length - 1);

                if (screen.Text.Length > 0)
                    screen.Text = screen.Text.Substring(0, screen.Text.Length - 1);
            }

            if (screen.Text == "")
            {
                screen.Text = "";
                expression = "";
            }
        }

       
        
           private void btnOpen_Click(object sender, EventArgs e)
        {
            AddToExpression("(", "(");
        }
        

        private void btnClose_Click(object sender, EventArgs e)
        {
            AddToExpression(")", ")");
        }

        private void btnSquare_Click(object sender, EventArgs e)
        {
            AddToExpression("²", "^2");
        }

        private void btnSqrt_Click(object sender, EventArgs e)

        {
                AddToExpression("√(", "sqrt(");

   
            }

        private void btnSin_Click(object sender, EventArgs e)
        {
            AddToExpression("sin(", "sin(");
        }

        private void btnCos_Click(object sender, EventArgs e)
        {
            AddToExpression("cos(", "cos(");
        }

        private void btnTan_Click(object sender, EventArgs e)
        {
            AddToExpression("tan(", "tan(");
        }

        private void btnPlusMinus_Click(object sender, EventArgs e)
        {
            if (expression == "")
                return;

            int i = expression.Length - 1;

            // Find the last number
            while (i >= 0 && (char.IsDigit(expression[i]) || expression[i] == '.'))
            {
                i--;
            }

            int numberStart = i + 1;

            if (numberStart >= expression.Length)
                return;

            // If the number is already negative, remove the minus
            if (i >= 0 && expression[i] == '-')
            {
                if (i == 0 || "+-*/^(".Contains(expression[i - 1]))
                {
                    expression = expression.Remove(i, 1);
                }
                else
                {
                    expression = expression.Insert(numberStart, "-");
                }
            }
            else
            {
                expression = expression.Insert(numberStart, "-");
            }

            screen.Text = expression
                .Replace("*", "×")
                .Replace("/", "÷")
                 .Replace("sqrt(", "√(");
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            AddToExpression("log(", "log(");
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            AddToExpression("ln(", "ln(");
        }

        private void btnPercent_Click(object sender, EventArgs e)
        {
            AddToExpression("%", "%");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pnlCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            lastPoint = e.Location;
        }

        private void pnlCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                using (Graphics g = pnlCanvas.CreateGraphics())
                {
                    g.DrawLine(Pens.Black, lastPoint, e.Location);
                }

                lastPoint = e.Location;
            }
        }

        private void pnlCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
        }

        private void btnClearSketch_Click(object sender, EventArgs e)
        {
            pnlCanvas.Invalidate();

        }

        private void screen_Click(object sender, EventArgs e)
        {

        }
    }
}
