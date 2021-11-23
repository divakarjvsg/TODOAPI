using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApi.Database.Models;

namespace ToDoApi.DataAccess.Repositories.Contracts
{
    public interface ILabelsRepository
    {
        Task<Labels> AddLabels(Labels labels);
        Task<IEnumerable<Labels>> GetLabels(PageParmeters pageParmeters);
        Task DeleteLabel(int labelId);
        Task<Labels> GetLabelByName(string labelName);
        Task<Labels> GetLabel(int labelId);
        Task AssignLabel(Guid selectedGuid, List<Labels> selectedLabels);
        Task<List<Labels>> GetLabelByGuid(Guid assignedGuid);
        Task<Labels> UpdateLabels(Labels labels);
    }
}
