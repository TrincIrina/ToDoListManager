using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListManager.DB
{
    internal interface IDealsRepository
    {
        // Добавление дела в список
        void AddCase(Deals deals);
        // Удаление дела
        void DeleteCase(int id);
        // Редактирование дела
        void EditCase(Deals deals);
        // Вывод списка
        List<Deals> GetAll();
        // Сортировка по приоритету
        List<Deals> OrderByPriority();
        // Сортировка по дате дедлайна
        List<Deals> OrderByDeadline();
       
    }
}
