using System;
using System.Collections.Generic;
using System.Text;

namespace Study.EventManager.Data.Contract
{
    public interface IContextManager
    {
        T CreateRepositiry<T>(string id = "")
            where T : IRepository;

        void Save(string id = "");
    }
}