using System.Collections.Generic;
using TaxService.Models;

namespace TaxService.Database
{
    public interface IDataBaseRepository
    {
        System.Threading.Tasks.Task<List<TaxDetails>> FetchTaxDetailsFromDB();
    }
}
