namespace treemove
{
    partial class FormMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.labelSource = new System.Windows.Forms.Label();
            this.listBoxFiles = new System.Windows.Forms.ListBox();
            this.labelDest = new System.Windows.Forms.Label();
            this.comboBoxDest = new System.Windows.Forms.ComboBox();
            this.buttonMove = new System.Windows.Forms.Button();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.mainEngine = new treemove.MainEngine();
            this.SuspendLayout();
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(12, 12);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "&Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(93, 12);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 1;
            this.buttonRemove.Text = "&Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(174, 12);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 2;
            this.buttonClear.Text = "C&lear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.Location = new System.Drawing.Point(12, 38);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(57, 12);
            this.labelSource.TabIndex = 3;
            this.labelSource.Text = "&Source:";
            // 
            // listBoxFiles
            // 
            this.listBoxFiles.AllowDrop = true;
            this.listBoxFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxFiles.FormattingEnabled = true;
            this.listBoxFiles.HorizontalScrollbar = true;
            this.listBoxFiles.ItemHeight = 12;
            this.listBoxFiles.Location = new System.Drawing.Point(12, 53);
            this.listBoxFiles.Name = "listBoxFiles";
            this.listBoxFiles.Size = new System.Drawing.Size(260, 124);
            this.listBoxFiles.TabIndex = 4;
            this.listBoxFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBoxFiles_DragDrop);
            this.listBoxFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBoxFiles_DragEnter);
            // 
            // labelDest
            // 
            this.labelDest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDest.AutoSize = true;
            this.labelDest.Location = new System.Drawing.Point(12, 185);
            this.labelDest.Name = "labelDest";
            this.labelDest.Size = new System.Drawing.Size(47, 12);
            this.labelDest.TabIndex = 5;
            this.labelDest.Text = "&Dest:";
            // 
            // comboBoxDest
            // 
            this.comboBoxDest.AllowDrop = true;
            this.comboBoxDest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxDest.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxDest.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.comboBoxDest.FormattingEnabled = true;
            this.comboBoxDest.Location = new System.Drawing.Point(12, 200);
            this.comboBoxDest.Name = "comboBoxDest";
            this.comboBoxDest.Size = new System.Drawing.Size(260, 20);
            this.comboBoxDest.TabIndex = 6;
            this.comboBoxDest.DragDrop += new System.Windows.Forms.DragEventHandler(this.comboBoxDest_DragDrop);
            this.comboBoxDest.DragEnter += new System.Windows.Forms.DragEventHandler(this.comboBoxDest_DragEnter);
            // 
            // buttonMove
            // 
            this.buttonMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMove.Location = new System.Drawing.Point(116, 226);
            this.buttonMove.Name = "buttonMove";
            this.buttonMove.Size = new System.Drawing.Size(75, 23);
            this.buttonMove.TabIndex = 7;
            this.buttonMove.Text = "&Move";
            this.buttonMove.UseVisualStyleBackColor = true;
            this.buttonMove.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // buttonCopy
            // 
            this.buttonCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopy.Location = new System.Drawing.Point(197, 226);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(75, 23);
            this.buttonCopy.TabIndex = 8;
            this.buttonCopy.Text = "&Copy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // mainEngine
            // 
            this.mainEngine.ExceptionOccurred += new treemove.MainEngine.ExceptionOccurredEventHandler(this.mainEngine_ExceptionOccurred);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.buttonCopy);
            this.Controls.Add(this.buttonMove);
            this.Controls.Add(this.comboBoxDest);
            this.Controls.Add(this.labelDest);
            this.Controls.Add(this.listBoxFiles);
            this.Controls.Add(this.labelSource);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMain";
            this.Text = "Tree Move";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.ListBox listBoxFiles;
        private System.Windows.Forms.Label labelDest;
        private System.Windows.Forms.ComboBox comboBoxDest;
        private System.Windows.Forms.Button buttonMove;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private MainEngine mainEngine;
    }
}

