using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Anderson.PackageAudit.Audit.Models;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Infrastructure.Authorization;
using Anderson.PackageAudit.Keys.Models;
using Anderson.PackageAudit.SharedPipes.Authorization.Errors;
using Anderson.Pipelines.Definitions;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Audit.Pipes
{
    public class AuditPackagePipe : PipelineDefinition<IList<AuditPackageRequest>>
    {
        private readonly IMongoCollection<Key> _keyCollection;

        public AuditPackagePipe(IMongoCollection<Key> keyCollection)
        {
            _keyCollection = keyCollection;
        }

        public override Task HandleAsync(IList<AuditPackageRequest> request,
            Context context, CancellationToken token = default(CancellationToken))
        {
            if (/*IsAuthorized(context)*/true)
            {
                IList<Vulnerability> response = new List<Vulnerability>
                {
                    new Vulnerability
                    {
                        Name = "ReDoS",
                        Package = "FluentValidation",
                        Version = "1.2.4",
                        Classification = Classification.High,
                        Info = "www.google.com"
                    },
                    new Vulnerability
                    {
                        Name = "ReDoS",
                        Package = "FluentValidation",
                        Version = "1.2.4",
                        Classification = Classification.Low,
                        Info = "www.google.com"
                    },
                    new Vulnerability
                    {
                        Name = "ReDoS",
                        Package = "FluentValidation",
                        Version = "1.2.4",
                        Classification = Classification.Suppressed,
                        Info = "www.google.com"
                    },
                };

                context.SetResponse(response);
                return Task.CompletedTask;
            }

            context.SetError(AuthorizationErrors.Unauthorized);
            return Task.CompletedTask;
        }

        private bool IsAuthorized(Context context)
        {
            return _keyCollection.Find(x => x.Id == Guid.Parse((string)context[WellKnownContextKeys.ApiKey]))
                .Any();
        }
    }
}