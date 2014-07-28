using System;
using System.Windows.Forms;

namespace RedDog.ServiceBus.EventHubs.SignatureGenerator
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            InitializeView();
        }

        private void InitializeView()
        {
            comboMode.SelectedIndex = 0;
        }

        private void OnGenerate(object sender, System.EventArgs e)
        {
            try
            {
                if (comboMode.Text == "Http")
                {
                    textSignature.Text =
                        EventHubSharedAccessSignature.CreateForHttpSender(textSenderKeyName.Text, textSenderKey.Text,
                            textNamespace.Text, textHubName.Text, textPublisher.Text, TimeSpan.FromMinutes(double.Parse(textTTL.Text)));
                }
                else
                {
                    textSignature.Text =
                        EventHubSharedAccessSignature.CreateForSender(textSenderKeyName.Text, textSenderKey.Text,
                            textNamespace.Text, textHubName.Text, textPublisher.Text, TimeSpan.FromMinutes(double.Parse(textTTL.Text)));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
