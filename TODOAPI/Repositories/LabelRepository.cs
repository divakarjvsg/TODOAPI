using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Context;
using TodoAPI.Models;

namespace TodoAPI.Repositories
{
    public class LabelRepository : ILabelRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Guid LoginUser;
        public LabelRepository(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.appDbContext = appDbContext;

            _httpContextAccessor = httpContextAccessor;
            LoginUser = new Guid(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
        public async Task<Labels> AddLabels(Labels labels)
        {
            labels.CreatedBy = LoginUser;
            var result = await appDbContext.Labels.AddAsync(labels);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteLabel(int LabelId)
        {
            var result = await appDbContext.Labels
                .FirstOrDefaultAsync(x => x.LabelId == LabelId);

            if (result != null)
            {
                appDbContext.Labels.Remove(result);
                await appDbContext.SaveChangesAsync();
            }
        }

        public async Task<Labels> GetLabelByName(string LabelName)
        {
            return await appDbContext.Labels
                .FirstOrDefaultAsync(x => x.LabelName == LabelName && x.CreatedBy==LoginUser);
        }

        public async Task<Labels> GetLabel(int Id)
        {
            return await appDbContext.Labels
                .FirstOrDefaultAsync(x => x.LabelId == Id && x.CreatedBy==LoginUser);
        }

        public async Task<IEnumerable<Labels>> GetLabels()
        {
            IQueryable <Labels> query=  appDbContext.Labels.Where(x => x.CreatedBy == LoginUser);
            var result = await query.ToListAsync();
            return result;
        }

        public async Task AssignLabel(Guid SelectedGuid, List<Labels> SelectedLabels)
        {
            for (int i = 0; i < SelectedLabels.Count(); i++)
            {
                AssignLabels assign = new AssignLabels();
                assign.LabelId = Convert.ToInt32(SelectedLabels[i].LabelId);
                assign.AssignedGuid = SelectedGuid;
                var result = await appDbContext.AssignLabels.AddAsync(assign);
                await appDbContext.SaveChangesAsync();             
            }
        }
    }
}
