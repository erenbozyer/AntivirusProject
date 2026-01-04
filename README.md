# 🛡️ Anti-Virus Scanner

## 🚀 Özellikler

* **Dinamik İmza Veritabanı:** Proje dizinindeki `virusSignatures.txt` dosyasından MD5 imzalarını otomatik olarak çeker.
* **Derinlemesine Tarama:** Seçilen klasör altındaki tüm alt klasörleri (recursive) tarayabilir.
* **MD5 Hash Hesaplama:** Dosyaların benzersiz parmak izlerini çıkarmak için `System.Security.Cryptography` kütüphanesini kullanır.
* **Kullanıcı Dostu Arayüz:** Tarama sürecini gösteren progress bar ve detaylı sonuç listesi (ListBox) içerir.
* **Hata Yönetimi:** Erişim yetkisi olmayan veya sistem tarafından kullanılan dosyalar için hata yakalama (try-catch) mekanizması barındırır.

## 🛠️ Teknik Detaylar

* **Dil:** C#
* **Platform:** .NET Framework / Windows Forms
* **Geliştirme Ortamı:** Visual Studio

## 📋 Nasıl Çalışır?

1. Uygulama başlatıldığında proje dizininde `virusSignatures.txt` dosyasını arar. Bulamazsa otomatik olarak oluşturur.
2. Kullanıcı "Klasör Seç" butonu ile taranacak dizini belirler.
3. "Taramayı Başlat" butonuna basıldığında, dizindeki her bir dosyanın MD5 özeti hesaplanır.
4. Hesaplanan özet, veritabanındaki (txt dosyası) "kara liste" imzalarıyla karşılaştırılır.
5. Eğer eşleşme varsa, dosya "TEHLİKE" etiketiyle kullanıcıya raporlanır.

## 🧪 Test Etme

Uygulamayı test etmek için:

1. Herhangi bir metin dosyasının MD5 hash kodunu öğrenin.
2. Bu kodu `virusSignatures.txt` dosyasının içine yeni bir satır olarak ekleyin.
3. Uygulama üzerinden o dosyayı tarattığınızda yazılımın dosyayı tehdit olarak algıladığını göreceksiniz.
   
