using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoTrack.Contracts;

namespace TodoTrack.Cli
{
    internal class KeyedList<T> : List<T>
        where T : class, IEntity
    {
        public T? this[string id]
        {
            get
            {
                /* return the specified index here */
                return Find(w => w.Id == id);
            }
        }
    }
}
