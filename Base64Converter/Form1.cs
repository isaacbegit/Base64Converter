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

namespace Base64Converter
{
    public partial class Form1 : Form
    {

        private string filePath = null;
        private string outputDirectoryPath = null;
        public Form1()
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
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {

                    txt_outputFile.Text = fbd.SelectedPath.ToString();
                    outputDirectoryPath = fbd.SelectedPath.ToString();
                    // System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {

                if (File.Exists(filePath))
                {
                    var bytes = File.ReadAllBytes(filePath);
                    var base64String = Convert.ToBase64String(bytes);
                    Clipboard.SetText(base64String);
                    MessageBox.Show("Done ");
                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static void EncodeWithConvertToBase64(byte[] inputBytes, string targetFile)
        {
            string fileString = Convert.ToBase64String(inputBytes);

            using (StreamWriter output = new StreamWriter(targetFile))
            {
                output.Write(fileString);
                output.Close();
            }
        }


        public static byte[] ReadFile(string filePath)
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

        private void btn_convert_tofile_Click(object sender, EventArgs e)
        {
            try
            {
                var b64Str = Clipboard.GetText();
              
                Byte[] bytes = Convert.FromBase64String(b64Str);
                string filePath = outputDirectoryPath + @"\" + txt_file_ext.Text.Trim();
                File.WriteAllBytes(filePath, bytes);

                MessageBox.Show("Converted !");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
