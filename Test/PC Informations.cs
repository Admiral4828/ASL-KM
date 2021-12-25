using ASL_KM.Manager;
using System;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            sysInformations informations = new sysInformations();
            dataGridView1.DataSource = informations.GetNetworkList();
            dataGridView2.DataSource = informations.ListBiosInfo();
            dataGridView3.DataSource = informations.ListBaseBoardInfo();
            dataGridView4.DataSource = informations.ListCPUInfo();
            dataGridView5.DataSource = informations.ListOSInfo();
            dataGridView6.DataSource = informations.DiskDriveInfo();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
