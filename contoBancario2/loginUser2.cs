using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace contoBancario2
{
    public partial class loginUser2 : Form
    {
        public string userStore;

        public loginUser2()
        {
            InitializeComponent();
        }

        public class UserConfig
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Saldo { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string percorsoLogin = @".\TextFiles\config.json";

         
            string jsonContent = File.ReadAllText(percorsoLogin);

            UserConfig config = JsonConvert.DeserializeObject<UserConfig>(jsonContent);

            if (config.Username == username.Text && config.Password == password.Text)
            {
                userStore = config.Username;

                MessageBox.Show("Benvenuto " + userStore, "Login Avvenuto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                // Nascondi la form di login e mostra la MainPage2
                loginUser2.ActiveForm.Hide();

                // Passa il nome utente alla nuova form MainPage2 tramite il costruttore
                MainPage2 mainPage = new MainPage2();
                mainPage.Saldo = config.Saldo;
                mainPage.Show();
            }
            else
            {
                // Mostra il messaggio di errore se il login fallisce
                MessageBox.Show("Impossibile Accedere", "Errore", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);

                // Pulisci i campi di input
                username.Clear();
                password.Clear();
            }
        }
    }
}
