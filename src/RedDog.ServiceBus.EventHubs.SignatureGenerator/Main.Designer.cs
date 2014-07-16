namespace RedDog.ServiceBus.EventHubs.SignatureGenerator
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.groupHub = new System.Windows.Forms.GroupBox();
            this.comboMode = new System.Windows.Forms.ComboBox();
            this.labelMode = new System.Windows.Forms.Label();
            this.textPublisher = new System.Windows.Forms.TextBox();
            this.labelPublisher = new System.Windows.Forms.Label();
            this.textHubName = new System.Windows.Forms.TextBox();
            this.labelHubName = new System.Windows.Forms.Label();
            this.textNamespace = new System.Windows.Forms.TextBox();
            this.labelNamespace = new System.Windows.Forms.Label();
            this.groupCredentials = new System.Windows.Forms.GroupBox();
            this.textTTL = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textSenderKey = new System.Windows.Forms.TextBox();
            this.labelSenderKeyName = new System.Windows.Forms.Label();
            this.textSenderKeyName = new System.Windows.Forms.TextBox();
            this.labelSenderKey = new System.Windows.Forms.Label();
            this.groupSignature = new System.Windows.Forms.GroupBox();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.textSignature = new System.Windows.Forms.TextBox();
            this.groupHub.SuspendLayout();
            this.groupCredentials.SuspendLayout();
            this.groupSignature.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupHub
            // 
            this.groupHub.Controls.Add(this.comboMode);
            this.groupHub.Controls.Add(this.labelMode);
            this.groupHub.Controls.Add(this.textPublisher);
            this.groupHub.Controls.Add(this.labelPublisher);
            this.groupHub.Controls.Add(this.textHubName);
            this.groupHub.Controls.Add(this.labelHubName);
            this.groupHub.Controls.Add(this.textNamespace);
            this.groupHub.Controls.Add(this.labelNamespace);
            this.groupHub.Location = new System.Drawing.Point(13, 13);
            this.groupHub.Name = "groupHub";
            this.groupHub.Size = new System.Drawing.Size(402, 151);
            this.groupHub.TabIndex = 0;
            this.groupHub.TabStop = false;
            this.groupHub.Text = "Hub";
            // 
            // comboMode
            // 
            this.comboMode.FormattingEnabled = true;
            this.comboMode.Items.AddRange(new object[] {
            "Http",
            "AMQP"});
            this.comboMode.Location = new System.Drawing.Point(111, 105);
            this.comboMode.Name = "comboMode";
            this.comboMode.Size = new System.Drawing.Size(270, 21);
            this.comboMode.TabIndex = 7;
            // 
            // labelMode
            // 
            this.labelMode.AutoSize = true;
            this.labelMode.Location = new System.Drawing.Point(18, 108);
            this.labelMode.Name = "labelMode";
            this.labelMode.Size = new System.Drawing.Size(34, 13);
            this.labelMode.TabIndex = 6;
            this.labelMode.Text = "Mode";
            // 
            // textPublisher
            // 
            this.textPublisher.Location = new System.Drawing.Point(111, 79);
            this.textPublisher.Name = "textPublisher";
            this.textPublisher.Size = new System.Drawing.Size(270, 20);
            this.textPublisher.TabIndex = 5;
            // 
            // labelPublisher
            // 
            this.labelPublisher.AutoSize = true;
            this.labelPublisher.Location = new System.Drawing.Point(18, 82);
            this.labelPublisher.Name = "labelPublisher";
            this.labelPublisher.Size = new System.Drawing.Size(50, 13);
            this.labelPublisher.TabIndex = 4;
            this.labelPublisher.Text = "Publisher";
            // 
            // textHubName
            // 
            this.textHubName.Location = new System.Drawing.Point(111, 53);
            this.textHubName.Name = "textHubName";
            this.textHubName.Size = new System.Drawing.Size(270, 20);
            this.textHubName.TabIndex = 3;
            // 
            // labelHubName
            // 
            this.labelHubName.AutoSize = true;
            this.labelHubName.Location = new System.Drawing.Point(18, 56);
            this.labelHubName.Name = "labelHubName";
            this.labelHubName.Size = new System.Drawing.Size(58, 13);
            this.labelHubName.TabIndex = 2;
            this.labelHubName.Text = "Hub Name";
            // 
            // textNamespace
            // 
            this.textNamespace.Location = new System.Drawing.Point(111, 27);
            this.textNamespace.Name = "textNamespace";
            this.textNamespace.Size = new System.Drawing.Size(270, 20);
            this.textNamespace.TabIndex = 1;
            // 
            // labelNamespace
            // 
            this.labelNamespace.AutoSize = true;
            this.labelNamespace.Location = new System.Drawing.Point(18, 30);
            this.labelNamespace.Name = "labelNamespace";
            this.labelNamespace.Size = new System.Drawing.Size(64, 13);
            this.labelNamespace.TabIndex = 0;
            this.labelNamespace.Text = "Namespace";
            // 
            // groupCredentials
            // 
            this.groupCredentials.Controls.Add(this.textTTL);
            this.groupCredentials.Controls.Add(this.label2);
            this.groupCredentials.Controls.Add(this.textSenderKey);
            this.groupCredentials.Controls.Add(this.labelSenderKeyName);
            this.groupCredentials.Controls.Add(this.textSenderKeyName);
            this.groupCredentials.Controls.Add(this.labelSenderKey);
            this.groupCredentials.Location = new System.Drawing.Point(421, 13);
            this.groupCredentials.Name = "groupCredentials";
            this.groupCredentials.Size = new System.Drawing.Size(402, 151);
            this.groupCredentials.TabIndex = 8;
            this.groupCredentials.TabStop = false;
            this.groupCredentials.Text = "Credentials";
            // 
            // textTTL
            // 
            this.textTTL.Location = new System.Drawing.Point(148, 79);
            this.textTTL.Name = "textTTL";
            this.textTTL.Size = new System.Drawing.Size(233, 20);
            this.textTTL.TabIndex = 5;
            this.textTTL.Text = "60";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Token TTL (minutes)";
            // 
            // textSenderKey
            // 
            this.textSenderKey.Location = new System.Drawing.Point(148, 53);
            this.textSenderKey.Name = "textSenderKey";
            this.textSenderKey.Size = new System.Drawing.Size(233, 20);
            this.textSenderKey.TabIndex = 3;
            // 
            // labelSenderKeyName
            // 
            this.labelSenderKeyName.AutoSize = true;
            this.labelSenderKeyName.Location = new System.Drawing.Point(18, 30);
            this.labelSenderKeyName.Name = "labelSenderKeyName";
            this.labelSenderKeyName.Size = new System.Drawing.Size(93, 13);
            this.labelSenderKeyName.TabIndex = 2;
            this.labelSenderKeyName.Text = "Sender Key Name";
            // 
            // textSenderKeyName
            // 
            this.textSenderKeyName.Location = new System.Drawing.Point(148, 27);
            this.textSenderKeyName.Name = "textSenderKeyName";
            this.textSenderKeyName.Size = new System.Drawing.Size(233, 20);
            this.textSenderKeyName.TabIndex = 1;
            // 
            // labelSenderKey
            // 
            this.labelSenderKey.AutoSize = true;
            this.labelSenderKey.Location = new System.Drawing.Point(18, 56);
            this.labelSenderKey.Name = "labelSenderKey";
            this.labelSenderKey.Size = new System.Drawing.Size(62, 13);
            this.labelSenderKey.TabIndex = 0;
            this.labelSenderKey.Text = "Sender Key";
            // 
            // groupSignature
            // 
            this.groupSignature.Controls.Add(this.buttonGenerate);
            this.groupSignature.Controls.Add(this.textSignature);
            this.groupSignature.Location = new System.Drawing.Point(13, 171);
            this.groupSignature.Name = "groupSignature";
            this.groupSignature.Size = new System.Drawing.Size(810, 150);
            this.groupSignature.TabIndex = 9;
            this.groupSignature.TabStop = false;
            this.groupSignature.Text = "Signature";
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(714, 116);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(75, 23);
            this.buttonGenerate.TabIndex = 1;
            this.buttonGenerate.Text = "Generate";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.OnGenerate);
            // 
            // textSignature
            // 
            this.textSignature.Location = new System.Drawing.Point(13, 26);
            this.textSignature.Margin = new System.Windows.Forms.Padding(10);
            this.textSignature.Multiline = true;
            this.textSignature.Name = "textSignature";
            this.textSignature.Size = new System.Drawing.Size(776, 87);
            this.textSignature.TabIndex = 0;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 338);
            this.Controls.Add(this.groupSignature);
            this.Controls.Add(this.groupCredentials);
            this.Controls.Add(this.groupHub);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Event Hubs - Signature Generator";
            this.groupHub.ResumeLayout(false);
            this.groupHub.PerformLayout();
            this.groupCredentials.ResumeLayout(false);
            this.groupCredentials.PerformLayout();
            this.groupSignature.ResumeLayout(false);
            this.groupSignature.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupHub;
        private System.Windows.Forms.Label labelNamespace;
        private System.Windows.Forms.TextBox textNamespace;
        private System.Windows.Forms.TextBox textPublisher;
        private System.Windows.Forms.Label labelPublisher;
        private System.Windows.Forms.TextBox textHubName;
        private System.Windows.Forms.Label labelHubName;
        private System.Windows.Forms.Label labelMode;
        private System.Windows.Forms.ComboBox comboMode;
        private System.Windows.Forms.GroupBox groupCredentials;
        private System.Windows.Forms.TextBox textTTL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textSenderKey;
        private System.Windows.Forms.Label labelSenderKeyName;
        private System.Windows.Forms.TextBox textSenderKeyName;
        private System.Windows.Forms.Label labelSenderKey;
        private System.Windows.Forms.GroupBox groupSignature;
        private System.Windows.Forms.TextBox textSignature;
        private System.Windows.Forms.Button buttonGenerate;
    }
}

