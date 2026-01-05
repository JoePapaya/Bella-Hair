using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Enums
{
    public enum RabatType
    {
        Ingen = 0,
        Stamkunde = 1,
        Kampagne = 2
    }
    // Vi har enums til at repræsentere rabat- og kundetyper,
    // men i UI-filtreringen blev de ikke konsekvent anvendt.
    // I stedet bruges string-værdier ("Stamkunde", "Kampagne").
    // Med mere tid burde vi have brugt enums direkte for
    // bedre typesikkerhed og mindre risiko for fejl.
}
