using System.Collections.Generic;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.PackageModels;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Responses;

namespace Anderson.PackageAudit.Tests
{
    public class TestHandler : PipelineDefinition<AuditRequest, Response<AuditResponse, Error>>
    {
        private readonly PackageSummary[] _response;

        public TestHandler(params PackageSummary[] packageSummaries)
        {
            _response = packageSummaries;
        }
        public override Response<AuditResponse, Error> Handle(AuditRequest request)
        {
            return new AuditResponse
            {
                Packages = _response 
            };
        }
    }
}