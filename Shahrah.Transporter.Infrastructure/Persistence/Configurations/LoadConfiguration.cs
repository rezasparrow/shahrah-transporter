using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shahrah.Transporter.Domain.Entities;

namespace Shahrah.Transporter.Infrastructure.Persistence.Configurations
{
    public class LoadConfiguration : IEntityTypeConfiguration<Load>
    {
        public void Configure(EntityTypeBuilder<Load> builder)
        {
            builder.Property(p => p.Title).HasColumnType("nvarchar(128)").IsRequired();

            builder.HasData(new Load(1) { Title = "مواد غذایی ، آشامیدنی و محصولات وابسته" });
            builder.HasData(new Load(2) { Title = "کالاهای دامی " });
            builder.HasData(new Load(3) { Title = "محصولات کشاورزی " });
            builder.HasData(new Load(4) { Title = "انواع سموم و کود شیمیایی و طبیعی" });
            builder.HasData(new Load(5) { Title = "انواع دارو" });
            builder.HasData(new Load(6) { Title = "دخانیات" });
            builder.HasData(new Load(7) { Title = "انواع چرم، پوست، مو، پر و مصنوعات وابسته" });
            builder.HasData(new Load(8) { Title = "انواع چوب و مصنوعات وابسته" });
            builder.HasData(new Load(9) { Title = "انواع مقوا و کاغذ و مطبوعات و مصنوعات وابسته" });
            builder.HasData(new Load(10) { Title = "انواع ترکیبات نفتی و شیمیایی" });
            builder.HasData(new Load(11) { Title = "انواع رنگ و مواد پاک کننده" });
            builder.HasData(new Load(12) { Title = "سوخت، مواد منفجره و مواد قابل اشتعال" });
            builder.HasData(new Load(13) { Title = "محصولات پلاستیکی و کائوچو و محصولات وابسته" });
            builder.HasData(new Load(14) { Title = "لوازم و محصولات بهداشتی" });
            builder.HasData(new Load(15) { Title = "مواد سلولزی و محصولات وابسته" });
            builder.HasData(new Load(16) { Title = "لوازم و تجهیزات پزشکی" });
            builder.HasData(new Load(17) { Title = "انواع گونی و کیسه و چتایی" });
            builder.HasData(new Load(18) { Title = "انواع پوشاک و منسوجات آن" });
            builder.HasData(new Load(19) { Title = "انواع فرش و گلیم و موکت" });
            builder.HasData(new Load(20) { Title = "انواع سیمان" });
            builder.HasData(new Load(21) { Title = "مصنوعات سیمانی" });
            builder.HasData(new Load(22) { Title = "مصنوعات سفالی" });
            builder.HasData(new Load(23) { Title = "محصولات شیشه ای و چینی" });
            builder.HasData(new Load(24) { Title = "ضایعات ساختمانی - نخاله" });
            builder.HasData(new Load(25) { Title = "ضایعات ساختمانی - درب و پنجره" });
            builder.HasData(new Load(26) { Title = "کالاهای ساختمانی" });
            builder.HasData(new Load(27) { Title = "آهن آلات شامل میلگرد، تیرآهن، نبشی و قوطی" });
            builder.HasData(new Load(28) { Title = "کالاهای فلزی - ورق" });
            builder.HasData(new Load(29) { Title = "کالاهای فلزی - ورق رول" });
            builder.HasData(new Load(30) { Title = "کالاهای فلزی - شمش و بیلت" });
            builder.HasData(new Load(31) { Title = "کالاهای فلزی" });
            builder.HasData(new Load(32) { Title = "کالاهای معدنی و مواد معدنی استخراجی از معدن" });
            builder.HasData(new Load(33) { Title = "لوازم آموزشی و ورزشی و اسباب بازی و وسایل مربوطه" });
            builder.HasData(new Load(34) { Title = "ابزار آلات" });
            builder.HasData(new Load(35) { Title = "انواع قطعات یدکی وسایل نقلیه و ماشین آلات  و دستگاه های سبک و سنگین" });
            builder.HasData(new Load(36) { Title = "انواع ماشین آلات و تجهیزات مکانیکی" });
            builder.HasData(new Load(37) { Title = "خودرو، وسایل نقلیه باری و مسافری" });
            builder.HasData(new Load(38) { Title = "دستگاه های الکتریکی و الکترونیکی و قطعات آنها" });
            builder.HasData(new Load(39) { Title = "آلات و دستگاه های اپتیک، عکاسی، سینماتوگرافی، دقت سنجی، ساعت سازی، موسیقی و قطعات مربوطه" });
            builder.HasData(new Load(40) { Title = "اشیا هنری، کلکسیون و عتیقه" });
            builder.HasData(new Load(41) { Title = "لوازم خانگی" });
        }
    }
}