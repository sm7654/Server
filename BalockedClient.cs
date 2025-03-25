using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerSide
{
    public partial class BalockedClient : UserControl
    {
        public BalockedClient(BlackGuest guest)
        {
            InitializeComponent();
            MotherBoardlbl.Text = guest.GEt_MotherBoard_SN();
            this.guest = guest;
        }
        
        private BlackGuest guest;
        public void AddAttempt()
        {
            ConnetAttemps.Text = $"{guest.AddAttempt()}";
        }
        public string Get_MotherBoard_SN()
        {
            return this.guest.GEt_MotherBoard_SN();
        }
        
        private void ConnetAttemps_Click(object sender, EventArgs e)
        {

        }
    }
}
