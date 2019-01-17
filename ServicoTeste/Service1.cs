using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using System.Windows.Forms;

namespace ServicoTeste
{
    public partial class Service1 : ServiceBase
    {
        public const string NAME = "Serviço de Testes";
        public const string DISPLAY_NAME = "Serviço de Testes";
        public const string DESCRIPTION = "TESTE";
        private bool pausa = false;
        private System.Timers.Timer tmrCalcularMeta;

        public Service1()
        {
            InitializeComponent();
        }

        static void Main(string[] args)
        {

            try
            {
                Trace.WriteLine("Iniciando DEBUG Processo");
                Console.WriteLine("INICIADO AS " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);

                //regra
                ServicoPrincipal();

                Console.WriteLine("FINALIZADO AS " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
            }
            catch (Exception e)
            {
                Trace.WriteLine("Erro ao Executar Processo" + e.Message + "\n" + e.StackTrace);
            }

            //Verifica se a chamada do Serviço foi ou não chamado pelo usuário.
            //if (!Environment.UserInteractive)
            //{
            //    //Chamada pelo Sistema => Executa o Serviço
            //    ServiceBase[] ServicesToRun;
            //    ServicesToRun = new ServiceBase[] { new Service1() };
            //    ServiceBase.Run(ServicesToRun);
            //}
            //else
            //{
            //    //Chamada pelo Usuário => Instala ou Desinstala o Serviço
            //    ServiceController sc = new ServiceController(NAME);

            //    //if (!ServiceExists())
            //    //{
            //    //    if (DialogResult.OK ==
            //    //        MessageBox.Show("Deseja instalar o serviço " + DISPLAY_NAME + "?", DISPLAY_NAME
            //    //                        , MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
            //    //                        MessageBoxDefaultButton.Button2))
            //    //    {
            //    //        try
            //    //        {
            //    //            Trace.WriteLine("Instalando o serviço \"" + DISPLAY_NAME + "\"...");
            //    //            ProjectInstaller.Install();
            //    //        }
            //    //        catch (Exception ex)
            //    //        {
            //    //            Trace.TraceError(ex.Message);
            //    //        }
            //    //    }
            //    //}
            //    //else
            //    //{
            //    //    if (DialogResult.OK ==
            //    //        MessageBox.Show("Deseja desinstalar o serviço " + DISPLAY_NAME + "?", DISPLAY_NAME,
            //    //                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
            //    //                        MessageBoxDefaultButton.Button2))
            //    //    {
            //    //        try
            //    //        {
            //    //            Trace.WriteLine("Desinstalando o serviço \"" + DISPLAY_NAME + "\"...");
            //    //            ProjectInstaller.Uninstall();
            //    //        }
            //    //        catch (Exception ex)
            //    //        {
            //    //            Trace.TraceError(ex.Message);
            //    //        }
            //    //    }
            //    //}
            //}
        }

        static void ServicoPrincipal()
        {
            try
            {
                //MessageBox.Show("Impressora localizada em: " + impressora.usu_endimp + "\nImprimir da pasta: " + impressora.usu_locpas, DISPLAY_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                string[] arquivos = Directory.GetFiles(@"C:\testes");

                Console.WriteLine("Arquivos Excluídos:");

                StreamWriter vWriter = new StreamWriter(@"C:\testes\ArquivosExcluidos.txt", true);
                vWriter.WriteLine("[" + DateTime.Now.ToString() + "] - Arquivos Excluídos: ");

                foreach (string arq in arquivos)
                {
                    Console.WriteLine(arq);

                    vWriter.WriteLine(arq);

                    File.Delete(arq);
                }

                vWriter.Flush();
                vWriter.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected override void OnStart(string[] args)
        {

            /* 30000 é igual a trinta segundos;
             * 60000 é igual a sessenta segundos; 
             * 300000 é gual a cinco minutos; 
             * 420000 É igual a sete minutos;
             * 720000  É igual a doze minutos;
             * 900000 é gual a quinze minutos; 
             * 3600000 é igual a uma hora; 
             * 86400000 é igual a vinte e quatro horas; 
            */
            tmrCalcularMeta = new System.Timers.Timer(30000);//MEIA HORA
            tmrCalcularMeta.Elapsed += new ElapsedEventHandler(tmr_processo_principal_Elapsed);
            tmrCalcularMeta.Enabled = true;
            tmrCalcularMeta.AutoReset = true;
            tmrCalcularMeta.Start();
            //
        }

        protected override void OnStop()
        {
            StreamWriter vWriter = new StreamWriter(@"c:\testeServico.txt", true);

            vWriter.WriteLine("Servico Parado: " + DateTime.Now.ToString());
            vWriter.Flush();
            vWriter.Close();
        }

        private static bool ServiceExists()
        {
            foreach (ServiceController sc in ServiceController.GetServices())
                if (sc.ServiceName == NAME)
                    return true;
            return false;
        }

        private void tmr_processo_principal_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!pausa)
            {
                try
                {
                    pausa = true;

                    StreamWriter vWriter = new StreamWriter(@"C:\Users\jordan.rocha\Desktop\testeServico.txt", true);
                    vWriter.WriteLine("[" + DateTime.Now.ToString() + "] - Impressoras: ");
                    vWriter.Flush();
                    vWriter.Close();

                    pausa = false;
                }
                catch (Exception ex)
                {
                    eventLog1.WriteEntry(string.Format("Erro no Serviço: {0} "
                        , ex.Message)
                        , EventLogEntryType.Information);
                }
            }
        }
    }
}
