using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reactive;
using Avalonia.Media;
using JetBrains.Annotations;

namespace Avalonia.Demo.Models
{
    public partial class ClientService
    {
        public ClientService()
        {
            DocumentByServices = new HashSet<DocumentByService>();
            ProductSales = new HashSet<ProductSale>();
        }

        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ServiceId { get; set; }
        public DateTime StartTime { get; set; }
        public string? Comment { get; set; }

        [NotMapped]
        [CanBeNull]
        public SolidColorBrush RowColor
        {
            get
            {
                var time = GetTime();
                if (time.Hour==0&&time.Day<2)
                {
                    return new SolidColorBrush(Colors.Red);
                }

                return new SolidColorBrush(Colors.Black);
            }
        }

        public string LostTime
        {
            get
            {
                var time = GetTime();
                return time.Day > 1 ? $"{time.Day - 1}d {time.ToShortTimeString()}" : time.ToShortTimeString();
            }
        }

        private DateTime GetTime()
        {
            DateTime date = DateTime.Now;
            TimeSpan diff = StartTime.Subtract(date);
            var time = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddTicks(diff.Ticks);
            return time;
        }
        public virtual Client Client { get; set; } = null!;
        public virtual Service Service { get; set; } = null!;
        public virtual ICollection<DocumentByService> DocumentByServices { get; set; }
        public virtual ICollection<ProductSale> ProductSales { get; set; }
    }
}