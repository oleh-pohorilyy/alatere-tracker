using System.Collections.Generic;

namespace AlatereTracker.QLI
{
    public interface IQLInterpretator<Act>
    {
        IEnumerable<Act> Interpretate(string query);
    }
}
