using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strack.Data.Queryable;

public delegate IQueryable<TEntity> EntityQueryableOptionHandler<TEntity>(IQueryable<TEntity> queryable);
