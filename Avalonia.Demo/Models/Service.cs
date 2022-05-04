using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Avalonia.Demo.Contracts;
using Avalonia.Media;
using JetBrains.Annotations;

namespace Avalonia.Demo.Models
{
    public partial class Service
    {
        public Service()
        {
            ClientServices = new HashSet<ClientService>();
            ServicePhotos = new HashSet<ServicePhoto>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Cost { get; set; }

        [Range(1, 14400, ErrorMessage = "Время должно быть в диапазоне 1-14400 секунд")]
        public int DurationInSeconds { get; set; }

        public string? Description { get; set; }
        public double? Discount { get; set; }
        public string? MainImagePath { get; set; }

        [NotMapped]
        public string DurationInMinutesString
        {
            get
            {
                var time = DurationInSeconds / 60;
                var duration = GetCorrectTime(time);
                if (IsDiscount)
                {
                    return DiscountCost is null ? null : $"{GetCorrectName(DiscountCost)} за {duration}";
                }

                return IsDiscount ? null : $"{GetCorrectName(Cost)} за {duration}";
            }
        }

        [NotMapped] public SolidColorBrush? DiscountBrush => IsDiscount ? new SolidColorBrush(Colors.LightGreen) : null;

        [NotMapped]
        public decimal? DiscountCost
        {
            get
            {
                if (IsDiscount) return Cost - Cost * (decimal)Discount / 100;
                return null;
            }
        }

        [NotMapped]
        public bool IsDiscount
        {
            get
            {
                if (Discount is > 0)
                    return true;
                return false;
            }
        }

        [NotMapped] public bool IsVisible => Helper.CurrentRole == Roles.Admin;

        //This needs to be completed
        private string GetCorrectTime(int time)
        {
            string message = string.Empty;
            if (FindNumberOfDigit(time) == 1)
            {
                if (time == 1)
                    message = "минута";
                if (time is > 1 and < 5)
                    message = "минуты";
                if (time is > 4 or 0)
                    message = "минут";
            }
            else
            {
                message = "минут";
            }

            return $"{time} " + message;
        }

        private string GetCorrectName(decimal? cost)
        {
            string message = string.Empty;
            if (cost != null && FindNumberOfDigit((int)cost.Value) == 1)
            {
                if (cost == 1)
                    message = " рубль";
                if (cost is > 1 and < 5)
                    message = " рубля";
                if (cost is > 4 or 0)
                    message = " рублей";
            }
            else
            {
                if (cost % 10 == 1)
                    message = " рубль";
                if (cost % 10 is > 1 and < 5)
                    message = " рубля";
                if (cost % 10 is > 4 or 0)
                    message = " рублей";
            }

            return message;
        }

        private int FindNumberOfDigit(int number)
        {
            int count = 0;
            while (number > 0)
            {
                number /= 10;
                count++;
            }

            return count;
        }

        [NotMapped]
        public TextDecorationCollection ServiceDecoration => IsDiscount ? TextDecorations.Strikethrough : null;

        public virtual ICollection<ClientService> ClientServices { get; set; }
        public virtual ICollection<ServicePhoto> ServicePhotos { get; set; }
    }
}