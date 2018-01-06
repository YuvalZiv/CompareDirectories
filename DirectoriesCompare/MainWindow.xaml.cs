using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using NLog;

namespace DirectoriesCompare
{
    public partial class MainWindow : Window
    {
        #region Consts
        private const int BytesToRead = sizeof(Int64);
        #region ToolTip Consts
        private const string CompareDataCheckedToolTipData = "Compares the data of the files aswell.";
        private const string CompareDataUnheckedToolTipData = "Doesn't compare the data of the files.";
        private const string CompareStructureCheckedToolTipData = "Compares the structure of the files aswell.\r\nCompare structure checks that every file in both directories is in the same partial path";
        private const string CompareStructureUnheckedToolTipData = "Doesn't compare the structure of the files.\r\nCompare structure checks that every file in both directories is in the same partial path";
        private const string FoundToolTipDataFormat = "Found {0} matching files.";
        private const string NotFoundToolTipDataFormat = "Couldn't find {0} files from the first directory in the second.\r\nCouldn't find {1} files from the second directory in the first.";
        #endregion ToolTip Consts
        #region TextBox Consts
        private const string SummaryFormat = "{0}:\r\nFiles - {1}\r\nSub Directories - {2}\r\nSize - {3} {4}";
        private const string FoundOutputFormat = "Found in both directories:\r\n{0}- ";
        private const string NotFoundOutputFormat = "Not found in first directory:\r\n- {0}\r\n\r\nNot found in second directory:\r\n- {1}";
        private const string ConclustionExistsAll = "All {0} files exists in both directories.";
        private const string ConclustionNotAllExists = "{0} files from the first directory are missing in the second directory.\r\n{1} files from the second directory are missing in the first directory.";
        private readonly string[] ByteMagnitude = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        #endregion TextBox Consts
        #endregion Consts

