using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace treemove
{
    public class MainEngine : Component
    {
        #region Public Classes

        public class ExceptionOccurredEventArgs : EventArgs
        {
            public readonly bool CanContinue;
            public bool Continue;
            public readonly Exception Exception;

            public ExceptionOccurredEventArgs(Exception exception, bool canContinue = false)
            {
                CanContinue = canContinue;
                Continue = false;
                Exception = exception;
            }
        }

        public delegate void ExceptionOccurredEventHandler(object sender, ExceptionOccurredEventArgs e);

        #endregion

        #region Private Classes

        private delegate bool FileOperationFunction(string[] fileNames, string destDirectory, IntPtr handle);

        private class FileItem
        {
            public string FileName;
            public int Index;

            public FileItem(string fileName, int index)
            {
                FileName = fileName;
                Index = index;
            }
        }

        #endregion

        #region Private Fields

        private static readonly string directorySeparatorString = Path.DirectorySeparatorChar.ToString();
        private static readonly string volumeSeparatorString = Path.VolumeSeparatorChar.ToString();

        #endregion

        #region Public Properties

        public event ExceptionOccurredEventHandler ExceptionOccurred = delegate { };

        #endregion

        #region Public Methods

        public int[] Operate(string[] files, string destination, bool copy, IntPtr handle)
        {
            if (string.IsNullOrEmpty(destination))
            {
                throw new Exception("コピー／移動先が空です。");
            }

            var completedFileIndices = new List<int>();

            try
            {
                completedFileIndices.Capacity = files.Length;
            }
            catch
            {
                throw;
            }

            try
            {
                var operation = (copy ? new FileOperationFunction(FileOperation.Copy) : new FileOperationFunction(FileOperation.Move));
                var destAndTargets = new Dictionary<string, List<FileItem>>();

                for (int i = 0; i < files.Length; ++i)
                {
                    string fileName = files[i];
                    string source = fileName;
                    string destRoot = destination;

                    if (!Uri.TryCreate(destRoot, UriKind.Absolute, out Uri uri))
                    {
                        DirectoryInfo directory;

                        try
                        {
                            directory = new DirectoryInfo(Path.GetDirectoryName(fileName));
                        }
                        catch
                        {
                            throw new Exception("ファイル名不正\r\n" + fileName);
                        }

                        if (destRoot.StartsWith(Path.DirectorySeparatorChar.ToString()) ||
                            destRoot.StartsWith(Path.PathSeparator.ToString()))
                        {
                            string root = directory.Root.FullName;

                            if (destRoot.Length < 2)
                            {
                                destRoot = root;
                            }
                            else
                            {
                                try
                                {
                                    destRoot = Path.Combine(root, destRoot.Substring(1));
                                }
                                catch
                                {
                                    throw new Exception("コピー／移動先が不正です。\r\n" + fileName);
                                }
                            }
                        }
                        else
                        {
                            bool current;
                            bool parent;

                            do
                            {
                                current = IsCurrent(destRoot);

                                if (current)
                                {
                                    parent = false;

                                    if (destRoot.Length < 3)
                                    {
                                        throw new Exception("コピー／移動先が不正です。\r\n" + fileName);
                                    }

                                    destRoot = destRoot.Substring(2);
                                    continue;
                                }

                                parent = IsParent(destRoot);

                                if (parent)
                                {
                                    if (destRoot.Length < 4)
                                    {
                                        throw new Exception("コピー／移動先が不正です。\r\n" + fileName);
                                    }

                                    destRoot = destRoot.Substring(3);
                                }
                            }
                            while (current || parent);
                        }
                    }

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

                    if (!(destAndTargets.TryGetValue(dest, out List<FileItem> value)))
                    {
                        value = new List<FileItem>
                        {
                            new FileItem(source, i)
                        };

                        destAndTargets.Add(dest, value);
                    }
                    else
                    {
                        value.Add(new FileItem(source, i));
                    }
                }

                foreach (string key in destAndTargets.Keys)
                {
                    try
                    {
                        if (!(destAndTargets.TryGetValue(key, out List<FileItem> files_)))
                        {
                            continue;
                        }

                        var directoryInfo = new DirectoryInfo(key);

                        if (!(directoryInfo.Exists))
                        {
                            directoryInfo.Create();
                        }

                        var fileNames = new string[files_.Count];

                        for (int i = 0; i < fileNames.Length; ++i)
                        {
                            fileNames[i] = files_[i].FileName;
                        }

                        if (operation(fileNames, key, handle))
                        {
                            foreach (FileItem item in files_)
                            {
                                completedFileIndices.Add(item.Index);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        var eventArgs = new ExceptionOccurredEventArgs(exception, true);
                        ExceptionOccurred(this, eventArgs);

                        if (!eventArgs.Continue)
                        {
                            break;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return completedFileIndices.ToArray();
        }

        #endregion

        #region Private Methods

        private bool IsCurrent(string path)
        {
            return
                path.StartsWith($".{Path.DirectorySeparatorChar}") ||
                path.StartsWith($".{Path.PathSeparator}");
        }

        private bool IsParent(string path)
        {
            return
                path.StartsWith($"..{Path.DirectorySeparatorChar}") ||
                path.StartsWith($"..{Path.PathSeparator}");
        }

        #endregion
    }
}
