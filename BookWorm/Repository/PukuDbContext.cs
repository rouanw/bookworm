﻿using System;
using System.Data.Entity;

namespace BookWorm.Repository
{
    public class PukuDbContext : DbContext
    {
        public virtual IDbSet<T> GetDbSet<T>() where T : Model<T>
        {
            throw new NotImplementedException();
        }
    }
}