using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.Interfaces
{
    public interface IAchievementService
    {
        Task CreateMonthlyAchievementsAsync();
    }
}
