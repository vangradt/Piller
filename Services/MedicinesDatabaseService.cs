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
            xx();
        }

        protected abstract string PrepareDatabaseFile();

   
        public async Task<T> GetAsync<T>(long KodEAN) where T : new()
        {
            return await this.connection.FindAsync<T>(KodEAN);
        }
    }
}
