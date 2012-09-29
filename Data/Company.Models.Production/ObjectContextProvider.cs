using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using Company.Infrastructure.DataAccessLayer;

namespace Company.Models.Production
{
    public class ObjectContextProvider : IObjectContextProvider
    {
        #region IObjectContextProvider Members

        public System.Data.Objects.ObjectContext ObjectContext
        {
            get { return ((IObjectContextAdapter)new Company.Models.Production.CompanyProductsContext()).ObjectContext; }
        }

        #endregion
    }
}
