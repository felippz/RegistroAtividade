using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Exchange.WebServices.Data;
using System.Collections;
using System.Net.Mail;
using System.IO;

namespace RelatorioAtividades
{
    public partial class fmrEnvioEmail : Form
    {
        //Instancia publica
        //Form1 RegAtividade = new Form1();

        //Variaveis da Classe
        private string nomeUsuario = "luiz.felipe@deak.com.br";
        private string senha = "Sabrina100207";
        private string emailDestino; //"owlflee@gmail.com";
        private string assunto;
        private string textoEmail;
        private string recuperaTexto;
        private string textoEmailTratado;
        private int totalAtividades;
        private string retorno;
        private string EmailArq;
        private string SenhaArq;

        //Atributos da classe
        private ArrayList atRecuperaAtividade = new ArrayList();
        private ArrayList atTempoAtividade = new ArrayList();

        //Propriedades Classe
        private ArrayList PrRecuperaAtividade { get { return atRecuperaAtividade; } set { atRecuperaAtividade = value; } }
        private ArrayList PrTempoAtividade { get { return atTempoAtividade; } set { atTempoAtividade = value; } }

        //Metodo Construtor
        public fmrEnvioEmail(ArrayList registro, ArrayList tempo, int TotalTemp)
        {
            InitializeComponent();
            for (int conta = 0; conta < registro.Count; conta++)
            {
                PrRecuperaAtividade.Add(registro[conta]);
                PrTempoAtividade.Add(tempo[conta]);
            }
            totalAtividades = TotalTemp;
        }
        public fmrEnvioEmail()
        {
        }

        //Metodos do Form
        //Descoberta automática Dominio
        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }
        private void Envio()
        {
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
            service.Credentials = new WebCredentials(EmailArq, SenhaArq);
            service.TraceEnabled = true;
            service.TraceFlags = TraceFlags.All;
            service.UseDefaultCredentials = false;
            service.AutodiscoverUrl(EmailArq, RedirectionUrlValidationCallback);
            EmailMessage email = new EmailMessage(service);
            email.ToRecipients.Add(emailDestino);
            email.Subject = assunto;
            email.Body = new MessageBody(textoEmail);
            email.Send();
        }
        private void RecuperaAtividades()
        {
            string textoCab = string.Format("Atividades Realizadas:\n");
            string TextoCorp = "";
            for (int conta = 0; conta < PrRecuperaAtividade.Count; conta++)
            {
                TextoCorp += string.Format("{0}\nDuração: {1}\n", PrRecuperaAtividade[conta], PrTempoAtividade[conta]);
            }
            recuperaTexto = string.Format("{0}\n{1}\nDuração Total: {2}", textoCab, TextoCorp, totalAtividades);
        }
        private void TrataTextoEmail()
        {
            textoEmailTratado = string.Format(@"<pre>{0}</pre><br><br><br><br><br><font size=""1"">Enviado via Registro de Atividades</font> 
                                                <br> <font size=""1"">Desenvolvedor: Luiz felipe</font>
                                                <br> <font size=""1"">Contato: felipe_Luiz@icloud.com</font>", rtbTextoEmail.Text);
        }
        private void LeArquivo()
        {
            string EndAplicacao = Directory.GetCurrentDirectory() + @"\ArquivoConfig";
            StreamReader ArquivoConfig = new StreamReader(EndAplicacao);
            while (!ArquivoConfig.EndOfStream)
            {
                retorno = ArquivoConfig.ReadLine();
                bool continua = false;
                for (int conta = 0; conta < retorno.Length; conta++)
                {
                    if (Convert.ToString(retorno[conta]) == " ")
                    {
                        continua = true;
                    }
                    else if (Convert.ToString(retorno[conta]) != " " && continua == false)
                    {
                        EmailArq += retorno[conta];
                    }
                    else if (Convert.ToString(retorno[conta]) != " " && continua == true)
                    {
                        SenhaArq += retorno[conta];
                    }

                }
            }
        }

        //Eventos Do Form
        private void EnvioEmail_Load(object sender, EventArgs e)
        {
            RecuperaAtividades();
            rtbTextoEmail.Text = recuperaTexto;
            this.txtDestinatario.Focus();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                LeArquivo();
                TrataTextoEmail();
                if (this.txtDestinatario.Text == "" || !this.txtDestinatario.Text.Contains("@"))
                {
                    MessageBox.Show(string.Format("O E-mail {0}, digitado é invalido", this.txtDestinatario.Text));
                    this.txtDestinatario.Focus();
                    return;
                }
                emailDestino = this.txtDestinatario.Text;
                assunto = this.txtAssunto.Text;
                textoEmail = textoEmailTratado.ToUpper();
                Envio();
                MessageBox.Show("Email enviado com Sucesso");
                Close();
            }
            catch (Exception erroMail)
            {
                MessageBox.Show(erroMail.Message);
            }
        }

        private void txtDestinatario_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
