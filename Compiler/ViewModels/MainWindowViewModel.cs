using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
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

        #region Privates
        private AnaLexico analexico;
        private string arquivoNome;
        private string filePath = string.Empty;
        private bool isSaved = false;
        #endregion

        #region Propriedades
        public string ArquivoNome
        {
            get
            {
                if (string.IsNullOrEmpty(arquivoNome))
                    return "main.c";

                return arquivoNome;
            }
            set
            {
                if (!value.EndsWith(".c"))
                    value = value + ".c";
                arquivoNome = value;
            }
        }
        public string CompilerResult { get; set; }
        public ObservableCollection<Variaveis> VariaveisCollection { get; set; }
        public TextDocument CodeDocument { get; set; }
        public WindowViewModel WindowProperties { get; set; }
        #endregion

        public MainWindowViewModel(Window window)
        {
            CodeDocument = new TextDocument();
            AnalisarCommand = new RelayCommand(Analisar);
            SalvarArquivoCommand = new RelayCommand(Salvar);
            ProcurarArquivoCommand = new RelayCommand(Abrir);
            WindowProperties = new WindowViewModel(window);
            VariaveisCollection = new ObservableCollection<Variaveis>();
        }

        #region Command Actions

        private void Analisar()
        {
            if (string.IsNullOrEmpty(CodeDocument.Text))
                return;

            try
            {
                analexico = new AnaLexico(CodeDocument.Text);
                var result = analexico.Analisar();

                Sintatico sintatico = new Sintatico(result);
                ArvoreNo no = sintatico.Analisar();

                Semantico semantico = new Semantico();
                var x = no.GetValor(semantico);

                var list = x as List<Variaveis>;
                VariaveisCollection.Clear();

                foreach (var item in list)
                {
                    VariaveisCollection.Add(item);
                }

                CompilerResult = "Compilação bem-sucedida.";
            }
            catch (LexicoException ex)
            {
                CompilerResult = $"{ex.Message} ({ex.Caracter}) na linha {ex.Linha}";
            }
            catch (SintaticoException ex)
            {
                CompilerResult = ex.Message;
            }
            catch (SemanticoException ex)
            {
                CompilerResult = ex.Message;
            }
        }

        private void Salvar()
        {
            if (string.IsNullOrEmpty(filePath))
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Filter = "Código C|*.c|Todos os arquivos|*.*";
                    dialog.FileName = ArquivoNome;
                    dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        isSaved = true;
                        filePath = dialog.FileName;
                    }
                }

            if (!string.IsNullOrEmpty(filePath))
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    using (StreamWriter writer = new StreamWriter(fs))
                        CodeDocument.WriteTextTo(writer);
        }
        private void Abrir()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Codigo C (*.c)|*.c|Todos os arquivos (*.*)|*.*";
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    using (FileStream fs = new FileStream(dialog.FileName, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(fs))
                        {
                            CodeDocument.Text = reader.ReadToEnd();
                            filePath = dialog.FileName;
                            ArquivoNome = Path.GetFileName(dialog.FileName);
                        }
                    }
                }
            }
        }
        #endregion
    }
}