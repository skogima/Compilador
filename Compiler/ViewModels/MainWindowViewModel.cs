using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

        private AnaLexico analexico;
        private string arquivoNome;
        private bool isSaved = false;

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
        public string LexicoResult { get; set; }
        public ObservableCollection<Variaveis> VariaveisCollection { get; set; }
        public TextDocument CodeDocument { get; set; }
        public WindowViewModel WindowProperties { get; set; }

        public MainWindowViewModel(Window window)
        {
            CodeDocument = new TextDocument();
            AnalisarCommand = new RelayCommand(Analisar);
            SalvarArquivoCommand = new RelayCommand(Salvar);
            ProcurarArquivoCommand = new RelayCommand(Abrir);
            WindowProperties = new WindowViewModel(window);
            VariaveisCollection = new ObservableCollection<Variaveis>();
        }

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
            }
            catch (LexicoException ex)
            {
                LexicoResult = $"{ex.Message} ({ex.Caracter}) na linha {ex.Linha}";
            }
            catch (SintaticoException ex)
            {
                LexicoResult = ex.Message;
            }
            catch (Exception ex)
            {
                LexicoResult = ex.Message;
            }
        }

        #region Command Actions
        private void Salvar()
        {
            string filePath = string.Empty;
            if (isSaved)
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

            if (string.IsNullOrEmpty(filePath))
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {

                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        CodeDocument.WriteTextTo(writer);
                    }
                }
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
                            ArquivoNome = dialog.FileName;
                            isSaved = true;
                        }
                    }
                }
            }
        }
        #endregion
    }
}