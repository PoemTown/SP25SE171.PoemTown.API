using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.QueryOptions.SortOptions.PoemSorts
{
    public enum GetPoemsSortOption
    {
        LikeCountAscending = 1,
        LikeCountDescending = 2,
        CommentCountAscending = 3,
        CommentCountDescending = 4,
        TypeAscending = 5,
        TypeDescending = 6,
    }
}
