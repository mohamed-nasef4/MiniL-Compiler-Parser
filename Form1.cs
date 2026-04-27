using System.Text.RegularExpressions;
using System.Data;
namespace compiler_project

{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string inputString = textBox1.Text;


            string keywords = @"\b(num|text|check|otherwise|until|repeat|then)\b";
            string identifiers = @"\b[a-zA-Z][a-zA-Z0-9]*\b";
            string numbers = @"\b[0-9]+\b";
            string operators = @":=|==|[\+\-\*/<>]";
            string symbols = @"[;{}()]";

            string masterPattern = $"{keywords}|{identifiers}|{numbers}|{operators}|{symbols}";

            DataTable dt = new DataTable();
            dt.Columns.Add("Lexeme");
            dt.Columns.Add("Token Type");

            MatchCollection matches = Regex.Matches(inputString, masterPattern);
            List<Token> allTokens = new List<Token>();

            foreach (Match m in matches)
            {
                string lex = m.Value.Trim();
                string type = "";

                if (Regex.IsMatch(lex, keywords)) type = "Keyword";
                else if (Regex.IsMatch(lex, numbers)) type = "Number";
                else if (Regex.IsMatch(lex, identifiers)) type = "Identifier";
                else if (lex == ":=") type = "Assignment_Op";
                else if (lex == "==") type = "Equal_Op";
                else if (lex == "+") type = "Plus_Op";
                else if (lex == "-") type = "Minus_Op";
                else if (lex == "*") type = "Multiply_Op";
                else if (lex == "/") type = "Divide_Op";
                else if (lex == ">") type = "Greater_Than_Op";
                else if (lex == "<") type = "Less_Than_Op";
                else if (lex == ";") type = "Semicolon";
                else if (lex == "(") type = "Left_Paren";
                else if (lex == ")") type = "Right_Paren";
                else if (lex == "{") type = "Left_Brace";
                else if (lex == "}") type = "Right_Brace";
                else type = "Unknown";

                allTokens.Add(new Token { Value = lex, Type = type });
                dt.Rows.Add(lex, type);
            }

            dataGridView1.DataSource = dt;

            try
            {
                if (allTokens.Count == 0) return;

                MiniLParser parser = new MiniLParser(allTokens);
                parser.ParseProgram();

                MessageBox.Show("Success: Your miniL code is correct!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Syntax Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
