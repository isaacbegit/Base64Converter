using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Base64Converter
{
    public partial class Mainform : Form
    {
        private string filePath = null;
        private string outputDirectoryPath = null;

        public Mainform()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse  Files",

                CheckFileExists = true,
                CheckPathExists = true,

                Filter = "All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                filePath = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var bytes = File.ReadAllBytes(filePath);
                    var base64String = Convert.ToBase64String(bytes);

                    var clipboardStr = StringCompression.Compress(base64String);
                    Clipboard.SetText(clipboardStr);
                    MessageBox.Show("Done! \n Open the application on the other host to receive it ", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please select a file ", "Select file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EncodeWithConvertToBase64(byte[] inputBytes, string targetFile)
        {
            string fileString = Convert.ToBase64String(inputBytes);

            using (StreamWriter output = new StreamWriter(targetFile))
            {
                output.Write(fileString);
                output.Close();
            }
        }

        public byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

        private async void btn_convert_tofile_Click(object sender, EventArgs e)
        {
            try
            {
                var saveFileDialog1 = new SaveFileDialog()
                {
                    Title = "Save  File",
                    CheckFileExists = false,
                    Filter = "Text files (All files (*.*)|*.*",
                    RestoreDirectory = true
                };
                if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                // Receiving file
                string fileName = saveFileDialog1.FileName;
                string msg = string.Empty;
                progressBar1.Visible = true;
                tabControl1.Enabled = false;
                lb_info.Text = "Receiving file data,please wait...";
                var complited = GenerateFileFromCP(fileName, out msg);
                progressBar1.Visible = false;
                tabControl1.Enabled = true;
                lb_info.Text = "";
                if (complited)
                {
                    DialogResult result = MessageBox.Show("The file received successfully would you open file location? ", "Open file location?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        if (File.Exists(filePath))
                        {
                            Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + filePath));
                        }
                    }
                }
                else
                {
                    lb_info.Text = "Error!:" + msg;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Can't receive this file ");
            }
        }

        private bool GenerateFileFromCP(string fileName, out string msg)
        {
            try
            {
                msg = string.Empty;
                var b64Str = Clipboard.GetText();
                var fromClipboardStr = StringCompression.Decompress(b64Str);
                Byte[] bytes = Convert.FromBase64String(fromClipboardStr);
                File.WriteAllBytes(fileName, bytes);
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }

        private void Mainform_Load(object sender, EventArgs e)
        {
            this.Text = "File Transfer Version:" + Application.ProductVersion;
        }
    }
}