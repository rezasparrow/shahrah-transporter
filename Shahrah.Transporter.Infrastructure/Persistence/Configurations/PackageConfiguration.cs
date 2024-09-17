using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations;

public class PackageConfiguration : IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> builder)
    {
        builder.Property(p => p.Title).HasColumnType("nvarchar(128)").IsRequired();

        builder.HasData(new Package(1) { Title = "ایزوتانک" });
        builder.HasData(new Package(2) { Title = "بشکه" });
        builder.HasData(new Package(3) { Title = "بندل" });
        builder.HasData(new Package(4) { Title = "پالت" });
        builder.HasData(new Package(5) { Title = "پاکت" });
        builder.HasData(new Package(6) { Title = "تانکری" });
        builder.HasData(new Package(7) { Title = "تیغه" });
        builder.HasData(new Package(8) { Title = "جعبه" });
        builder.HasData(new Package(9) { Title = "حلب" });
        builder.HasData(new Package(10) { Title = "حلقه" });
        builder.HasData(new Package(11) { Title = "دستگاه" });
        builder.HasData(new Package(12) { Title = "راس" });
        builder.HasData(new Package(13) { Title = "رول" });
        builder.HasData(new Package(14) { Title = "سبد" });
        builder.HasData(new Package(15) { Title = "سیلندر" });
        builder.HasData(new Package(16) { Title = "شاخه" });
        builder.HasData(new Package(17) { Title = "صندوق" });
        builder.HasData(new Package(18) { Title = "عدل" });
        builder.HasData(new Package(19) { Title = "فاقد بسته بندی" });
        builder.HasData(new Package(20) { Title = "فله" });
        builder.HasData(new Package(21) { Title = "قالب" });
        builder.HasData(new Package(22) { Title = "قرقره" });
        builder.HasData(new Package(23) { Title = "قوطی" });
        builder.HasData(new Package(24) { Title = "گونی" });
        builder.HasData(new Package(25) { Title = "نگله" });
        builder.HasData(new Package(26) { Title = "کارتن" });
        builder.HasData(new Package(27) { Title = "کانتینر" });
        builder.HasData(new Package(28) { Title = "کانتینر ۲۰ فوت" });
        builder.HasData(new Package(29) { Title = "کانتینر ۴۰ فوت" });
        builder.HasData(new Package(30) { Title = "کانتینر ۲۰ فوت جفتی" });
        builder.HasData(new Package(31) { Title = "کانتینر ۴۰ فوت فلت" });
        builder.HasData(new Package(32) { Title = "کانتینر ۴۵ فوت" });
        builder.HasData(new Package(33) { Title = "کلاف" });
        builder.HasData(new Package(34) { Title = "کیسه" });
        builder.HasData(new Package(35) { Title = "کیسه جامبو" });
        builder.HasData(new Package(36) { Title = "یخچالی" });
        builder.HasData(new Package(37) { Title = "سایر" });
    }
}