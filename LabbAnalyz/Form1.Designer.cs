namespace LabbAnalyz
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SourceCodeTextBox = new TextBox();
            LoadButton = new Button();
            AnalyzeButton = new Button();
            SyntaxAnalysis = new Button();
            ResultsTextBox1 = new RichTextBox();
            SyntBox = new TextBox();
            SuspendLayout();
            // 
            // SourceCodeTextBox
            // 
            SourceCodeTextBox.Location = new Point(44, 49);
            SourceCodeTextBox.Multiline = true;
            SourceCodeTextBox.Name = "SourceCodeTextBox";
            SourceCodeTextBox.ScrollBars = ScrollBars.Vertical;
            SourceCodeTextBox.Size = new Size(260, 246);
            SourceCodeTextBox.TabIndex = 0;
            // 
            // LoadButton
            // 
            LoadButton.Location = new Point(44, 365);
            LoadButton.Name = "LoadButton";
            LoadButton.Size = new Size(94, 29);
            LoadButton.TabIndex = 3;
            LoadButton.Text = "Load";
            LoadButton.UseVisualStyleBackColor = true;
            LoadButton.Click += LoadButton_Click;
            // 
            // AnalyzeButton
            // 
            AnalyzeButton.Location = new Point(164, 365);
            AnalyzeButton.Name = "AnalyzeButton";
            AnalyzeButton.Size = new Size(94, 29);
            AnalyzeButton.TabIndex = 4;
            AnalyzeButton.Text = "Syntax";
            AnalyzeButton.UseVisualStyleBackColor = true;
            AnalyzeButton.Click += AnalyzeButton_Click;
            // 
            // SyntaxAnalysis
            // 
            SyntaxAnalysis.Location = new Point(285, 365);
            SyntaxAnalysis.Name = "SyntaxAnalysis";
            SyntaxAnalysis.Size = new Size(94, 29);
            SyntaxAnalysis.TabIndex = 5;
            SyntaxAnalysis.Text = "Analyze";
            SyntaxAnalysis.UseVisualStyleBackColor = true;
            SyntaxAnalysis.Click += SyntaxAnalysis_Click;
            // 
            // ResultsTextBox1
            // 
            ResultsTextBox1.Location = new Point(436, 49);
            ResultsTextBox1.Name = "ResultsTextBox1";
            ResultsTextBox1.ScrollBars = RichTextBoxScrollBars.Vertical;
            ResultsTextBox1.Size = new Size(444, 406);
            ResultsTextBox1.TabIndex = 6;
            ResultsTextBox1.Text = "";
            // 
            // SyntBox
            // 
            SyntBox.Location = new Point(948, 49);
            SyntBox.Name = "SyntBox";
            SyntBox.Size = new Size(125, 27);
            SyntBox.TabIndex = 7;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1224, 493);
            Controls.Add(SyntBox);
            Controls.Add(ResultsTextBox1);
            Controls.Add(SyntaxAnalysis);
            Controls.Add(AnalyzeButton);
            Controls.Add(LoadButton);
            Controls.Add(SourceCodeTextBox);
            Name = "MainForm";
            Text = "MainForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox SourceCodeTextBox;
        private Button LoadButton;
        private Button AnalyzeButton;
        private Button SyntaxAnalysis;
        private RichTextBox ResultsTextBox1;
        private TextBox SyntBox;
    }
}