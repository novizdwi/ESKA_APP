using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ESKA_DI.Models._EF;

namespace Models
{ 
    public interface IMasterService<T_OBJECT,T_KEY>
    {
        void Add(T_OBJECT model);
        void Update(T_OBJECT model);
        void Delete(T_OBJECT model);
        void DeleteById(T_KEY id);

        T_OBJECT GetNewModel(); 
        T_OBJECT GetById(T_KEY id); 
        T_OBJECT NavFirst();
        T_OBJECT NavPrevious(T_KEY id);
        T_OBJECT NavNext(T_KEY id); 
        T_OBJECT NavLast();
    }
}