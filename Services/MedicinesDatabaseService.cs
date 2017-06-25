using System;
using System.Threading.Tasks;
using Piller.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using SQLite;
using MvvmCross.Platform;
using MvvmCross.Plugins.File;
using System.IO;

namespace Piller.Services
{
    public abstract class MedicinesDatabaseService : IMedicineDatabaseService
    {
        private readonly IMvxFileStore fileStore = Mvx.Resolve<IMvxFileStore>();




        private SQLiteAsyncConnection connection;

        public MedicinesDatabaseService()
        {
            var dbFileName = PrepareDatabaseFile();
            this.connection = new SQLiteAsyncConnection(dbFileName, SQLiteOpenFlags.ReadOnly);


            this.connection.GetConnection();
        }

        protected abstract string PrepareDatabaseFile();

   
        public async Task<Data.Medicines> GetAsync(string KodEAN)
        {
            var smth = await this.connection.GetAsync<Data.Medicines>(KodEAN);
            return smth;
        }
        /*
        public Task<List<Data.Medicines>> Query(string kodEAN)
        {
            return this.connection.QueryAsync<Data.Medicines>("select * from ProduktLeczniczyOpakowanie where KodEAN = ?", kodEAN);
        }
        */
    }
}