        System.Windows.Forms.FolderBrowserDialog folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
        private Dictionary<Button, Tuple<TextBox, TextBox>> BrowseButtonMatchingData = new Dictionary<Button, Tuple<TextBox, TextBox>>();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BrowseButtonMatchingData.Add(FirstDirectoryBrowse, new Tuple<TextBox, TextBox>(FirstDirectoryPath, FirstSummary));
            BrowseButtonMatchingData.Add(SecondDirectoryBrowse, new Tuple<TextBox, TextBox>(SecondDirectoryPath, SecondSummary));
            folderBrowser.RootFolder = Environment.SpecialFolder.Desktop;
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Tuple<TextBox, TextBox> browseButtonData = BrowseButtonMatchingData[(Button)sender];
                browseButtonData.Item1.Text = folderBrowser.SelectedPath;
                long sizeInBytes = 0;
                int filesCount = 0, subDirectoriesCount = 0;
                GetStats(folderBrowser.SelectedPath, ref sizeInBytes, ref subDirectoriesCount, ref filesCount);
                string magnitude = String.Empty;
                double sizeInMagnitude = ShrinkByteMagnitude(sizeInBytes, out magnitude);
                browseButtonData.Item2.Text = String.Format(SummaryFormat, folderBrowser.SelectedPath, filesCount, subDirectoriesCount, sizeInMagnitude, magnitude);
                browseButtonData.Item2.Foreground = Brushes.Black;
                browseButtonData.Item2.ToolTip = browseButtonData.Item2.Text;
            }
        }

        private void GetStats(string folderPath, ref long sizeInBytes, ref int subDirectoriesCount, ref int filesCount)
        {
            if (!(Directory.Exists(folderPath)))
            {
                logger.Error("Unknown directory at " + folderPath);
                return;
            }

            foreach (string absoluteSubDirectory in Directory.GetDirectories(folderPath))
            {
                try
                {
                    GetStats(absoluteSubDirectory, ref sizeInBytes, ref subDirectoriesCount, ref filesCount);
                    subDirectoriesCount++;
                }
                catch (UnauthorizedAccessException cause)
                {
                    logger.Error("Couldn't access directory at " + absoluteSubDirectory, cause);
                }
                catch (PathTooLongException cause)
                {
                    logger.Error("Couldn't access directory at " + absoluteSubDirectory + " because the path was too long.", cause);
                }
            }

            foreach (string absoluteFilePath in Directory.GetFiles(folderPath))
            {
                filesCount++;
                sizeInBytes += new FileInfo(absoluteFilePath).Length;
            }
        }

        private double ShrinkByteMagnitude(long sizeInBytes, out string magnitude)
        {
            int magnitudeValue = 0;
            double size = sizeInBytes;
            while (size > 1000)
            {
                size /= 1000;
                magnitudeValue++;
            }
            magnitude = ByteMagnitude[magnitudeValue];
            return Math.Round(size, 2);
        }

        private void Compare_Click(object sender, RoutedEventArgs e)
        {
            if (!(InputCheck(FirstDirectoryPath.Text, SecondDirectoryPath.Text)))
                return;

            bool compareStructure = IsChecked(CompareStructure);
            List<string> firstDirectoryTree = TreeFolder(FirstDirectoryPath.Text);
            List<string> secondDirectoryTree = TreeFolder(SecondDirectoryPath.Text);
            ShowComparison(firstDirectoryTree, secondDirectoryTree, FirstDirectoryPath.Text, SecondDirectoryPath.Text, IsChecked(CompareFilesData), IsChecked(CompareStructure));
        }

        private bool InputCheck(string p1, string p2)
        {
            if (String.IsNullOrWhiteSpace(FirstDirectoryPath.Text) || String.IsNullOrWhiteSpace(SecondDirectoryPath.Text))
            {
                MessageBox.Show("Empty folder path");
                return false;
            }

            if (!(CheckDirectory(FirstDirectoryPath.Text)))
            {
                return false;
            }

            if (!(CheckDirectory(SecondDirectoryPath.Text)))
            {
                return false;
            }

            if (string.Equals(FirstDirectoryPath.Text, SecondDirectoryPath.Text, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Please enter different directory paths.");
                return false;
            }
            return true;
        }

        private bool CheckDirectory(string directoryPath)
        {
            if (!(Directory.Exists(directoryPath)))
            {
                MessageBox.Show("Directory at " + directoryPath + " doesn't exists.");
                return false;
            }
            return true;
        }

        private bool IsChecked(CheckBox checkBox)
        {
            return checkBox.IsChecked.GetValueOrDefault();
        }

        private List<string> TreeFolder(string parentDirectoryPath)
        {
            return TreeFolder(parentDirectoryPath, parentDirectoryPath);
        }

        private List<string> TreeFolder(string parentDirectoryPath, string directoryPath)
        {
            List<string> files = new List<string>();

            foreach (string absoluteSubDirectoryPath in Directory.GetDirectories(directoryPath))
                files.AddRange(TreeFolder(parentDirectoryPath, absoluteSubDirectoryPath));

            foreach (string absoluteFilePath in Directory.GetFiles(directoryPath))
                files.Add(absoluteFilePath.Remove(0, parentDirectoryPath.Length + 1));

            return files;
        }

        private void ShowComparison(List<string> firstDirectoryTree, List<string> secondDirectoryTree,
            string firstDirectoryParent, string secondDirectoryParent, bool compareData, bool compareStructure)
        {
            Tuple<List<string>, List<string>, List<string>> results = CompareTwoLists(firstDirectoryTree, secondDirectoryTree,
                firstDirectoryParent, secondDirectoryParent, compareData, compareStructure);

            Found.Text = String.Format(FoundOutputFormat, String.Join("\r\n- ", results.Item1));
            NotFound.Text = String.Format(NotFoundOutputFormat, String.Join("\r\n- ", results.Item2), String.Join("\r\n- ", results.Item3));
            Found.ToolTip = String.Format(FoundToolTipDataFormat, results.Item1.Count);
            NotFound.ToolTip = String.Format(NotFoundToolTipDataFormat, results.Item2.Count, results.Item3.Count);
            if ((results.Item2.Count == 0) && (results.Item3.Count == 0))
            {
                Conclusion.Text = String.Format(ConclustionExistsAll, results.Item1.Count);
            }
            else
            {
                Conclusion.Text = String.Format(ConclustionNotAllExists, results.Item2.Count, results.Item3.Count);
            }

            Found.Foreground = Brushes.Black;
            NotFound.Foreground = Brushes.Black;
            Conclusion.Foreground = Brushes.Black;
        }

        private Tuple<List<string>, List<string>, List<string>> CompareTwoLists(List<string> firstDirectoryTree, List<string> secondDirectoryTree,
            string firstDirectoryParent, string secondDirectoryParent, bool compareData, bool compareStructure)
        {
            List<string> both = new List<string>(), first = new List<string>(), second = new List<string>();
            int index;
            foreach (string file in firstDirectoryTree)
            {
                if (Path.GetFileName(file) == "IMG_20160407_153749.jpg")
                    Console.WriteLine();
                if ((index = IndexOf(secondDirectoryTree, file, compareStructure)) != -1)
                {
                    if ((!compareData) || (compareData &&
                        CompareTwoFiles(Path.Combine(firstDirectoryParent, file), Path.Combine(secondDirectoryParent, secondDirectoryTree[index]))))
                    {
                        secondDirectoryTree.RemoveAt(index);
                        if (compareStructure)
                            both.Add(file);
                        else
                            both.Add(Path.GetFileName(file));
                    }
                }
                else
                {
                    first.Add(Path.Combine(firstDirectoryParent, file));
                }
            }

            foreach (string file in secondDirectoryTree)
            {
                second.Add(Path.Combine(secondDirectoryParent, file));
            }

            return new Tuple<List<string>, List<string>, List<string>>(both, first, second);
        }

        private static int IndexOf(List<string> list, string value, bool compareStructure)
        {
            if (compareStructure)
                return list.IndexOf(value);

            string shortVal;

            for (int i = 0; i < list.Count; i++)
            {
                shortVal = Path.GetFileName(list[i]);
                if (shortVal.StartsWith("IMG_20160407"))
                    Console.WriteLine();
                if (String.Equals(value, shortVal, StringComparison.OrdinalIgnoreCase))
                    return i;
            }

            return -1;
        }

        private bool CompareTwoFiles(string firstFilePath, string secondFilePath)
        {
            if (firstFilePath == secondFilePath)
                return true;

            FileInfo firstFileInfo = new FileInfo(firstFilePath);
            FileInfo secondFileInfo = new FileInfo(secondFilePath);

            if (firstFileInfo.Length != secondFileInfo.Length)
                return false;

            return CompareData(firstFileInfo, secondFileInfo);
        }

        private bool CompareData(FileInfo firstFileInfo, FileInfo secondFileInfo)
        {
            int iterations = (int)Math.Ceiling((double)firstFileInfo.Length / BytesToRead);

            using (FileStream fs1 = firstFileInfo.OpenRead())
            using (FileStream fs2 = secondFileInfo.OpenRead())
            {
                byte[] one = new byte[BytesToRead];
                byte[] two = new byte[BytesToRead];

                for (int i = 0; i < iterations; i++)
                {
                    fs1.Read(one, 0, BytesToRead);
                    fs2.Read(two, 0, BytesToRead);

                    if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
                        return false;
                }
            }

            return true;
        }

        private void CompareStructure_Changed(object sender, RoutedEventArgs e)
        {
            if (IsChecked(CompareStructure))
                CompareStructure.ToolTip = CompareStructureCheckedToolTipData;
            else
                CompareStructure.ToolTip = CompareStructureUnheckedToolTipData;
        }
        private void CompareFilesData_Changed(object sender, RoutedEventArgs e)
        {
            if (IsChecked(CompareFilesData))
                CompareFilesData.ToolTip = CompareDataCheckedToolTipData;
            else
                CompareFilesData.ToolTip = CompareDataUnheckedToolTipData;
        }
    }
}
