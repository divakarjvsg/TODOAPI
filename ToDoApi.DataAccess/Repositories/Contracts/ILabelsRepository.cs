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
        Task DeleteLabel(int LabelId);
        Task<Labels> GetLabelByName(string LabelName);
        Task<Labels> GetLabel(int Id);
        Task AssignLabel(Guid SelectedGuid, List<Labels> SelectedLabels);
        Task<List<Labels>> GetLabelByGuid(Guid AssignedGuid);        
    }
}
