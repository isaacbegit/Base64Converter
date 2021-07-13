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
                    Clipboard.SetText(base64String);
                    MessageBox.Show("Done! \n Open the application on the other host to receive it ", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please select a file ","Select file",MessageBoxButtons.OK,MessageBoxIcon.Error );
                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private  void EncodeWithConvertToBase64(byte[] inputBytes, string targetFile)
        {
            string fileString = Convert.ToBase64String(inputBytes);

            using (StreamWriter output = new StreamWriter(targetFile))
            {
                output.Write(fileString);
                output.Close();
            }
        }


        public  byte[] ReadFile(string filePath)
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
               
             

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = @"C:\";      
                saveFileDialog1.Title = "Save  File";
                saveFileDialog1.CheckFileExists = false ;
               
                
                saveFileDialog1.Filter = "Text files (All files (*.*)|*.*";
               
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                {
                    return;
                }


                //  var b64Str = Clipboard.GetText();
                var b64Str = Clipboard.GetText();
                Byte[] bytes = Convert.FromBase64String(b64Str);
                string filePath = saveFileDialog1.FileName;
                File.WriteAllBytes(filePath, bytes);

          DialogResult  result=MessageBox.Show("The file received successfully would you open file location? ","Open file location?", MessageBoxButtons.YesNo,MessageBoxIcon.Question );
           if (result==DialogResult.Yes  )
                {

                    if (File.Exists(filePath))
                    {
                        Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + filePath ));
                    }
                }
            
            }
            catch (Exception)
            {

                MessageBox.Show("Can't receive this file ");
            }
        }
    }
}
