using System;
using System.Collections.Generic;

namespace Avalonia.Demo.Models
{
    public partial class Client
    {
        public Client()
        {
            ClientServices = new HashSet<ClientService>();
            Tags = new HashSet<Tag>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public DateOnly? Birthday { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; } = null!;
        public string GenderCode { get; set; } = null!;
        public string? PhotoPath { get; set; }

        public virtual Gender GenderCodeNavigation { get; set; } = null!;
        public virtual ICollection<ClientService> ClientServices { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}