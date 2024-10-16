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
        private Timer timer;
        public string Saldo { get; set; }

        public MainPage1()
        {
            InitializeComponent();

            timer = new Timer();

            timer.Interval = (1 * 6000); // 3 sec
            timer.Tick += timer_Tick;
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
            string percorsoLogin = @".\TextFiles\config.json";

            bool valid = (importoDaInviare.Text).All(char.IsDigit) && (importoDaInviare.Text != "") && (importoDaPrelevare.Text == "");
            bool valid2 = (importoDaPrelevare.Text).All(char.IsDigit) && (importoDaPrelevare.Text != "") && (importoDaInviare.Text == "");

            // Mostra la barra di avanzamento o messaggio di caricamento
            progressBar1.Visible = true;
            progressBar1.Style = ProgressBarStyle.Marquee;  // Imposta la barra in modalità indeterminata
            await Task.Delay(2000);  // Simula un ritardo di 2 secondi (2000ms)



            // Caso di invio denaro
            if (valid)
            {
                if (File.Exists(@"..\CommonFiles\Scambio.txt"))
                {
                    MessageBox.Show("File Gia Esistente, Troppe Operazioni In Coda", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    double importoInviato = double.Parse(importoDaInviare.Text);
                    if (double.Parse(Saldo) < importoInviato)
                    {
                        MessageBox.Show("Saldo Insufficiente", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {


                        double sendingImport = double.Parse(importoDaInviare.Text);

                        // Leggi il file JSON e deserializza
                        string jsonContent = File.ReadAllText(percorsoLogin);
                        UserConfig config = JsonConvert.DeserializeObject<UserConfig>(jsonContent);


                        File.WriteAllText(@"..\CommonFiles\Scambio.txt", importoDaInviare.Text + " 1");

                        // Aggiungi l'importo al saldo
                        double saldoAttuale = double.Parse(config.Saldo);
                        saldoAttuale -= sendingImport;
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

            }
            // Caso di prelievo denaro
            else if (valid2)
            {
                double importoPrelevato = double.Parse(importoDaPrelevare.Text);


                // Leggi il file JSON e deserializza
                string jsonContent = File.ReadAllText(percorsoLogin);
                UserConfig config = JsonConvert.DeserializeObject<UserConfig>(jsonContent);

                // Sottrai l'importo dal saldo
                double saldoAttuale = double.Parse(config.Saldo);
                saldoAttuale += importoPrelevato;
                config.Saldo = saldoAttuale.ToString();

                // Serializza di nuovo e salva nel file JSON
                string jsonModificato = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(percorsoLogin, jsonModificato);

                // Aggiorna il campo del saldo visivamente
                Saldo = config.Saldo;
                saldo.Text = Saldo + "€";

                MessageBox.Show("Ricarica effettuata con successo", "Operazione Avvenuta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                importoDaPrelevare.Text = string.Empty;

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
        private void MainPage1_Load(object sender, EventArgs e)
        {
            saldo.Text = Saldo + "€";
            progressBar1.Visible = false;  // Nascondi la progress bar all'inizio
            
            timer.Start();

        }
        private void timer_Tick(object sender, EventArgs e)
        {
            string filePath = @"..\CommonFiles\Scambio.txt";

            // Controlla se il file esiste
            if (File.Exists(filePath))
            {
                try
                {
                    // Leggi il contenuto del file
                    string contenuto = File.ReadAllText(filePath);
                    string[] words = contenuto.Split(' ');

                    if (words.Length == 2 && double.TryParse(words[1], out double utente) && double.TryParse(words[0], out double importo))
                    {
                        if (utente == 2)
                        {
                            string percorsoLogin = @".\TextFiles\config.json";
                            string jsonContent = File.ReadAllText(percorsoLogin);
                            UserConfig config = JsonConvert.DeserializeObject<UserConfig>(jsonContent);

                            // Aggiungi l'importo al saldo
                            double saldoAttuale = double.Parse(config.Saldo);
                            saldoAttuale += importo;
                            config.Saldo = saldoAttuale.ToString();

                            // Serializza e salva nel file JSON
                            string jsonModificato = JsonConvert.SerializeObject(config, Formatting.Indented);
                            File.WriteAllText(percorsoLogin, jsonModificato);

                            // Aggiorna il saldo visivamente
                            Saldo = config.Saldo;
                            saldo.Text = Saldo + "€";

                            // Notifica l'utente
                            MessageBox.Show("Denaro ricevuto con successo", "Operazione Avvenuta", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Elimina il file dopo l'operazione
                            File.Delete(filePath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Gestisci eventuali errori di lettura/scrittura file
                    MessageBox.Show("Errore nella gestione del file: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
