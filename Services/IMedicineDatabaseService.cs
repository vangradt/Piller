using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Piller.Services
{
    public interface IMedicineDatabaseService
    {
        Task<Data.Medicines> GetAsync(string KodEAN);
        //Task<List<Data.Medicines>> Query(string kodEAN);
    }
}
