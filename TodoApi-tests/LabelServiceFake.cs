using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.Models;
using TodoAPI.Repositories;

namespace TodoApi_tests
{
    class LabelServiceFake : ILabelRepository
    {
        private readonly List<AssignLabels> _assignlabels;

        public LabelServiceFake()
        {
            _assignlabels = new List<AssignLabels>() {
            new AssignLabels(){LabelId=1,AssignedGuid=new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),AssignId=1 },
            new AssignLabels(){LabelId=2,AssignedGuid=new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),AssignId=2 },
            new AssignLabels(){LabelId=1,AssignedGuid=new Guid("815accac-fd5b-478a-a9d6-f171a2f6ae7f"),AssignId=3 },
            new AssignLabels(){LabelId=3,AssignedGuid=new Guid("33704c4a-5b87-464c-bfb6-51971b4d18ad"),AssignId=4 },
            };
        }
        Task<Labels> ILabelRepository.AddLabels(Labels labels)
        {
            throw new NotImplementedException();
        }

        Task ILabelRepository.AssignLabel(Guid SelectedGuid, List<Labels> SelectedLabels)
        {
            throw new NotImplementedException();
        }

        Task ILabelRepository.DeleteLabel(int LabelId)
        {
            throw new NotImplementedException();
        }

        Task<Labels> ILabelRepository.GetLabel(int Id)
        {
            throw new NotImplementedException();
        }

        Task<Labels> ILabelRepository.GetLabelByName(string LabelName)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Labels>> ILabelRepository.GetLabels()
        {
            throw new NotImplementedException();
        }
    }
}
