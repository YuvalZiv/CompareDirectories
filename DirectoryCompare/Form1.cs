using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class MainForm : Form
    {
        private const int BYTES_TO_READ = sizeof(Int64);
        private const string SummaryOutputFormat = "First Directory:\r\nFiles - {0}\r\nSub Directories - {1}\r\nSize - {2}B\r\n\r\nSecond Directory:\r\nFiles - {3}\r\nSub Directories - {4}\r\nSize - {5}B";
        private const string FoundOutputFormat = "Found in both directories:\r\n{0}";
        private const string NotFoundOutputFormat = "Not found in first directory:\r\n{0}\r\n\r\nNot found in second directory:\r\n{1}";
        private const string ConclustionExistsAll = "All {0} files exists in both directories.";
        private const string ConclustionNotAllExists = "{0} files from the first directory are missing in the second directory.\r\n{1} files from the second directory are missing in the first directory.";

        Dictionary<Button, TextBox> BrowseButtonMatchingTextBox = new Dictionary<Button, TextBox>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            BrowseButtonMatchingTextBox.Add(FirstBrowse, FirstDirectoryPath);
            BrowseButtonMatchingTextBox.Add(SecondBrowse, SecondDirectoryPath);
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            if (Browser.ShowDialog() == DialogResult.OK)
            {
                BrowseButtonMatchingTextBox[(Button)sender].Text = Browser.SelectedPath;
            }
        }

        private void Compare_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(FirstDirectoryPath.Text) || String.IsNullOrWhiteSpace(SecondDirectoryPath.Text))
                MessageBox.Show("Empty folder path");

            if (string.Equals(FirstDirectoryPath.Text, SecondDirectoryPath.Text, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Please enter different directory paths.");
                return;
            }

            ShowStats(FirstDirectoryPath.Text, SecondDirectoryPath.Text);                      
            List<string> existsInBoth = new List<string>();
            List<string> existsInFirstOnly = new List<string>();
            List<string> existsInSecondOnly = new List<string>();
            CompareTwoLists(FirstDirectoryPath.Text, SecondDirectoryPath.Text, TreeFolder(FirstDirectoryPath.Text), TreeFolder(SecondDirectoryPath.Text), ref existsInBoth, ref existsInFirstOnly, ref existsInSecondOnly);
            ShowCompare(existsInBoth, existsInFirstOnly, existsInSecondOnly);
        }

        private void ShowStats(string firstDirectoryPath, string secondDirectoryPath)
        {
            long firstSizeInBytes = 0, secondSizeInBytes = 0;
            int firstSubDirectoriesCount = 0, firstFilesCount = 0, secondSubDirectoriesCount = 0, secondFilesCount = 0;
            GetStats(firstDirectoryPath, ref firstSizeInBytes, ref firstSubDirectoriesCount, ref firstFilesCount);
            GetStats(secondDirectoryPath, ref secondSizeInBytes, ref secondSubDirectoriesCount, ref secondFilesCount);
            SummaryOutput.Text = String.Format(SummaryOutputFormat, firstFilesCount, firstSubDirectoriesCount, firstSizeInBytes, secondFilesCount, secondSubDirectoriesCount, secondSizeInBytes);
            SummaryOutput.ForeColor = Color.Black;
        }

        private void GetStats(string folderPath, ref long sizeInBytes, ref int subDirectoriesCount, ref int filesCount)
        {
            if (!(Directory.Exists(folderPath)))
                MessageBox.Show("Unknown directory at " + folderPath);

            foreach (string absoluteSubDirectory in Directory.GetDirectories(folderPath))
            {
                GetStats(absoluteSubDirectory, ref sizeInBytes, ref subDirectoriesCount, ref filesCount);
                subDirectoriesCount++;
            }

            foreach (string absoluteFilePath in Directory.GetFiles(folderPath))
            {
                filesCount++;
                sizeInBytes += new FileInfo(absoluteFilePath).Length;
            }
        }

        private List<string> TreeFolder(string parentDirectoryPath)
        {
            return TreeFolder(parentDirectoryPath, parentDirectoryPath);
        }

        private List<string> TreeFolder(string parentDirectoryPath, string directoryPath)
        {
            List<string> files = new List<string>();

            foreach (string absoluteSubDirectoryPath in Directory.GetDirectories(directoryPath))
            {
                files.AddRange(TreeFolder(parentDirectoryPath, absoluteSubDirectoryPath));
            }

            foreach (string absoluteFilePath in Directory.GetFiles(directoryPath))
            {
                files.Add(absoluteFilePath.Remove(0, parentDirectoryPath.Length));
            }

            return files;
        }

        private void CompareTwoLists(string firstDirectoryParent, string secondDirectoryParent, List<string> firstDirectoryTree, List<string> secondDirectoryTree, ref List<string> existsInBoth, ref List<string> existsInFirst, ref List<string> existsInSecond)
        {
            foreach (string file in firstDirectoryTree)
            {
                if (secondDirectoryTree.Contains(file) && CompareTwoFiles(firstDirectoryParent + file, secondDirectoryParent + file))
                {
                    secondDirectoryTree.Remove(file);
                    existsInBoth.Add(file);
                }
                else
                {
                    existsInFirst.Add(file);
                }
            }

            foreach (string file in secondDirectoryTree)
            {
                existsInSecond.Add(file);
            }
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
            int iterations = (int)Math.Ceiling((double)firstFileInfo.Length / BYTES_TO_READ);

            using (FileStream fs1 = firstFileInfo.OpenRead())
            using (FileStream fs2 = secondFileInfo.OpenRead())
            {
                byte[] one = new byte[BYTES_TO_READ];
                byte[] two = new byte[BYTES_TO_READ];

                for (int i = 0; i < iterations; i++)
                {
                    fs1.Read(one, 0, BYTES_TO_READ);
                    fs2.Read(two, 0, BYTES_TO_READ);

                    if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
                        return false;
                }
            }

            return true;
        }

        private void ShowCompare(List<string> existsInBoth, List<string> existsInFirstOnly, List<string> existsInSecondOnly)
        {
            FoundOutput.Text = String.Format(FoundOutputFormat, String.Join("\r\n- ", existsInBoth));
            NotFoundOutput.Text = String.Format(NotFoundOutputFormat, String.Join("\r\n- ", existsInFirstOnly), String.Join("\r\n- ", existsInSecondOnly));

            if ((existsInFirstOnly.Count == 0) && (existsInSecondOnly.Count == 0))
            {
                Conclusion.Text = String.Format(ConclustionExistsAll, existsInBoth.Count);
            }
            else
            {
                Conclusion.Text = String.Format(ConclustionNotAllExists, existsInFirstOnly.Count, existsInSecondOnly.Count);
            }

            FoundOutput.ForeColor = Color.Black;
            NotFoundOutput.ForeColor = Color.Black;
            Conclusion.ForeColor = Color.Black;
        }
    }
}
