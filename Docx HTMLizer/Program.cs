namespace Docx_HTMLizer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            string[] arguments = Environment.GetCommandLineArgs();

            if (arguments.Length > 1)
            {
                string filename = "";

                foreach (var arg in Environment.GetCommandLineArgs().Skip(1))
                {
                    filename = filename + arg;
                }

                if (File.Exists(filename) && filename.EndsWith(".docx"))
                {
                    MessageBox.Show(filename, "Error opening file");
                }
            }

            Application.Run(new MainForm());
        }
    }
}