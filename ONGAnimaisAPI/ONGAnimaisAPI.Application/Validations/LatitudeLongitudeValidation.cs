using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisAPI.Application.Validations
{
    internal class LatitudeLongitudeValidation : AbstractValidator<(decimal Latitude, decimal Longitude)>
    {
        public LatitudeLongitudeValidation()
        {
            RuleFor(geo => geo.Latitude)
                .NotEmpty().WithMessage("Longitude: por favor, preencha o campo Latitude");

            RuleFor(geo => geo.Longitude)
                .NotEmpty().WithMessage("Longitude: por favor, preencha o campo Longitude");
        }
    }
}
