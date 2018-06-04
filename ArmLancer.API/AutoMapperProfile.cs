using ArmLancer.API.Models.Requests;
using ArmLancer.Data.Models;
using AutoMapper;

namespace ArmLancer.API
{
    public class AutoMapperProfile : Profile
    {
        private void CreateMapsFor<TSource, TDest>()
        {
            CreateMap<TSource, TDest>();
            CreateMap<TDest, TSource>();
        }

        public AutoMapperProfile()
        {
            CreateMapsFor<Category, CategoryRequest>();
            CreateMapsFor<Job, JobRequest>();
            CreateMapsFor<JobSubmission, JobSubmissionRequest>();
        }
    }
}