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
    public partial class BlockedClient : UserControl
    {
        public BlockedClient(BadGuest guest)
        {
            InitializeComponent();
            MotherBoardlbl.Text = guest.GEt_MotherBoard_SN();
            this.guest = guest;
        }
        
        private BadGuest guest;
        public void AddAttempt()
        {
            ConnetAttemps.Text = $"{guest.AddAttempt()}";
        }
        public string Get_MotherBoard_SN()
        {
            return this.guest.GEt_MotherBoard_SN();
        }

        private void DateOfBlock_Click(object sender, EventArgs e)
        {

        }
    }
}
