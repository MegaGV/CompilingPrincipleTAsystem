using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompilingPrinciple
{
    public partial class Form1 : Form
    {
        private string path = "";
        WordAnalyzer wordAnalyse = new WordAnalyzer();
        SentenceAnalyzer sentenceAnalyzer = new SentenceAnalyzer();
        List<Token> tokens;
        List<WrongToken> wrongtokens;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openHelp(object sender, EventArgs e)
        {
            string fileName = @"../../帮助.chm";
            Help.ShowHelp(this, fileName);
        }

        private void openFile(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "*.txt | *.txt";
            if (openfile.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                this.path = openfile.FileName;
                this.richTextBox1.Text = File.ReadAllText(this.path, Encoding.UTF8);
            }
        }

        private void saveFile(object sender, EventArgs e)
        {
            if (this.path == "") {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "*.txt | *.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    this.path = sfd.FileName.ToString();
                    StreamWriter sw = new StreamWriter(this.path);
                    sw.Write(this.richTextBox1.Text);
                    sw.Dispose();
                }
            }
            else
            {
                StreamWriter sw = new StreamWriter(this.path);
                sw.Write(this.richTextBox1.Text);
                sw.Dispose();
            }
        }

        private void saveAs(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "*.txt | *.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                this.path = sfd.FileName.ToString();
                StreamWriter sw = new StreamWriter(this.path);
                sw.Write(this.richTextBox1.Text);
                sw.Dispose();
            }
        }

        private void WA(object sender, EventArgs e)
        {
            wordAnalyse.WordAnalyse(this.richTextBox1.Text);
            tokens = wordAnalyse.getTokens();
            wrongtokens = wordAnalyse.getWrongTokens();
            sentenceAnalyzer.setTokens(tokens);
            this.richTextBox2.Text = wordAnalyse.getTokensInfo();
            this.richTextBox3.Text = wordAnalyse.getWrongTokensInfo();
        }

        private void ExpressionAnalyse(object sender, EventArgs e)
        {
            if (tokens.Count != 0)
            {
                sentenceAnalyzer.expressionAnalyse();
                this.richTextBox2.Text = sentenceAnalyzer.getExpressionArgsInfo();
                this.richTextBox3.Text = sentenceAnalyzer.getExpressionWrongInfo();
            }
            else
            {
                this.richTextBox3.Text = "please get the tokens first";
            }
        }

        private void BoolExpressionAnalyse(object sender, EventArgs e)
        {
            if (tokens.Count != 0)
            {
                sentenceAnalyzer.boolExpressionAnalyse();
                this.richTextBox2.Text = sentenceAnalyzer.getBoolExpressionArgsInfo();
                this.richTextBox3.Text = sentenceAnalyzer.getBoolExpressionWrongInfo();
            }
            else
            {
                this.richTextBox3.Text = "please get the tokens first";
            }
        }

        private void AssignmentAnalyse(object sender, EventArgs e)
        {
            if (tokens.Count != 0)
            {
                sentenceAnalyzer.assignmentAnalyse();
                this.richTextBox2.Text = sentenceAnalyzer.getAssignmentArgsInfo();
                this.richTextBox3.Text = sentenceAnalyzer.getAssignmentWrongInfo();
            }
            else
            {
                this.richTextBox3.Text = "please get the tokens first";
            }
        }

        private void IfsAnalyse(object sender, EventArgs e)
        {
            if (tokens.Count != 0)
            {
                sentenceAnalyzer.ifsAnalyse();
                this.richTextBox2.Text = sentenceAnalyzer.getIfsArgsInfo();
                this.richTextBox3.Text = sentenceAnalyzer.getIfsWrongInfo();
            }
            else
            {
                this.richTextBox3.Text = "please get the tokens first";
            }
        }

        private void ForAnalyse(object sender, EventArgs e)
        {
            if (tokens.Count != 0)
            {
                sentenceAnalyzer.forAnalyse();
                this.richTextBox2.Text = sentenceAnalyzer.getForArgsInfo();
                this.richTextBox3.Text = sentenceAnalyzer.getForWrongInfo();
            }
            else
            {
                this.richTextBox3.Text = "please get the tokens first";
            }
        }

        private void WhileAnalyse(object sender, EventArgs e)
        {
            if (tokens.Count != 0)
            {
                sentenceAnalyzer.whileAnalyse();
                this.richTextBox2.Text = sentenceAnalyzer.getWhileArgsInfo();
                this.richTextBox3.Text = sentenceAnalyzer.getWhileWrongInfo();
            }
            else
            {
                this.richTextBox3.Text = "please get the tokens first";
            }
        }

        private void DoWhileAnalyse(object sender, EventArgs e)
        {
            if (tokens.Count != 0)
            {
                sentenceAnalyzer.doWhileAnalyse();
                this.richTextBox2.Text = sentenceAnalyzer.getDoWhileArgsInfo();
                this.richTextBox3.Text = sentenceAnalyzer.getDoWhileWrongInfo();
            }
            else
            {
                this.richTextBox3.Text = "please get the tokens first";
            }
        }
    }
}
