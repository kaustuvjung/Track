using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Track.Repository.Irepository
{
    public interface Imainrepo<T> where T : class 
    {
        public void Add(T obj);
        public void Delete(T obj);

        public IEnumerable<T> getAll(string? prop);

        public IEnumerable<T> getSpecifics(Expression<Func<T,bool>> filter,string? prop);
        public T GetOne(Expression<Func<T,bool>> filter,string? prop);

        public void DeleteMost(List<T> list);

    }
}
