using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace treemove
{
    public partial class FormMain : Form
    {
        #region Private Classes

        private delegate bool FileOperationFunction(string[] fileNames, string destDirectory, IntPtr handle);

        #endregion

        #region Private Fields

        private readonly string directorySeparatorString = Path.DirectorySeparatorChar.ToString();
        private readonly string volumeSeparatorString = Path.VolumeSeparatorChar.ToString();

        #endregion

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
            Control.ControlCollection controls = Controls;

            foreach (Control control in controls)
            {
                control.Enabled = false;
            }

            try
            {
                ListBox.ObjectCollection items = listBoxFiles.Items;
                var passedFilesNames = new List<string>(items.Count);
                var operation = (copy ? new FileOperationFunction(FileOperation.Copy) : new FileOperationFunction(FileOperation.Move));
                var destAndTargets = new Dictionary<string, List<string>>();

                foreach (object item in items)
                {
                    string source = item.ToString();
                    string destRoot = comboBoxDest.Text;

                    if (!(destRoot.EndsWith(directorySeparatorString)))
                    {
                        destRoot += Path.DirectorySeparatorChar;
                    }

                    string dest = source;
                    dest = dest.Replace("\\\\", directorySeparatorString);
                    dest = dest.Replace(volumeSeparatorString, string.Empty);
                    dest = destRoot + dest;
                    int index = dest.LastIndexOf(directorySeparatorString);

                    if (index < 0)
                    {
                        throw new Exception("コピー先作成不可\r\n" + dest);
                    }

                    dest = dest.Substring(0, index);
                    List<string> value;

                    if (!(destAndTargets.TryGetValue(dest, out value)))
                    {
                        value = new List<string>();
                        value.Add(source);
                        destAndTargets.Add(dest, value);
                    }
                    else
                    {
                        value.Add(source);
                    }
                }

                foreach (string key in destAndTargets.Keys)
                {
                    try
                    {
                        List<string> files;

                        if (!(destAndTargets.TryGetValue(key, out files)))
                        {
                            continue;
                        }

                        var directoryInfo = new DirectoryInfo(key);

                        if (!(directoryInfo.Exists))
                        {
                            directoryInfo.Create();
                        }

                        if (operation(files.ToArray(), key, Handle))
                        {
                            passedFilesNames.AddRange(files);
                        }
                    }
                    catch (Exception exception)
                    {
                        DialogResult result = MessageBox.Show(exception.Message, Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                        if (result == DialogResult.Cancel)
                        {
                            break;
                        }
                    }
                }

                listBoxFiles.BeginUpdate();

                foreach (string passedFileName in passedFilesNames)
                {
                    for (int index = 0; index < listBoxFiles.Items.Count; ++index)
                    {
                        object item = listBoxFiles.Items[index];

                        if (item.ToString() == passedFileName)
                        {
                            listBoxFiles.Items.Remove(item);
                            break;
                        }
                    }
                }

                listBoxFiles.EndUpdate();
            }
            catch (Exception exception)
            {
                showErrorMessage(exception.Message);
            }

            foreach (Control control in controls)
            {
                control.Enabled = true;
            }
        }

        private void showErrorMessage(string message)
        {
            MessageBox.Show(message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    DialogResult result = MessageBox.Show(exception.Message, Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                    if (result == DialogResult.Cancel)
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
    }
}
