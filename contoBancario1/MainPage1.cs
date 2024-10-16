using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace contoBancario1
{
    public partial class MainPage1 : Form
    {
        public string Saldo { get; set; }

        public MainPage1()
        {
            InitializeComponent();
        }

        // Classe che rappresenta la struttura del JSON
        public class UserConfig
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Saldo { get; set; }
        }

        private async void sendMoney_Click(object sender, EventArgs e)
        {
            string percorsoLogin = @"C:\Users\sbena\Source\Repos\EasySimho\appBancaria\contoBancario1\TextFiles\config.json";

            bool valid = (importoDaInviare.Text).All(char.IsDigit) && (importoDaInviare.Text != "") && (importoDaPrelevare.Text == "");
            bool valid2 = (importoDaPrelevare.Text).All(char.IsDigit) && (importoDaPrelevare.Text != "") && (importoDaInviare.Text == "");

            // Mostra la barra di avanzamento o messaggio di caricamento
            progressBar1.Visible = true;
            progressBar1.Style = ProgressBarStyle.Marquee;  // Imposta la barra in modalità indeterminata
            await Task.Delay(2000);  // Simula un ritardo di 2 secondi (2000ms)



            // Caso di invio denaro
            if (valid)
            {
                if (File.Exists(@"C:\Users\sbena\Source\Repos\EasySimho\appBancaria\CommonFiles\Scambio.txt"))
                {
                    MessageBox.Show("File Gia Esistente, Troppe Operazioni In Coda", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    double sendingImport = double.Parse(importoDaInviare.Text);

                    // Leggi il file JSON e deserializza
                    string jsonContent = File.ReadAllText(percorsoLogin);
                    UserConfig config = JsonConvert.DeserializeObject<UserConfig>(jsonContent);


                    File.Create(@"C:\Users\sbena\Source\Repos\EasySimho\appBancaria\CommonFiles\Scambio.txt");

                    File.WriteAllText(@"C:\Users\sbena\Source\Repos\EasySimho\appBancaria\CommonFiles\Scambio.txt", importoDaInviare.Text + " 1");
                    // Aggiungi l'importo al saldo
                    double saldoAttuale = double.Parse(config.Saldo);
                    saldoAttuale += sendingImport;
                    config.Saldo = saldoAttuale.ToString();

                    // Serializza di nuovo e salva nel file JSON
                    string jsonModificato = JsonConvert.SerializeObject(config, Formatting.Indented);
                    File.WriteAllText(percorsoLogin, jsonModificato);

                    // Aggiorna il campo del saldo visivamente
                    Saldo = config.Saldo;
                    saldo.Text = Saldo + "€";

                    MessageBox.Show("Denaro inviato con successo", "Operazione Avvenuta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    importoDaInviare.Text = string.Empty;
                }

            }
            // Caso di prelievo denaro
            else if (valid2)
            {
                double importoPrelevato = double.Parse(importoDaPrelevare.Text);

                if (double.Parse(Saldo) < importoPrelevato)
                {
                    MessageBox.Show("Saldo Insufficiente", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Leggi il file JSON e deserializza
                    string jsonContent = File.ReadAllText(percorsoLogin);
                    UserConfig config = JsonConvert.DeserializeObject<UserConfig>(jsonContent);

                    // Sottrai l'importo dal saldo
                    double saldoAttuale = double.Parse(config.Saldo);
                    saldoAttuale -= importoPrelevato;
                    config.Saldo = saldoAttuale.ToString();

                    // Serializza di nuovo e salva nel file JSON
                    string jsonModificato = JsonConvert.SerializeObject(config, Formatting.Indented);
                    File.WriteAllText(percorsoLogin, jsonModificato);

                    // Aggiorna il campo del saldo visivamente
                    Saldo = config.Saldo;
                    saldo.Text = Saldo + "€";

                    MessageBox.Show("Prelievo effettuato con successo", "Operazione Avvenuta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    importoDaPrelevare.Text = string.Empty;
                }
            }
            else
            {
                MessageBox.Show("Valore non valido", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                importoDaInviare.Text = string.Empty;
                importoDaPrelevare.Text = string.Empty;
            }

            // Nascondi la barra di caricamento una volta completata l'operazione
            progressBar1.Visible = false;
        }

        private void MainPage2_Load(object sender, EventArgs e)
        {
            saldo.Text = Saldo + "€";
            progressBar1.Visible = false;  // Nascondi la progress bar all'inizio
            Timer timer = new Timer();
            timer.Interval = (1 * 3000); // 3 sec
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (File.Exists(@"C:\Users\sbena\Source\Repos\EasySimho\appBancaria\CommonFiles\Scambio.txt"))
            {
                string contenuto = File.ReadAllText(@"C:\Users\sbena\Source\Repos\EasySimho\appBancaria\CommonFiles\Scambio.txt");
                string[] words = contenuto.Split(' ');
                double utente = double.Parse(words[1]);
                double importo = double.Parse(words[0]);
                if (utente == 2)
                {
                    string percorsoLogin = @"C:\Users\sbena\Source\Repos\EasySimho\appBancaria\contoBancario1\TextFiles\config.json";
                    string jsonContent = File.ReadAllText(percorsoLogin);
                    UserConfig config = JsonConvert.DeserializeObject<UserConfig>(jsonContent);

                    double saldoAttuale = double.Parse(config.Saldo);
                    saldoAttuale += importo;
                    config.Saldo = saldoAttuale.ToString();

                    // Serializza di nuovo e salva nel file JSON
                    string jsonModificato = JsonConvert.SerializeObject(config, Formatting.Indented);
                    File.WriteAllText(percorsoLogin, jsonModificato);

                    // Aggiorna il campo del saldo visivamente
                    Saldo = config.Saldo;
                    saldo.Text = Saldo + "€";

                    MessageBox.Show("Denaro ricevuto con successo", "Operazione Avvenuta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }
    }
}
