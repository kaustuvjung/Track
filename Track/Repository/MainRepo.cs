using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Track.Data;
using Track.Repository.Irepository;

namespace Track.Repository
{
    public class MainRepo<T> : Imainrepo<T> where T : class
    {
        private readonly Applicationdbcontext _db;
        DbSet<T> _dbset;
        public MainRepo(Applicationdbcontext db)
        {
            _db = db;
            _dbset= db.Set<T>();
        }
        public void Add(T obj)
        {
            _dbset.Add(obj);
        }

        public void Delete(T obj)
        {
            _dbset.Remove(obj);
        }

        public virtual void DeleteMost(List<T> list)
        {
            _dbset.RemoveRange(list);
        }

        public IEnumerable<T> getAll(string? prop)
        {
            IQueryable<T> one= _dbset.AsQueryable();
            if (!string.IsNullOrEmpty(prop))
            {
                foreach(var pop in prop.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    one=one.Include(pop);
                }
            }
            return one.ToList();
        }


        public T GetOne(Expression<Func<T, bool>> filter, string? prop)
        {
            IQueryable<T> one = _dbset.AsQueryable();
            one = one.Where(filter);
            if(prop != null) 
            {
                foreach(var pop in prop.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    one=one.Include(pop);
                }
            }
            return one.FirstOrDefault();
        }

        public IEnumerable<T> getSpecifics(Expression<Func<T, bool>> filter, string? prop)
        {
            IQueryable<T> one = _dbset.AsQueryable();
            one = one.Where(filter);
            if (prop != null)
            {
                foreach (var pop in prop.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    one = one.Include(pop);
                }
            }
            return one;
        }
    }
}
