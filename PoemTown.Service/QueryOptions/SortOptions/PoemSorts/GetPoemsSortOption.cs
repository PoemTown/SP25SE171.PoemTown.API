using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.QueryOptions.SortOptions.PoemSorts
{
    public enum GetPoemsSortOption
    {
        LikeCountAscending = 0,
        LikeCountDescending = 1,
        CommentCountAscending = 2,
        CommentCountDescending = 3,
        TypeAscending = 4,
        TypeDescending = 5,
    }
}
