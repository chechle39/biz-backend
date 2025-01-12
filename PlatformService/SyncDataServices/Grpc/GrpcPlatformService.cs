using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;
        public GrpcPlatformService(IPlatformRepo platformRepo, IMapper mapper) 
        { 
            _platformRepo = platformRepo;
            _mapper = mapper;
        }

        public override Task<PlatformRespone> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var response = new PlatformRespone();
            var platforms = _platformRepo.GetAllPlatform();
            foreach (var plat in platforms)
            {
                response.Plaform.Add(_mapper.Map<GrpcPlatformModel>(plat));
            }

            return Task.FromResult(response);
        }
    }
}
