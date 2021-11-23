using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Context;
using ToDoApi.Database.Models;

namespace ToDoApi.DataAccess.Repositories
{
    public class LabelsRepository : ILabelsRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly Guid _loginUser;

        public LabelsRepository(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            if (httpContextAccessor.HttpContext.User.Identity is ClaimsIdentity identity)
            {
                IEnumerable<Claim> claims = identity.Claims;
                _loginUser = new Guid(claims.ToList()[0].Value);
            }
        }

        public async Task<Labels> AddLabels(Labels labels)
        {
            labels.CreatedBy = _loginUser;
            var result = await _appDbContext.Labels.AddAsync(labels);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteLabel(int labelId)
        {
            var result = await _appDbContext.Labels
                .FirstOrDefaultAsync(x => x.LabelId == labelId);
            if (result != null)
            {
                _appDbContext.Labels.Remove(result);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<Labels> GetLabelByName(string labelName)
        {
            return await _appDbContext.Labels
                .FirstOrDefaultAsync(x => x.LabelName == labelName && x.CreatedBy == _loginUser);
        }

        public async Task<Labels> GetLabel(int labelId)
        {
            return await _appDbContext.Labels
                .FirstOrDefaultAsync(x => x.LabelId == labelId && x.CreatedBy == _loginUser);
        }

        public async Task<IEnumerable<Labels>> GetLabels(PageParmeters pageParmeters)
        {
            IQueryable<Labels> query = _appDbContext.Labels.Where(x => x.CreatedBy == _loginUser);
            var result = await query.Skip((pageParmeters.PageNumber - 1) * pageParmeters.PageSize).Take(pageParmeters.PageSize).ToListAsync();
            return result;
        }

        public async Task AssignLabel(Guid selectedGuid, List<Labels> selectedLabels)
        {
            for (int i = 0; i < selectedLabels.Count; i++)
            {
                AssignLabels assign = new AssignLabels
                {
                    LabelId = Convert.ToInt32(selectedLabels[i].LabelId),
                    AssignedGuid = selectedGuid
                };
                await _appDbContext.AssignLabels.AddAsync(assign);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Labels>> GetLabelByGuid(Guid assignedGuid)
        {
            var labelsResult = await _appDbContext.AssignLabels.Where(x => x.AssignedGuid == assignedGuid).ToListAsync();
            List<Labels> labels = new List<Labels>();
            foreach (var assignedlabel in labelsResult)
            {
                var LabelFetched = await GetLabel(assignedlabel.LabelId);
                labels.Add(LabelFetched);
            }
            return labels;
        }

        public async Task<Labels> UpdateLabels(Labels labels)
        {
            var result = await _appDbContext.Labels
               .FirstOrDefaultAsync(x => x.LabelId == labels.LabelId);
            if (result != null)
            {
                result.LabelName = labels.LabelName;
                await _appDbContext.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
