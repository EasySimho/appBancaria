using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace contoBancario2
{
    public partial class loginUser2 : Form
    {
        public string userStore;

        public loginUser2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            userStore = username.Text;
            MessageBox.Show("Benvenuto " + userStore);
            loginUser2.ActiveForm.Hide();
            new Form().ShowDialog();
            
        }
    }
}
