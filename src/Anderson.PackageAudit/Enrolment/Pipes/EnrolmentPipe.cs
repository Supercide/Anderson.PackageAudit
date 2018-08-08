using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Enrolment.Errors;
using Anderson.PackageAudit.Enrolment.Models;
using Anderson.PackageAudit.Infrastructure;
using Anderson.PackageAudit.Tenants.Models;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Responses;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Enrolment.Pipes
{
    public class EnrolmentPipe : PipelineDefinition<EnrolmentRequest>
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<Tenant> _tenantCollection;

        public EnrolmentPipe(IMongoCollection<User> userCollection, IMongoCollection<Tenant> tenantCollection)
        {
            _userCollection = userCollection;
            _tenantCollection = tenantCollection;
        }

        public override async Task HandleAsync(EnrolmentRequest request, Context context, CancellationToken token)
        {
            var account = Thread.CurrentPrincipal.ToAccount();

            var accounts = new List<Account>
            {
                account
            };
                if (_tenantCollection.Find(x => x.Name == request.Name).Any())
                {
                    context.SetError(EnrolmentError.TenantNameInUse);
                    return;
                }

                await _tenantCollection.InsertOneAsync(new Tenant
                {
                    Name = request.Name,
                    Accounts = accounts
                }, cancellationToken: token);
            

            if (!_userCollection.Find(x => x.Accounts.Contains(account)).Any())
            {
                await _userCollection.InsertOneAsync(new User
                {
                    Accounts = accounts,
                    MarketingPreference = request.OptIntoMarketing
                }, cancellationToken: token);
            }

            context.SetResponse(new TenantResponse
            {
                Name = request.Name,
                CreatedAt = DateTime.UtcNow,
                Id = Guid.NewGuid()
            });
        }
    }
}
