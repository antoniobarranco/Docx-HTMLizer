using Mammoth;
using AngleSharp.Html.Parser;
using AngleSharp.Html;
using System.Security;

namespace Docx_HTMLizer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        static string BeautyHTML(string xml)
        {
            var parser = new HtmlParser();

            var document = parser.ParseDocument(xml);

            var sw = new StringWriter();
            document.ToHtml(sw, new PrettyMarkupFormatter());


            var salida = sw.ToString();
            salida = salida.Replace("\n", Environment.NewLine);
            salida = salida.Replace("\t", "    ");

            return salida;
        }

        void LoadFile(string docxFile)
        {
            try
            {
                var converter = new DocumentConverter().ImageConverter(image => {
                    using (var stream = image.GetStream())
                    {
                        var src = "";
                        return new Dictionary<string, string> { { "src", src } };
                    }
                });
                var result = converter.ConvertToHtml(docxFile);
                var html = result.Value;
                var warnings = result.Warnings;

                textBoxHtmlResult.Font = new Font(textBoxHtmlResult.Font.FontFamily, 9);
                textBoxHtmlResult.TextAlign = HorizontalAlignment.Left;
                textBoxHtmlResult.Enabled = true;

                textBoxHtmlResult.Text = BeautyHTML(html);
                toolStripButtonSave.Enabled = true;
                ToolStripMenuItemSave.Enabled = true;
            }
            catch
            {
                MessageBox.Show("Error loading file: Perhaps file is open with another app.", "Error opening file");
            }
        }

        void WriteFile(string htmlFile)
        {
            try
            {
                File.WriteAllText(htmlFile, textBoxHtmlResult.Text);
            }
            catch
            {
                MessageBox.Show("Error saving file: Perhaps file is open with another app.", "Error savig file");
            }
        }

        void OpenFile ()
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    LoadFile(openFileDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("Error loading file: Perhaps file is open with another app.", "Error opening file");
                }
            }
        }

        void SaveFile()
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    WriteFile(saveFileDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("Error saving file: Perhaps file is open with another app.", "Error savig file");
                }
            }
        }

        private void ToolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void ToolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void ToolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void toolStripButtonAbout_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void ToolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {

            string[] filename = (string[])e.Data.GetData(DataFormats.FileDrop);
            string selectedFilename = filename[0];
            if (selectedFilename.EndsWith(".docx"))
            {
                LoadFile(filename[0]);
            }


        }
    }
}