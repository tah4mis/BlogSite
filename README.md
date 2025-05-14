# ASP.NET Core Blog Sitesi

Bu proje ASP.NET Core MVC kullanılarak geliştirilmiş bir blog sitesidir. Kullanıcılar makale yazabilir, düzenleyebilir ve kategorilere göre filtreleyebilir.

## 🚀 Özellikler

- 👤 Kullanıcı kaydı ve girişi
- 📝 Makale oluşturma, düzenleme ve silme
- 🏷️ Kategori bazlı filtreleme
- 🔍 Makale arama
- 📱 Responsive tasarım
- 📊 Sayfalama
- 🌐 Türkçe arayüz

## 🔑 Admin Girişi

Varsayılan admin kullanıcı bilgileri:

- **Kullanıcı Adı:** admin
- **Şifre:** Admin123!
- **Email:** admin@example.com

> ⚠️ **Not:** Güvenlik için, ilk girişten sonra admin şifresini değiştirmeniz önerilir.

## 🛠️ Kurulum

1. Projeyi klonlayın:
   ```bash
   git clone https://github.com/tah4mis/BlogSite.git
   ```

2. Proje dizinine gidin:
   ```bash
   cd BlogSite
   ```

3. Bağımlılıkları yükleyin:
   ```bash
   dotnet restore
   ```

4. Veritabanını oluşturun:
   ```bash
   dotnet ef database update
   ```

5. Projeyi çalıştırın:
   ```bash
   dotnet run
   ```

## 💻 Teknolojiler

- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server
- Bootstrap 5
- Identity Framework

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Daha fazla bilgi için [LICENSE](LICENSE) dosyasına bakın. 