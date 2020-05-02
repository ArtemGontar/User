using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Shared.Common;

namespace User.Application.Update
{
    public class UpdateUserCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public EnglishLevel EnglishLevel { get; set; }

        public DateTime BirthDate { get; set; }

        public string Departament { get; set; }

        public string JobTitle { get; set; }

        public string PhoneNumber { get; set; }
    }
}
