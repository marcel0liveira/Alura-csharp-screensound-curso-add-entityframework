﻿using Microsoft.EntityFrameworkCore;
using ScreenSound.Modelos;
using System.Linq;

namespace ScreenSound.Banco
{
    public class DAL<T> where T : class
    {
        #region contructor
        public readonly ScreenSoundContext context;

        public DAL(ScreenSoundContext context)
        {
            this.context = context;
        }
        #endregion

        #region basico
        public IEnumerable<T> Listar()
        {
            return context.Set<T>().ToList();
        }

        public void Adicionar(T objeto)
        {
            context.Set<T>().Add(objeto);
            context.SaveChanges();
        }

        public void Atualizar(T objeto)
        {
            context.Set<T>().Update(objeto);
            context.SaveChanges();
        }

        public void Deletar(T objeto)
        {
            context.Set<T>().Remove(objeto);
            context.SaveChanges();
        }
        #endregion

        #region Async
        public async Task<IEnumerable<T>> ListarAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task AdicionarAsync(T objeto)
        {
            await context.Set<T>().AddAsync(objeto);
            await context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(T objeto)
        {
            context.Set<T>().Update(objeto);
            await context.SaveChangesAsync();
        }

        public async Task RemoverAsync(T objeto)
        {
            context.Set<T>().Remove(objeto);
            await context.SaveChangesAsync();
        }

        #endregion

        #region especializado
        public T? RecuperarPor(Func<T, bool> condicao)
        {
            //int existe = context
            //.GetType()
            //.GetProperties()
            //.Where(x => x.PropertyType == typeof(Musica)).Count();

            //if (existe > 0)
            //{
            //    return context.Set<T>().Include( => x.Musicas).FirstOrDefault(condicao);
            //}
            IEnumerable<T> ret = context.Set<T>().Where(condicao).ToList();
            return ret.FirstOrDefault();
        }

        public IEnumerable<T> ListarPor(Func<T, bool> condicao)
        {
            return context.Set<T>().Where(condicao);
        }


        //public async Task<T?> RecuperarPorAsync(Func<T, bool> condicao, CancellationToken.None)
        //{
        //    return await context.Set<T>().FirstOrDefaultAsync(condicao);
        //}
        #endregion
    }
}
