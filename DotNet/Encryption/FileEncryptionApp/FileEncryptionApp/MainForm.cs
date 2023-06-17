using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace FileEncryptionApp
{
    public partial class MainForm : Form
    {
        private TextBox txtFilePath;
        private Button btnEncrypt;
        private Button btnDecrypt;
        private Button Button;
        private RadioButton rdoTripleDES;
        private RadioButton rdoAES;

        public MainForm()
        {
            InitializeComponent();
        }

        private byte[] Encrypt(SymmetricAlgorithm algorithm, string data)
        {
            ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);

            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(data);
                    }
                    return msEncrypt.ToArray();
                }
            }
        }

        private string Decrypt(SymmetricAlgorithm algorithm, byte[] data)
        {
            ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV);

            using (var msDecrypt = new MemoryStream(data))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            var filePath = txtFilePath.Text;
            if (File.Exists(filePath))
            {
                var fileContent = File.ReadAllText(filePath);
                byte[] encryptedContent;

                if (rdoTripleDES.Checked)
                {
                    var tripleDES = new TripleDESCryptoServiceProvider();
                    encryptedContent = Encrypt(tripleDES, fileContent);
                }
                else // AES is selected
                {
                    var aes = new AesCryptoServiceProvider();
                    encryptedContent = Encrypt(aes, fileContent);
                }

                var newFilePath = Path.Combine(Path.GetDirectoryName(filePath), "encrypted_" + Path.GetFileName(filePath));
                File.WriteAllBytes(newFilePath, encryptedContent);
            }
            else
            {
                MessageBox.Show("File not found");
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            var filePath = txtFilePath.Text;
            if (File.Exists(filePath))
            {
                var fileContent = File.ReadAllBytes(filePath);
                string decryptedContent;

                if (rdoTripleDES.Checked)
                {
                    var tripleDES = new TripleDESCryptoServiceProvider();
                    decryptedContent = Decrypt(tripleDES, fileContent);
                }
                else // AES is selected
                {
                    var aes = new AesCryptoServiceProvider();
                    decryptedContent = Decrypt(aes, fileContent);
                }

                var newFilePath = Path.Combine(Path.GetDirectoryName(filePath), "decrypted_" + Path.GetFileName(filePath));
                File.WriteAllText(newFilePath, decryptedContent);
            }
            else
            {
                MessageBox.Show("File not found");
            }
        }

        private void InitializeComponent()
        {
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.Button = new System.Windows.Forms.Button();
            this.rdoTripleDES = new System.Windows.Forms.RadioButton();
            this.rdoAES = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(259, 135);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(374, 26);
            this.txtFilePath.TabIndex = 0;
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(259, 189);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(90, 35);
            this.btnEncrypt.TabIndex = 1;
            this.btnEncrypt.Text = "Encrypt";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Location = new System.Drawing.Point(544, 189);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(89, 35);
            this.btnDecrypt.TabIndex = 2;
            this.btnDecrypt.Text = "Decrypt";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // Button
            // 
            this.Button.Location = new System.Drawing.Point(398, 189);
            this.Button.Name = "Button";
            this.Button.Size = new System.Drawing.Size(99, 35);
            this.Button.TabIndex = 3;
            this.Button.Text = "Browse";
            this.Button.UseVisualStyleBackColor = true;
            this.Button.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // rdoTripleDES
            // 
            this.rdoTripleDES.AutoSize = true;
            this.rdoTripleDES.Location = new System.Drawing.Point(259, 247);
            this.rdoTripleDES.Name = "rdoTripleDES";
            this.rdoTripleDES.Size = new System.Drawing.Size(106, 24);
            this.rdoTripleDES.TabIndex = 4;
            this.rdoTripleDES.TabStop = true;
            this.rdoTripleDES.Text = "TripleDES";
            this.rdoTripleDES.UseVisualStyleBackColor = true;
            // 
            // rdoAES
            // 
            this.rdoAES.AutoSize = true;
            this.rdoAES.Location = new System.Drawing.Point(566, 247);
            this.rdoAES.Name = "rdoAES";
            this.rdoAES.Size = new System.Drawing.Size(67, 24);
            this.rdoAES.TabIndex = 5;
            this.rdoAES.TabStop = true;
            this.rdoAES.Text = "AES";
            this.rdoAES.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(965, 358);
            this.Controls.Add(this.rdoAES);
            this.Controls.Add(this.rdoTripleDES);
            this.Controls.Add(this.Button);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.btnEncrypt);
            this.Controls.Add(this.txtFilePath);
            this.Name = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = openFileDialog.FileName;
            }
        }
    }
}
