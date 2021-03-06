﻿using System;
using System.Data;
using System.Data.Objects;

namespace Company.Infrastructure.DataAccessLayer
{
    public interface ICoreUnitOfWork : IDisposable, IObjectContextProvider, IUnitOfWork
    {        
    }

    public interface IUnitOfWork
    {
        bool IsInTransaction { get; }

        void SaveChanges();

        void BeginTransaction();

        void BeginTransaction(IsolationLevel isolationLevel);

        void RollBackTransaction();

        void CommitTransaction();
    }
}
