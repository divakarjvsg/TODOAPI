using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Models;

namespace TodoAPI.Repositories
{
    public interface ILabelRepository
    {
        Task<Labels> AddLabels(Labels labels);
        Task<IEnumerable<Labels>> GetLabels();
        Task DeleteLabel(int LabelId);
        Task<Labels> GetLabelByName(string LabelName);
        Task<Labels> GetLabel(int Id);
        Task AssignLabel(Guid SelectedGuid, List<Labels> SelectedLabels);
    }
}
