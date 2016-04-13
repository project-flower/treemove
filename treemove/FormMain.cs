using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace treemove
{
    public partial class FormMain : Form
    {
        delegate bool FileOperationFunction(string[] fileNames, string destDirectory, IntPtr handle);

        void addFileName(string fileName)
        {
            if (listBoxFiles.Items.IndexOf(fileName) >= 0)
            {
                return;
            }

            listBoxFiles.Items.Add(fileName);
        }

        bool getDropData(DragEventArgs dragEventArgs, out string[] data)
        {
            data = (dragEventArgs.Data.GetData(DataFormats.FileDrop)) as string[];

            if ((data == null) || (data.Length < 1))
            {
                return false;
            }

            return true;
        }

        void operate(bool copy)
        {
            Control.ControlCollection controls = Controls;

            foreach (Control control in controls)
            {
                control.Enabled = false;
            }

            ListBox.ObjectCollection items = listBoxFiles.Items;
            var passedItems = new List<object>(items.Count);
            var operation = (copy ? new FileOperationFunction(FileOperation.Copy) : new FileOperationFunction(FileOperation.Move));

            foreach (object item in items)
            {
                string source = item.ToString();
                string destRoot = comboBoxDest.Text;

                if (!(destRoot.EndsWith("\\")))
                {
                    destRoot = destRoot + "\\";
                }

                string dest = source;
                dest = dest.Replace("\\\\", "\\");
                dest = dest.Replace(":", string.Empty);
                dest = destRoot + dest;

                try
                {
                    if (operation(new string[] { source }, dest, Handle))
                    {
                        passedItems.Add(item);
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

            foreach (object passedItem in passedItems)
            {
                listBoxFiles.Items.Remove(passedItem);
            }

            listBoxFiles.EndUpdate();

            foreach (Control control in controls)
            {
                control.Enabled = true;
            }
        }

        public FormMain()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.Sizable;
        }

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

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            operate(true);
        }

        private void buttonMove_Click(object sender, EventArgs e)
        {
            operate(false);
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
                MessageBox.Show(exception.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
