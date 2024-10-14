using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace contoBancario2
{
    public partial class MainPage2 : Form
    {
        public MainPage2()
        {
            InitializeComponent();
        }

        private void sendMoney_Click(object sender, EventArgs e)
        {
            bool valid = (importoDaInviare.Text).All(char.IsDigit);
            if (valid)
            {
                double sendingImport = double.Parse(importoDaInviare.Text);
                if (File.Exists(@"C:\Users\simone.benanchietti\Source\Repos\EasySimho\appBancaria\appBancaria\TextFiles\"))
                {
                    MessageBox.Show("File di scambio occupato", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    File.WriteAllText(@"C:\Users\simone.benanchietti\Source\Repos\EasySimho\appBancaria\appBancaria\TextFiles\", importoDaInviare.Text);
                }



                importoDaInviare.Text = string.Empty;
                destinatarioDaInviare.Text = string.Empty;
                passwordDaInviare.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Valore non valido", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                importoDaInviare.Text = string.Empty;
                destinatarioDaInviare.Text = string.Empty;
                passwordDaInviare.Text = string.Empty;
            }


        }
    }
}
