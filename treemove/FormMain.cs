using System;
using System.IO;
using System.Windows.Forms;

namespace treemove
{
    public partial class FormMain : Form
    {
        #region Public Methods

        public FormMain()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.Sizable;
        }

        #endregion

        #region Private Methods

        private void addFileName(string fileName)
        {
            if (listBoxFiles.Items.IndexOf(fileName) >= 0)
            {
                return;
            }

            listBoxFiles.Items.Add(fileName);
        }

        private void enableControls(bool enabled)
        {
            foreach (Control control in Controls)
            {
                control.Enabled = enabled;
            }
        }

        private bool getDropData(DragEventArgs dragEventArgs, out string[] data)
        {
            data = (dragEventArgs.Data.GetData(DataFormats.FileDrop)) as string[];

            if ((data == null) || (data.Length < 1))
            {
                return false;
            }

            return true;
        }

        private void operate(bool copy)
        {
            int count = listBoxFiles.Items.Count;

            if (count < 1)
            {
                return;
            }

            var files = new string[count];

            for (int i = 0; i < count; ++i)
            {
                files[i] = listBoxFiles.Items[i].ToString();
            }

            enableControls(false);

            try
            {
                int[] completedFileNames = mainEngine.Operate(files, comboBoxDest.Text, copy, Handle);
                removeItems(completedFileNames);
            }
            catch (Exception exception)
            {
                showErrorMessage(exception.Message);
            }
            finally
            {
                enableControls(true);
            }
        }

        private void removeItems(int[] indices)
        {
            if (indices == null)
            {
                return;
            }

            int length = indices.Length;

            if (length < 1)
            {
                return;
            }

            listBoxFiles.BeginUpdate();
            var sortedIndices = new int[length];
            Array.Copy(indices, sortedIndices, indices.Length);
            Array.Sort(sortedIndices);

            try
            {
                for (int i = 0; i < sortedIndices.Length; ++i)
                {
                    listBoxFiles.Items.RemoveAt(sortedIndices[i] - i);
                }
            }
            catch (Exception exception)
            {
                showErrorMessage(exception.Message);
            }

            listBoxFiles.EndUpdate();
        }

        private void showErrorMessage(string message)
        {
            showMessage(message, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private DialogResult showMessage(string text, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(this, text, Text, buttons, icon);
        }

        #endregion

        // Designer's Methods

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string[] fileNames = openFileDialog.FileNames;

            foreach (string fileName in fileNames)
            {
                addFileName(fileName);
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            listBoxFiles.Items.Clear();
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            operate(true);
        }

        private void buttonMove_Click(object sender, EventArgs e)
        {
            operate(false);
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            ListBox.SelectedObjectCollection currentSelectedItems = listBoxFiles.SelectedItems;
            int selectedItemsCount = currentSelectedItems.Count;

            if (selectedItemsCount < 1)
            {
                return;
            }

            var selectedItems = new object[selectedItemsCount];

            try
            {
                currentSelectedItems.CopyTo(selectedItems, 0);
            }
            catch
            {
                return;
            }

            listBoxFiles.BeginUpdate();

            foreach (object item in selectedItems)
            {
                listBoxFiles.Items.Remove(item);
            }

            listBoxFiles.EndUpdate();
        }

        private void comboBoxDest_DragDrop(object sender, DragEventArgs e)
        {
            string[] dropData;

            if (!getDropData(e, out dropData))
            {
                return;
            }

            string data = dropData[0];

            try
            {
                var info = new DirectoryInfo(data);

                if (info.Exists)
                {
                    comboBoxDest.Text = data;
                }
            }
            catch (Exception exception)
            {
                showErrorMessage(exception.Message);
            }
        }

        private void comboBoxDest_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = (e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None);
        }

        private void listBoxFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] dropData;

            if (!getDropData(e, out dropData))
            {
                return;
            }

            foreach (string data in dropData)
            {
                try
                {
                    FileSystemInfo info;
                    info = new FileInfo(data);

                    if (!(info.Exists))
                    {
                        info = new DirectoryInfo(data);

                        if (!(info.Exists))
                        {
                            continue;
                        }
                    }

                    addFileName(info.FullName);
                }
                catch (Exception exception)
                {
                    DialogResult dialogResult = showMessage(
                        string.Format("{0}\r\n\r\n{1}", exception.Message, data), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                    if (dialogResult == DialogResult.Cancel)
                    {
                        break;
                    }
                }
            }
        }

        private void listBoxFiles_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = (e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None);
        }

        private void mainEngine_ExceptionOccurred(object sender, MainEngine.ExceptionOccurredEventArgs e)
        {
            bool canContinue = e.CanContinue;
            MessageBoxButtons buttons = canContinue ? MessageBoxButtons.OKCancel : MessageBoxButtons.OK;
            DialogResult dialogResult = showMessage(e.Exception.Message, buttons, MessageBoxIcon.Error);

            if (canContinue)
            {
                e.Continue = dialogResult == DialogResult.OK;
            }
        }
    }
}
