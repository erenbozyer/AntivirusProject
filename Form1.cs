using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace AntivirusProject
{
    public partial class Form1 : Form
    {
        // Dinamik liste
        List<string> virusSignatures = new List<string>();

        // Dosya yolu: Programın çalıştığı klasördeki virusSignatures.txt
        string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "virusSignatures.txt");

        public Form1()
        {
            InitializeComponent();
            DatabaseHazirla(); // Form açılırken veritabanını kontrol et
        }

        private void DatabaseHazirla()
        {
            try
            {
                // Eğer dosya yoksa oluştur ve içine örnek bir imza yaz
                if (!File.Exists(dbPath))
                {
                    // Örnek bir MD5 (boş dosya imzası)
                    string[] varsayilanImzalar = { "e3b0c44298fc1c149afbf4c8996fb924" };
                    File.WriteAllLines(dbPath, varsayilanImzalar);
                    MessageBox.Show("Veritabanı dosyası bulunamadı ve yeni oluşturuldu: " + dbPath);
                }

                // Dosyadaki her satırı oku ve listeye aktar
                // Satır sonlarındaki boşlukları temizlemek için Select(x => x.Trim()) kullanıyoruz
                virusSignatures = File.ReadAllLines(dbPath)
                                      .Where(line => !string.IsNullOrWhiteSpace(line))
                                      .Select(line => line.Trim().ToLower())
                                      .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı yüklenirken hata oluştu: " + ex.Message);
            }
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtFolderPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnStartScan_Click(object sender, EventArgs e)
        {
            string path = txtFolderPath.Text;

            if (!Directory.Exists(path))
            {
                MessageBox.Show("Lütfen geçerli bir klasör seçin!");
                return;
            }

            lstResults.Items.Clear();
            lstResults.Items.Add("Tarama başlatıldı...");
            lstResults.Items.Add("Yüklü İmza Sayısı: " + virusSignatures.Count);

            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            progressBar1.Maximum = files.Length;
            progressBar1.Value = 0;

            int virusCount = 0;

            foreach (string file in files)
            {
                try
                {
                    string fileHash = GetMD5Hash(file);

                    // Listede bu hash var mı kontrol et
                    if (virusSignatures.Contains(fileHash))
                    {
                        lstResults.Items.Add("[TEHLİKE] Virüs Bulundu: " + Path.GetFileName(file));
                        virusCount++;
                    }
                }
                catch (Exception)
                {
                    // Dosya kullanımda olabilir veya erişim yetkisi olmayabilir
                    lstResults.Items.Add("[HATA] Okunamadı: " + Path.GetFileName(file));
                }

                progressBar1.Value++;
                Application.DoEvents();
            }

            lstResults.Items.Add("------------------------------------");
            lstResults.Items.Add("Tarama Tamamlandı. Bulunan Tehdit: " + virusCount);
        }

        public string GetMD5Hash(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}