using System;
using ManejoInventario.Domain.Entities;

namespace ManejoInventario.Domain.Ports

    {
        public interface IGenericRepository<T>
        {
            List<T> ObtenerTodos();
            void Crear(T entity);
            void Actualizar(T entity);
            void Eliminar(int id);
        }
    }