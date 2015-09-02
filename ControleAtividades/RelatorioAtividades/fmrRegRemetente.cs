using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Security.Permissions;

namespace RelatorioAtividades
{
    public partial class fmrRegRemetente : Form
    {
        private string caminhoArq = @"C:\ArquivoConfig.txt";
        private string emailRemetente;
        private string senha;
        private string retorno;
        private string EmailArq;
        private string SenhaArq;

        public fmrRegRemetente()
        {
            InitializeComponent();
        }

        //Metodos da Classe
        //Metodo que pegara o valor digitado de email e senha
        private void RecebeRemetente()
        {
            emailRemetente = this.txtEmailRemetente.Text;
            senha = this.txtSenhaRemetente.Text;
        }
        private void GravaArquivo(string Email, string Senha)
        {
            try
            {
                //Escreve ou cria Arquivo
                string EndAplicacao = Directory.GetCurrentDirectory() + @"\ArquivoConfig";
                //FileIOPermission FilePermission = new FileIOPermission(FileIOPermissionAccess.PathDiscovery, EndAplicacao);
                //FilePermission.Assert();
                StreamWriter ArquivoConfig = new StreamWriter(EndAplicacao);
                string Dados = string.Format("{0} {1}", Email, Senha);
                ArquivoConfig.WriteLine(Dados);
                ArquivoConfig.Close();
                MessageBox.Show("Gravado com sucesso!");
            }catch(Exception Erro)
            {
                MessageBox.Show(Erro.Message);
            }

        }
        private void LeArquivo(string Caminho)
        {
        }

        public static void UpdateAppSettings(string chave, string valor)
        {

            // Open App.Config of executable

            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // Add an Application Setting.
            config.AppSettings.Settings.Remove(chave);

            config.AppSettings.Settings.Add(chave, valor);

            // Save the configuration file.
            config.Save(ConfigurationSaveMode.Modified);

            // Force a reload of a changed section.
            ConfigurationManager.RefreshSection("appSettings");
        }


        private void btnSalvaRemetente_Click(object sender, EventArgs e)
        {
            //Verifica se o endereço de email tem @ e se a senha não esta vazia
            if (this.txtEmailRemetente.Text.Contains("@") && this.txtSenhaRemetente.Text != "")
            {
                RecebeRemetente();
                GravaArquivo(emailRemetente, senha);
            }
            else
            {
                //Verifica se o email é invalido
                if (!this.txtEmailRemetente.Text.Contains("@"))
                {
                    MessageBox.Show(string.Format(" O email {0} Digitado é invalido ", this.txtEmailRemetente.Text));
                    this.txtEmailRemetente.Focus();
                    return;
                }
                //Verifica se o campo senha foi preenchido.
                if (this.txtSenhaRemetente.Text == "")
                {
                    MessageBox.Show(string.Format("Digite sua senha"));
                    this.txtSenhaRemetente.Focus();
                    return;
                }
            }
            Close();
        }

        private void fmrRegRemetente_Load(object sender, EventArgs e)
        {
        }
    }
}
