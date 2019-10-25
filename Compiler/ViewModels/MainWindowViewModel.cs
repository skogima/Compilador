using ICSharpCode.AvalonEdit.Document;
using System.IO;
using System.Windows.Forms;

namespace Compiler
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Commandos
        public RelayCommand AnalisarCommand { get; set; }
        public RelayCommand SalvarArquivoCommand { get; set; }
        public RelayCommand ProcurarArquivoCommand { get; set; }
        #endregion

        private AnaLexico analexico = new AnaLexico();
        public string LexicoResult { get; set; }
        public TextDocument CodeDocument { get; set; }

        public MainWindowViewModel()
        {
            CodeDocument = new TextDocument();
            AnalisarCommand = new RelayCommand(Analisar);
            SalvarArquivoCommand = new RelayCommand(Salvar);
            ProcurarArquivoCommand = new RelayCommand(Abrir);
        }

        private void Analisar()
        {
            if (string.IsNullOrEmpty(CodeDocument.Text))
                return;

            try
            {
                var result = analexico.Analisar(CodeDocument.Text);
                LexicoResult = string.Empty;
                result.ForEach(x => LexicoResult += x + " ");

                Sintatico sintatico = new Sintatico(result);
                sintatico.Analisar();
            }
            catch (LexicoException ex)
            {
                LexicoResult = $"{ex.Message} ({ex.Caracter}) na linha {ex.Linha}";
            }
            catch (SintaticoException ex)
            {
                LexicoResult = $"{ex.Message}";
            }
        }

        #region Command Actions
        private void Salvar()
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Código C|*.c|Todos os arquivos|*.*";
                var result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    using (FileStream fs = new FileStream(dialog.FileName, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(fs))
                        {
                            CodeDocument.WriteTextTo(writer);
                        }
                    }
                }
            }
        }
        private void Abrir()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Codigo C (*.c)|*.c|Todos os arquivos (*.*)|*.*";
                var result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    using (FileStream fs = new FileStream(dialog.FileName, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(fs))
                        {
                            CodeDocument.Text = reader.ReadToEnd();
                        }
                    }
                }
            }
        }
        #endregion
    }
}